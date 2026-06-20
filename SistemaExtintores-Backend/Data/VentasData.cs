using Microsoft.Data.SqlClient;
using SistemaExtintores.Modelos;

namespace SistemaExtintores.Data
{
    public class VentasData
    {
        private static string SQLVentas =
            "SELECT v.Id, v.Fecha, v.ClienteId, v.SucursalId, v.UsuarioId, v.Descuento, v.Estado, v.NumeroComprobante, " +
            "i.Id AS ItemId, i.TipoItem, i.Cantidad, i.PrecioUnitario, " +
            "i.NroSerie, i.TipoAgente, i.Capacidad, i.NroSerieExtintor, i.InsumoId AS ItemInsumoId " +
            "FROM Ventas v LEFT JOIN ItemsVenta i ON i.VentaId = v.Id";

        private static List<Venta> MapearConJoin(SqlDataReader r)
        {
            List<Venta> lista = new List<Venta>();
            int ultimoId = -1;

            while (r.Read())
            {
                int id = (int)r["Id"];

                if (id != ultimoId)
                {
                    Venta v = new Venta();
                    v.Id               = id;
                    v.Fecha            = (DateTime)r["Fecha"];
                    v.ClienteId        = (int)r["ClienteId"];
                    v.SucursalId       = r["SucursalId"] == DBNull.Value ? null : (int?)r["SucursalId"];
                    v.UsuarioId        = (int)r["UsuarioId"];
                    v.Descuento        = (double)r["Descuento"];
                    v.Estado           = (EstadoVenta)(int)r["Estado"];
                    v.NumeroComprobante = r["NumeroComprobante"] == DBNull.Value ? "" : r["NumeroComprobante"].ToString();
                    lista.Add(v);
                    ultimoId = id;
                }

                if (r["ItemId"] != DBNull.Value)
                {
                    Venta actual = lista[lista.Count - 1];
                    string tipo = r["TipoItem"].ToString();
                    ItemVenta item;

                    if (tipo == "Extintor")
                    {
                        ExtintorItem ei = new ExtintorItem();
                        ei.NroSerie  = r["NroSerie"] == DBNull.Value ? "" : r["NroSerie"].ToString();
                        ei.TipoAgente = r["TipoAgente"] == DBNull.Value ? "" : r["TipoAgente"].ToString();
                        ei.Capacidad  = r["Capacidad"] == DBNull.Value ? 0 : (double)r["Capacidad"];
                        item = ei;
                    }
                    else
                    {
                        RecargaItem ri = new RecargaItem();
                        ri.NroSerieExtintor = r["NroSerieExtintor"] == DBNull.Value ? "" : r["NroSerieExtintor"].ToString();
                        ri.InsumoId         = r["ItemInsumoId"] == DBNull.Value ? 0 : (int)r["ItemInsumoId"];
                        item = ri;
                    }

                    item.Id            = (int)r["ItemId"];
                    item.Cantidad      = (int)r["Cantidad"];
                    item.PrecioUnitario = (double)r["PrecioUnitario"];
                    actual.AgregarItem(item);
                }
            }

            return lista;
        }

        public List<Venta> ObtenerTodas()
        {
            SqlConnection con = ConexionBD.Abrir();
            SqlCommand cmd = new SqlCommand(SQLVentas + " ORDER BY v.Id, i.Id", con);
            SqlDataReader r = cmd.ExecuteReader();
            List<Venta> lista = MapearConJoin(r);
            con.Close();
            return lista;
        }

        public Venta ObtenerPorId(int id)
        {
            SqlConnection con = ConexionBD.Abrir();
            SqlCommand cmd = new SqlCommand(SQLVentas + " WHERE v.Id = @Id ORDER BY i.Id", con);
            cmd.Parameters.AddWithValue("@Id", id);
            SqlDataReader r = cmd.ExecuteReader();
            List<Venta> lista = MapearConJoin(r);
            con.Close();
            return lista.Count > 0 ? lista[0] : null;
        }

        public List<Venta> ObtenerPorCliente(int clienteId)
        {
            SqlConnection con = ConexionBD.Abrir();
            SqlCommand cmd = new SqlCommand(SQLVentas + " WHERE v.ClienteId = @CId ORDER BY v.Id, i.Id", con);
            cmd.Parameters.AddWithValue("@CId", clienteId);
            SqlDataReader r = cmd.ExecuteReader();
            List<Venta> lista = MapearConJoin(r);
            con.Close();
            return lista;
        }

        public List<Venta> ObtenerPorFecha(DateTime desde, DateTime hasta)
        {
            SqlConnection con = ConexionBD.Abrir();
            SqlCommand cmd = new SqlCommand(
                SQLVentas + " WHERE v.Fecha >= @D AND v.Fecha <= @H ORDER BY v.Id, i.Id", con);
            cmd.Parameters.AddWithValue("@D", desde);
            cmd.Parameters.AddWithValue("@H", hasta);
            SqlDataReader r = cmd.ExecuteReader();
            List<Venta> lista = MapearConJoin(r);
            con.Close();
            return lista;
        }

        public Venta InsertarVenta(Venta v)
        {
            SqlConnection con = ConexionBD.Abrir();
            SqlCommand cmd = new SqlCommand(
                "INSERT INTO Ventas (Fecha, ClienteId, SucursalId, UsuarioId, Descuento, Estado, NumeroComprobante) " +
                "VALUES (@F, @CId, @SId, @UId, @Des, @Est, @NC); SELECT SCOPE_IDENTITY();", con);
            cmd.Parameters.AddWithValue("@F",   v.Fecha);
            cmd.Parameters.AddWithValue("@CId", v.ClienteId);
            cmd.Parameters.AddWithValue("@SId", (object)v.SucursalId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@UId", v.UsuarioId);
            cmd.Parameters.AddWithValue("@Des", v.Descuento);
            cmd.Parameters.AddWithValue("@Est", (int)v.Estado);
            cmd.Parameters.AddWithValue("@NC",  v.NumeroComprobante ?? "");
            v.Id = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            return v;
        }

        public ItemVenta InsertarItem(int ventaId, ItemVenta item)
        {
            string tipo = item is ExtintorItem ? "Extintor" : "Recarga";
            string nroSerie = null, tipoAgente = null;
            double? capacidad = null;
            string nroSerieExtintor = null;
            int? insumoId = null;

            if (item is ExtintorItem)
            {
                ExtintorItem ei = (ExtintorItem)item;
                nroSerie   = ei.NroSerie;
                tipoAgente = ei.TipoAgente;
                capacidad  = ei.Capacidad;
            }
            else if (item is RecargaItem)
            {
                RecargaItem ri = (RecargaItem)item;
                nroSerieExtintor = ri.NroSerieExtintor;
                insumoId         = ri.InsumoId;
            }

            SqlConnection con = ConexionBD.Abrir();
            SqlCommand cmd = new SqlCommand(
                "INSERT INTO ItemsVenta (VentaId, TipoItem, Cantidad, PrecioUnitario, NroSerie, TipoAgente, Capacidad, NroSerieExtintor, InsumoId) " +
                "VALUES (@VId, @Tipo, @Cant, @Precio, @NS, @TA, @Cap, @NSE, @IId); SELECT SCOPE_IDENTITY();", con);
            cmd.Parameters.AddWithValue("@VId",   ventaId);
            cmd.Parameters.AddWithValue("@Tipo",  tipo);
            cmd.Parameters.AddWithValue("@Cant",  item.Cantidad);
            cmd.Parameters.AddWithValue("@Precio", item.PrecioUnitario);
            cmd.Parameters.AddWithValue("@NS",    (object)nroSerie ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@TA",    (object)tipoAgente ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Cap",   (object)capacidad ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@NSE",   (object)nroSerieExtintor ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IId",   (object)insumoId ?? DBNull.Value);
            item.Id = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            return item;
        }

        public bool EliminarItem(int itemId)
        {
            SqlConnection con = ConexionBD.Abrir();
            SqlCommand cmd = new SqlCommand("DELETE FROM ItemsVenta WHERE Id = @Id", con);
            cmd.Parameters.AddWithValue("@Id", itemId);
            int filas = cmd.ExecuteNonQuery();
            con.Close();
            return filas > 0;
        }

        public bool ActualizarDescuento(int ventaId, double descuento)
        {
            SqlConnection con = ConexionBD.Abrir();
            SqlCommand cmd = new SqlCommand(
                "UPDATE Ventas SET Descuento = @D WHERE Id = @Id", con);
            cmd.Parameters.AddWithValue("@D",  descuento);
            cmd.Parameters.AddWithValue("@Id", ventaId);
            int filas = cmd.ExecuteNonQuery();
            con.Close();
            return filas > 0;
        }

        public bool ActualizarCliente(int ventaId, int clienteId)
        {
            SqlConnection con = ConexionBD.Abrir();
            SqlCommand cmd = new SqlCommand(
                "UPDATE Ventas SET ClienteId = @C WHERE Id = @Id", con);
            cmd.Parameters.AddWithValue("@C",  clienteId);
            cmd.Parameters.AddWithValue("@Id", ventaId);
            int filas = cmd.ExecuteNonQuery();
            con.Close();
            return filas > 0;
        }

        public bool ActualizarEstado(int ventaId, EstadoVenta estado, string numeroComprobante)
        {
            SqlConnection con = ConexionBD.Abrir();
            SqlCommand cmd = new SqlCommand(
                "UPDATE Ventas SET Estado = @E, NumeroComprobante = @NC WHERE Id = @Id", con);
            cmd.Parameters.AddWithValue("@E",  (int)estado);
            cmd.Parameters.AddWithValue("@NC", numeroComprobante ?? "");
            cmd.Parameters.AddWithValue("@Id", ventaId);
            int filas = cmd.ExecuteNonQuery();
            con.Close();
            return filas > 0;
        }
    }
}
