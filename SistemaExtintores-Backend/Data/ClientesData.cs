using Microsoft.Data.SqlClient;
using SistemaExtintores.Modelos;

namespace SistemaExtintores.Data
{
    public class ClientesData
    {
        private static string SQLClientes = "SELECT Id, TipoCliente, Telefono, Correo, Direccion, Activo, " +
            "NombreCompleto, CI, RazonSocial, NIT, NombreContacto FROM Clientes";

        private static Cliente MapearCliente(SqlDataReader r)
        {
            string tipo = r["TipoCliente"].ToString();
            Cliente c;

            if (tipo == "Persona")
            {
                ClientePersona p = new ClientePersona();
                p.NombreCompleto = r["NombreCompleto"] == DBNull.Value ? "" : r["NombreCompleto"].ToString();
                p.Ci             = r["CI"] == DBNull.Value ? "" : r["CI"].ToString();
                c = p;
            }
            else
            {
                ClienteEmpresa e = new ClienteEmpresa();
                e.RazonSocial   = r["RazonSocial"] == DBNull.Value ? "" : r["RazonSocial"].ToString();
                e.Nit           = r["NIT"] == DBNull.Value ? "" : r["NIT"].ToString();
                e.NombreContacto = r["NombreContacto"] == DBNull.Value ? "" : r["NombreContacto"].ToString();
                e.Sucursales    = new List<Sucursal>();
                c = e;
            }

            c.Id        = (int)r["Id"];
            c.Telefono  = r["Telefono"] == DBNull.Value ? "" : r["Telefono"].ToString();
            c.Correo    = r["Correo"] == DBNull.Value ? "" : r["Correo"].ToString();
            c.Direccion = r["Direccion"] == DBNull.Value ? "" : r["Direccion"].ToString();
            c.Activo    = (bool)r["Activo"];
            return c;
        }

        private static Sucursal MapearSucursal(SqlDataReader r)
        {
            Sucursal s = new Sucursal();
            s.Id               = (int)r["Id"];
            s.Nombre           = r["Nombre"].ToString();
            s.Direccion        = r["Direccion"] == DBNull.Value ? "" : r["Direccion"].ToString();
            s.Telefono         = r["Telefono"] == DBNull.Value ? "" : r["Telefono"].ToString();
            s.Activa           = (bool)r["Activa"];
            s.ClienteEmpresaId = (int)r["ClienteEmpresaId"];
            return s;
        }

        private void CargarSucursales(ClienteEmpresa empresa)
        {
            SqlConnection con = ConexionBD.Abrir();
            SqlCommand cmd = new SqlCommand(
                "SELECT * FROM Sucursales WHERE ClienteEmpresaId = @Id", con);
            cmd.Parameters.AddWithValue("@Id", empresa.Id);
            SqlDataReader r = cmd.ExecuteReader();
            while (r.Read()) empresa.Sucursales.Add(MapearSucursal(r));
            con.Close();
        }

        public List<Cliente> ObtenerTodos()
        {
            List<Cliente> lista = new List<Cliente>();
            SqlConnection con = ConexionBD.Abrir();
            SqlCommand cmd = new SqlCommand(SQLClientes, con);
            SqlDataReader r = cmd.ExecuteReader();
            while (r.Read()) lista.Add(MapearCliente(r));
            con.Close();

            foreach (Cliente c in lista)
            {
                if (c is ClienteEmpresa) CargarSucursales((ClienteEmpresa)c);
            }
            return lista;
        }

        public Cliente ObtenerPorId(int id)
        {
            SqlConnection con = ConexionBD.Abrir();
            SqlCommand cmd = new SqlCommand(SQLClientes + " WHERE Id = @Id", con);
            cmd.Parameters.AddWithValue("@Id", id);
            SqlDataReader r = cmd.ExecuteReader();
            Cliente c = null;
            if (r.Read()) c = MapearCliente(r);
            con.Close();

            if (c is ClienteEmpresa) CargarSucursales((ClienteEmpresa)c);
            return c;
        }

        public Cliente InsertarPersona(ClientePersona p)
        {
            SqlConnection con = ConexionBD.Abrir();
            SqlCommand cmd = new SqlCommand(
                "INSERT INTO Clientes (TipoCliente, Telefono, Correo, Direccion, Activo, NombreCompleto, CI) " +
                "VALUES ('Persona', @Tel, @Cor, @Dir, 1, @NC, @CI); SELECT SCOPE_IDENTITY();", con);
            cmd.Parameters.AddWithValue("@Tel", p.Telefono ?? "");
            cmd.Parameters.AddWithValue("@Cor", p.Correo ?? "");
            cmd.Parameters.AddWithValue("@Dir", p.Direccion ?? "");
            cmd.Parameters.AddWithValue("@NC",  p.NombreCompleto ?? "");
            cmd.Parameters.AddWithValue("@CI",  p.Ci ?? "");
            p.Id = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            return p;
        }

        public Cliente InsertarEmpresa(ClienteEmpresa e)
        {
            SqlConnection con = ConexionBD.Abrir();
            SqlCommand cmd = new SqlCommand(
                "INSERT INTO Clientes (TipoCliente, Telefono, Correo, Direccion, Activo, RazonSocial, NIT, NombreContacto) " +
                "VALUES ('Empresa', @Tel, @Cor, @Dir, 1, @RS, @NIT, @NC); SELECT SCOPE_IDENTITY();", con);
            cmd.Parameters.AddWithValue("@Tel", e.Telefono ?? "");
            cmd.Parameters.AddWithValue("@Cor", e.Correo ?? "");
            cmd.Parameters.AddWithValue("@Dir", e.Direccion ?? "");
            cmd.Parameters.AddWithValue("@RS",  e.RazonSocial ?? "");
            cmd.Parameters.AddWithValue("@NIT", e.Nit ?? "");
            cmd.Parameters.AddWithValue("@NC",  e.NombreContacto ?? "");
            e.Id = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            return e;
        }

        public bool ActualizarContacto(int id, string telefono, string correo, string direccion)
        {
            SqlConnection con = ConexionBD.Abrir();
            SqlCommand cmd = new SqlCommand(
                "UPDATE Clientes SET Telefono=@T, Correo=@C, Direccion=@D WHERE Id=@Id", con);
            cmd.Parameters.AddWithValue("@T",  telefono ?? "");
            cmd.Parameters.AddWithValue("@C",  correo ?? "");
            cmd.Parameters.AddWithValue("@D",  direccion ?? "");
            cmd.Parameters.AddWithValue("@Id", id);
            int filas = cmd.ExecuteNonQuery();
            con.Close();
            return filas > 0;
        }

        public bool Desactivar(int id)
        {
            SqlConnection con = ConexionBD.Abrir();
            SqlCommand cmd = new SqlCommand("UPDATE Clientes SET Activo = 0 WHERE Id = @Id", con);
            cmd.Parameters.AddWithValue("@Id", id);
            int filas = cmd.ExecuteNonQuery();
            con.Close();
            return filas > 0;
        }

        // SUCURSALES
        public Sucursal InsertarSucursal(Sucursal s)
        {
            SqlConnection con = ConexionBD.Abrir();
            SqlCommand cmd = new SqlCommand(
                "INSERT INTO Sucursales (ClienteEmpresaId, Nombre, Direccion, Telefono, Activa) " +
                "VALUES (@EId, @N, @D, @T, 1); SELECT SCOPE_IDENTITY();", con);
            cmd.Parameters.AddWithValue("@EId", s.ClienteEmpresaId);
            cmd.Parameters.AddWithValue("@N",   s.Nombre ?? "");
            cmd.Parameters.AddWithValue("@D",   s.Direccion ?? "");
            cmd.Parameters.AddWithValue("@T",   s.Telefono ?? "");
            s.Id = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            return s;
        }

        public Sucursal ObtenerSucursalPorId(int sucursalId)
        {
            SqlConnection con = ConexionBD.Abrir();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Sucursales WHERE Id = @Id", con);
            cmd.Parameters.AddWithValue("@Id", sucursalId);
            SqlDataReader r = cmd.ExecuteReader();
            Sucursal s = null;
            if (r.Read()) s = MapearSucursal(r);
            con.Close();
            return s;
        }

        public bool ActualizarSucursal(int id, string nombre, string direccion, string telefono)
        {
            SqlConnection con = ConexionBD.Abrir();
            SqlCommand cmd = new SqlCommand(
                "UPDATE Sucursales SET Nombre=@N, Direccion=@D, Telefono=@T WHERE Id=@Id", con);
            cmd.Parameters.AddWithValue("@N",  nombre ?? "");
            cmd.Parameters.AddWithValue("@D",  direccion ?? "");
            cmd.Parameters.AddWithValue("@T",  telefono ?? "");
            cmd.Parameters.AddWithValue("@Id", id);
            int filas = cmd.ExecuteNonQuery();
            con.Close();
            return filas > 0;
        }

        public bool DesactivarSucursal(int id)
        {
            SqlConnection con = ConexionBD.Abrir();
            SqlCommand cmd = new SqlCommand("UPDATE Sucursales SET Activa = 0 WHERE Id = @Id", con);
            cmd.Parameters.AddWithValue("@Id", id);
            int filas = cmd.ExecuteNonQuery();
            con.Close();
            return filas > 0;
        }

        public int ContarSucursalesActivas(int clienteEmpresaId)
        {
            SqlConnection con = ConexionBD.Abrir();
            SqlCommand cmd = new SqlCommand(
                "SELECT COUNT(*) FROM Sucursales WHERE ClienteEmpresaId=@Id AND Activa=1", con);
            cmd.Parameters.AddWithValue("@Id", clienteEmpresaId);
            int count = (int)cmd.ExecuteScalar();
            con.Close();
            return count;
        }
    }
}
