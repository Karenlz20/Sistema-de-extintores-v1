using Microsoft.Data.SqlClient;
using SistemaExtintores.Modelos;

namespace SistemaExtintores.Data
{
    public class ExtintoresData
    {
        private static string SQLExtintores =
            "SELECT e.Id, e.NroSerie, e.TipoAgente, e.Capacidad, e.FechaFabricacion, e.FechaUltimaCarga, " +
            "e.FechaVencimiento, e.ClienteId, e.SucursalId, e.CilindroInventarioId, " +
            "h.Id AS HojaId, h.FechaRegistroInicial, " +
            "r.Id AS RecargaId, r.Fecha AS RecargaFecha, r.AgenteUsado, r.CantidadKg, r.InsumoId, r.Tecnico, r.Observaciones " +
            "FROM Extintores e " +
            "LEFT JOIN HojasDeVida h ON h.ExtintorId = e.Id " +
            "LEFT JOIN Recargas r ON r.HojaDeVidaId = h.Id";

        private static List<Extintor> MapearConJoin(SqlDataReader r)
        {
            List<Extintor> lista = new List<Extintor>();
            int ultimoId = -1;

            while (r.Read())
            {
                int id = (int)r["Id"];

                if (id != ultimoId)
                {
                    Extintor e = new Extintor();
                    e.Id                  = id;
                    e.NroSerie            = r["NroSerie"].ToString();
                    e.TipoAgente          = r["TipoAgente"].ToString();
                    e.Capacidad           = (double)r["Capacidad"];
                    e.FechaFabricacion    = (DateTime)r["FechaFabricacion"];
                    e.FechaUltimaCarga    = (DateTime)r["FechaUltimaCarga"];
                    e.FechaVencimiento    = (DateTime)r["FechaVencimiento"];
                    e.ClienteId           = (int)r["ClienteId"];
                    e.SucursalId          = r["SucursalId"] == DBNull.Value ? null : (int?)r["SucursalId"];
                    e.CilindroInventarioId = r["CilindroInventarioId"] == DBNull.Value ? null : (int?)r["CilindroInventarioId"];

                    HojaDeVida h = new HojaDeVida();
                    h.Id = r["HojaId"] == DBNull.Value ? 0 : (int)r["HojaId"];
                    h.FechaRegistroInicial = r["FechaRegistroInicial"] == DBNull.Value
                        ? DateTime.Now : (DateTime)r["FechaRegistroInicial"];
                    e.HojaDeVida = h;

                    lista.Add(e);
                    ultimoId = id;
                }

                if (r["RecargaId"] != DBNull.Value)
                {
                    Extintor actual = lista[lista.Count - 1];
                    Recarga rec = new Recarga();
                    rec.Id           = (int)r["RecargaId"];
                    rec.Fecha        = (DateTime)r["RecargaFecha"];
                    rec.AgenteUsado  = r["AgenteUsado"] == DBNull.Value ? "" : r["AgenteUsado"].ToString();
                    rec.CantidadKg   = (double)r["CantidadKg"];
                    rec.InsumoId     = (int)r["InsumoId"];
                    rec.Tecnico      = r["Tecnico"] == DBNull.Value ? "" : r["Tecnico"].ToString();
                    rec.Observaciones = r["Observaciones"] == DBNull.Value ? "" : r["Observaciones"].ToString();
                    actual.HojaDeVida.AgregarRecarga(rec);
                }
            }

            return lista;
        }

        public List<Extintor> ObtenerTodos()
        {
            SqlConnection con = ConexionBD.Abrir();
            SqlCommand cmd = new SqlCommand(SQLExtintores + " ORDER BY e.Id, r.Id", con);
            SqlDataReader r = cmd.ExecuteReader();
            List<Extintor> lista = MapearConJoin(r);
            con.Close();
            return lista;
        }

        public Extintor ObtenerPorId(int id)
        {
            SqlConnection con = ConexionBD.Abrir();
            SqlCommand cmd = new SqlCommand(SQLExtintores + " WHERE e.Id = @Id ORDER BY r.Id", con);
            cmd.Parameters.AddWithValue("@Id", id);
            SqlDataReader r = cmd.ExecuteReader();
            List<Extintor> lista = MapearConJoin(r);
            con.Close();
            return lista.Count > 0 ? lista[0] : null;
        }

        public Extintor ObtenerPorNroSerie(string nroSerie)
        {
            SqlConnection con = ConexionBD.Abrir();
            SqlCommand cmd = new SqlCommand(SQLExtintores + " WHERE e.NroSerie = @NS ORDER BY r.Id", con);
            cmd.Parameters.AddWithValue("@NS", nroSerie);
            SqlDataReader r = cmd.ExecuteReader();
            List<Extintor> lista = MapearConJoin(r);
            con.Close();
            return lista.Count > 0 ? lista[0] : null;
        }

        public List<Extintor> ObtenerPorCliente(int clienteId)
        {
            SqlConnection con = ConexionBD.Abrir();
            SqlCommand cmd = new SqlCommand(SQLExtintores + " WHERE e.ClienteId = @CId ORDER BY e.Id, r.Id", con);
            cmd.Parameters.AddWithValue("@CId", clienteId);
            SqlDataReader r = cmd.ExecuteReader();
            List<Extintor> lista = MapearConJoin(r);
            con.Close();
            return lista;
        }

        public Extintor Insertar(Extintor e)
        {
            SqlConnection con = ConexionBD.Abrir();
            SqlCommand cmd = new SqlCommand(
                "INSERT INTO Extintores (NroSerie, TipoAgente, Capacidad, FechaFabricacion, FechaUltimaCarga, FechaVencimiento, ClienteId, SucursalId, CilindroInventarioId) " +
                "VALUES (@NS, @TA, @Cap, @FF, @FU, @FV, @CId, @SId, @CInvId); SELECT SCOPE_IDENTITY();", con);
            cmd.Parameters.AddWithValue("@NS",    e.NroSerie);
            cmd.Parameters.AddWithValue("@TA",    e.TipoAgente);
            cmd.Parameters.AddWithValue("@Cap",   e.Capacidad);
            cmd.Parameters.AddWithValue("@FF",    e.FechaFabricacion);
            cmd.Parameters.AddWithValue("@FU",    e.FechaUltimaCarga);
            cmd.Parameters.AddWithValue("@FV",    e.FechaVencimiento);
            cmd.Parameters.AddWithValue("@CId",   e.ClienteId);
            cmd.Parameters.AddWithValue("@SId",   (object)e.SucursalId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CInvId", (object)e.CilindroInventarioId ?? DBNull.Value);
            e.Id = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();

            HojaDeVida h = InsertarHoja(e.Id);
            e.HojaDeVida = h;
            return e;
        }

        private HojaDeVida InsertarHoja(int extintorId)
        {
            SqlConnection con = ConexionBD.Abrir();
            SqlCommand cmd = new SqlCommand(
                "INSERT INTO HojasDeVida (ExtintorId, FechaRegistroInicial) " +
                "VALUES (@EId, @F); SELECT SCOPE_IDENTITY();", con);
            cmd.Parameters.AddWithValue("@EId", extintorId);
            cmd.Parameters.AddWithValue("@F",   DateTime.Now);
            int hojaId = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();

            HojaDeVida h = new HojaDeVida();
            h.Id = hojaId;
            h.FechaRegistroInicial = DateTime.Now;
            return h;
        }

        public bool ActualizarFechas(int id, DateTime fechaUltimaCarga, DateTime fechaVencimiento)
        {
            SqlConnection con = ConexionBD.Abrir();
            SqlCommand cmd = new SqlCommand(
                "UPDATE Extintores SET FechaUltimaCarga=@FU, FechaVencimiento=@FV WHERE Id=@Id", con);
            cmd.Parameters.AddWithValue("@FU",  fechaUltimaCarga);
            cmd.Parameters.AddWithValue("@FV",  fechaVencimiento);
            cmd.Parameters.AddWithValue("@Id",  id);
            int filas = cmd.ExecuteNonQuery();
            con.Close();
            return filas > 0;
        }

        public bool Eliminar(int id)
        {
            SqlConnection con = ConexionBD.Abrir();
            // Eliminar recargas, hoja y extintor en orden
            SqlCommand cmdR = new SqlCommand(
                "DELETE r FROM Recargas r INNER JOIN HojasDeVida h ON h.Id = r.HojaDeVidaId WHERE h.ExtintorId = @Id", con);
            cmdR.Parameters.AddWithValue("@Id", id);
            cmdR.ExecuteNonQuery();

            SqlCommand cmdH = new SqlCommand("DELETE FROM HojasDeVida WHERE ExtintorId = @Id", con);
            cmdH.Parameters.AddWithValue("@Id", id);
            cmdH.ExecuteNonQuery();

            SqlCommand cmdE = new SqlCommand("DELETE FROM Extintores WHERE Id = @Id", con);
            cmdE.Parameters.AddWithValue("@Id", id);
            int filas = cmdE.ExecuteNonQuery();
            con.Close();
            return filas > 0;
        }

        public Recarga InsertarRecarga(int extintorId, Recarga rec)
        {
            // Obtener HojaDeVidaId
            SqlConnection con = ConexionBD.Abrir();
            SqlCommand cmdH = new SqlCommand("SELECT Id FROM HojasDeVida WHERE ExtintorId = @EId", con);
            cmdH.Parameters.AddWithValue("@EId", extintorId);
            int hojaId = Convert.ToInt32(cmdH.ExecuteScalar());
            con.Close();

            SqlConnection con2 = ConexionBD.Abrir();
            SqlCommand cmd = new SqlCommand(
                "INSERT INTO Recargas (HojaDeVidaId, Fecha, AgenteUsado, CantidadKg, InsumoId, Tecnico, Observaciones) " +
                "VALUES (@HId, @F, @AU, @Kg, @IId, @T, @Obs); SELECT SCOPE_IDENTITY();", con2);
            cmd.Parameters.AddWithValue("@HId", hojaId);
            cmd.Parameters.AddWithValue("@F",   rec.Fecha);
            cmd.Parameters.AddWithValue("@AU",  rec.AgenteUsado ?? "");
            cmd.Parameters.AddWithValue("@Kg",  rec.CantidadKg);
            cmd.Parameters.AddWithValue("@IId", rec.InsumoId);
            cmd.Parameters.AddWithValue("@T",   rec.Tecnico ?? "");
            cmd.Parameters.AddWithValue("@Obs", rec.Observaciones ?? "");
            rec.Id = Convert.ToInt32(cmd.ExecuteScalar());
            con2.Close();
            return rec;
        }
    }
}
