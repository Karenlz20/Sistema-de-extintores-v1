using Microsoft.Data.SqlClient;
using SistemaExtintores.Modelos;

namespace SistemaExtintores.Data
{
    public class InventarioData
    {
        // CILINDROS
        private static CilindroInventario MapearCilindro(SqlDataReader r)
        {
            CilindroInventario c = new CilindroInventario();
            c.Id            = (int)r["Id"];
            c.NroSerie      = r["NroSerie"].ToString();
            c.TipoAgente    = r["TipoAgente"].ToString();
            c.Capacidad     = (double)r["Capacidad"];
            c.PrecioUnitario = (double)r["PrecioUnitario"];
            c.Disponible    = (bool)r["Disponible"];
            return c;
        }

        public List<CilindroInventario> ObtenerCilindros()
        {
            List<CilindroInventario> lista = new List<CilindroInventario>();
            SqlConnection con = ConexionBD.Abrir();
            SqlCommand cmd = new SqlCommand("SELECT * FROM CilindrosInventario", con);
            SqlDataReader r = cmd.ExecuteReader();
            while (r.Read()) lista.Add(MapearCilindro(r));
            con.Close();
            return lista;
        }

        public CilindroInventario ObtenerCilindroPorId(int id)
        {
            SqlConnection con = ConexionBD.Abrir();
            SqlCommand cmd = new SqlCommand("SELECT * FROM CilindrosInventario WHERE Id = @Id", con);
            cmd.Parameters.AddWithValue("@Id", id);
            SqlDataReader r = cmd.ExecuteReader();
            CilindroInventario c = null;
            if (r.Read()) c = MapearCilindro(r);
            con.Close();
            return c;
        }

        public CilindroInventario ObtenerCilindroPorNroSerie(string nroSerie)
        {
            SqlConnection con = ConexionBD.Abrir();
            SqlCommand cmd = new SqlCommand("SELECT * FROM CilindrosInventario WHERE NroSerie = @NS", con);
            cmd.Parameters.AddWithValue("@NS", nroSerie);
            SqlDataReader r = cmd.ExecuteReader();
            CilindroInventario c = null;
            if (r.Read()) c = MapearCilindro(r);
            con.Close();
            return c;
        }

        public CilindroInventario InsertarCilindro(CilindroInventario c)
        {
            SqlConnection con = ConexionBD.Abrir();
            SqlCommand cmd = new SqlCommand(
                "INSERT INTO CilindrosInventario (NroSerie, TipoAgente, Capacidad, PrecioUnitario, Disponible) " +
                "VALUES (@NS, @TA, @Cap, @Precio, 1); SELECT SCOPE_IDENTITY();", con);
            cmd.Parameters.AddWithValue("@NS",     c.NroSerie);
            cmd.Parameters.AddWithValue("@TA",     c.TipoAgente);
            cmd.Parameters.AddWithValue("@Cap",    c.Capacidad);
            cmd.Parameters.AddWithValue("@Precio", c.PrecioUnitario);
            c.Id = Convert.ToInt32(cmd.ExecuteScalar());
            c.Disponible = true;
            con.Close();
            return c;
        }

        public bool ActualizarDisponibilidad(int id, bool disponible)
        {
            SqlConnection con = ConexionBD.Abrir();
            SqlCommand cmd = new SqlCommand(
                "UPDATE CilindrosInventario SET Disponible = @D WHERE Id = @Id", con);
            cmd.Parameters.AddWithValue("@D",  disponible);
            cmd.Parameters.AddWithValue("@Id", id);
            int filas = cmd.ExecuteNonQuery();
            con.Close();
            return filas > 0;
        }

        public bool EliminarCilindro(int id)
        {
            SqlConnection con = ConexionBD.Abrir();
            SqlCommand cmd = new SqlCommand("DELETE FROM CilindrosInventario WHERE Id = @Id", con);
            cmd.Parameters.AddWithValue("@Id", id);
            int filas = cmd.ExecuteNonQuery();
            con.Close();
            return filas > 0;
        }

        // INSUMOS
        private static InsumoInventario MapearInsumo(SqlDataReader r)
        {
            InsumoInventario i = new InsumoInventario();
            i.Id              = (int)r["Id"];
            i.Nombre          = r["Nombre"].ToString();
            i.Unidad          = r["Unidad"].ToString();
            i.Stock           = (double)r["Stock"];
            i.PuntoReorden    = (double)r["PuntoReorden"];
            i.EsPresurizante  = (bool)r["EsPresurizante"];
            return i;
        }

        public List<InsumoInventario> ObtenerInsumos()
        {
            List<InsumoInventario> lista = new List<InsumoInventario>();
            SqlConnection con = ConexionBD.Abrir();
            SqlCommand cmd = new SqlCommand("SELECT * FROM InsumosInventario", con);
            SqlDataReader r = cmd.ExecuteReader();
            while (r.Read()) lista.Add(MapearInsumo(r));
            con.Close();
            return lista;
        }

        public InsumoInventario ObtenerInsumoPorId(int id)
        {
            SqlConnection con = ConexionBD.Abrir();
            SqlCommand cmd = new SqlCommand("SELECT * FROM InsumosInventario WHERE Id = @Id", con);
            cmd.Parameters.AddWithValue("@Id", id);
            SqlDataReader r = cmd.ExecuteReader();
            InsumoInventario i = null;
            if (r.Read()) i = MapearInsumo(r);
            con.Close();
            return i;
        }

        public InsumoInventario InsertarInsumo(InsumoInventario i)
        {
            SqlConnection con = ConexionBD.Abrir();
            SqlCommand cmd = new SqlCommand(
                "INSERT INTO InsumosInventario (Nombre, Unidad, Stock, PuntoReorden, EsPresurizante) " +
                "VALUES (@N, @U, @S, @PR, @EP); SELECT SCOPE_IDENTITY();", con);
            cmd.Parameters.AddWithValue("@N",  i.Nombre);
            cmd.Parameters.AddWithValue("@U",  i.Unidad);
            cmd.Parameters.AddWithValue("@S",  i.Stock);
            cmd.Parameters.AddWithValue("@PR", i.PuntoReorden);
            cmd.Parameters.AddWithValue("@EP", i.EsPresurizante);
            i.Id = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            return i;
        }

        public InsumoInventario ObtenerInsumoPresurizante()
        {
            SqlConnection con = ConexionBD.Abrir();
            SqlCommand cmd = new SqlCommand(
                "SELECT TOP 1 * FROM InsumosInventario WHERE EsPresurizante = 1", con);
            SqlDataReader r = cmd.ExecuteReader();
            InsumoInventario i = null;
            if (r.Read()) i = MapearInsumo(r);
            con.Close();
            return i;
        }

        public bool ActualizarStock(int id, double nuevoStock)
        {
            SqlConnection con = ConexionBD.Abrir();
            SqlCommand cmd = new SqlCommand(
                "UPDATE InsumosInventario SET Stock = @S WHERE Id = @Id", con);
            cmd.Parameters.AddWithValue("@S",  nuevoStock);
            cmd.Parameters.AddWithValue("@Id", id);
            int filas = cmd.ExecuteNonQuery();
            con.Close();
            return filas > 0;
        }

        public bool EliminarInsumo(int id)
        {
            SqlConnection con = ConexionBD.Abrir();
            SqlCommand cmd = new SqlCommand("DELETE FROM InsumosInventario WHERE Id = @Id", con);
            cmd.Parameters.AddWithValue("@Id", id);
            int filas = cmd.ExecuteNonQuery();
            con.Close();
            return filas > 0;
        }

        // MOVIMIENTOS
        public MovimientoInventario InsertarMovimiento(TipoMovimiento tipo, TipoItemInventario tipoItem,
            int itemId, double cantidad, int usuarioId, string observacion)
        {
            SqlConnection con = ConexionBD.Abrir();
            SqlCommand cmd = new SqlCommand(
                "INSERT INTO MovimientosInventario (Fecha, Tipo, TipoItem, ItemId, Cantidad, UsuarioId, Observacion) " +
                "VALUES (@F, @Tipo, @TItem, @IId, @Cant, @UId, @Obs); SELECT SCOPE_IDENTITY();", con);
            cmd.Parameters.AddWithValue("@F",     DateTime.Now);
            cmd.Parameters.AddWithValue("@Tipo",  (int)tipo);
            cmd.Parameters.AddWithValue("@TItem", (int)tipoItem);
            cmd.Parameters.AddWithValue("@IId",   itemId);
            cmd.Parameters.AddWithValue("@Cant",  cantidad);
            cmd.Parameters.AddWithValue("@UId",   usuarioId);
            cmd.Parameters.AddWithValue("@Obs",   observacion ?? "");
            int movId = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();

            MovimientoInventario m = new MovimientoInventario(movId, tipo, itemId, tipoItem, cantidad, usuarioId, observacion);
            return m;
        }

        public List<MovimientoInventario> ObtenerMovimientos()
        {
            List<MovimientoInventario> lista = new List<MovimientoInventario>();
            SqlConnection con = ConexionBD.Abrir();
            SqlCommand cmd = new SqlCommand("SELECT * FROM MovimientosInventario ORDER BY Fecha DESC", con);
            SqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                MovimientoInventario m = new MovimientoInventario();
                m.Id        = (int)r["Id"];
                m.Fecha     = (DateTime)r["Fecha"];
                m.Tipo      = (TipoMovimiento)(int)r["Tipo"];
                m.TipoItem  = (TipoItemInventario)(int)r["TipoItem"];
                m.ItemId    = (int)r["ItemId"];
                m.Cantidad  = (double)r["Cantidad"];
                m.UsuarioId = (int)r["UsuarioId"];
                m.Observacion = r["Observacion"] == DBNull.Value ? "" : r["Observacion"].ToString();
                lista.Add(m);
            }
            con.Close();
            return lista;
        }
    }
}
