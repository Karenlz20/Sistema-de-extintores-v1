using Microsoft.Data.SqlClient;
using SistemaExtintores.Modelos;

namespace SistemaExtintores.Data
{
    public class UsuariosData
    {
        private static Usuario Mapear(SqlDataReader r)
        {
            Usuario u = new Usuario();
            u.Id           = (int)r["Id"];
            u.NombreUsuario  = r["NombreUsuario"].ToString();
            u.Password       = r["Password"].ToString();
            u.NombreCompleto = r["NombreCompleto"].ToString();
            u.Rol            = (RolUsuario)(int)r["Rol"];
            u.Activo         = (bool)r["Activo"];
            return u;
        }

        public List<Usuario> ObtenerTodos()
        {
            List<Usuario> lista = new List<Usuario>();
            SqlConnection con = ConexionBD.Abrir();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Usuarios", con);
            SqlDataReader r = cmd.ExecuteReader();
            while (r.Read()) lista.Add(Mapear(r));
            con.Close();
            return lista;
        }

        public Usuario ObtenerPorId(int id)
        {
            SqlConnection con = ConexionBD.Abrir();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Usuarios WHERE Id = @Id", con);
            cmd.Parameters.AddWithValue("@Id", id);
            SqlDataReader r = cmd.ExecuteReader();
            Usuario u = null;
            if (r.Read()) u = Mapear(r);
            con.Close();
            return u;
        }

        public Usuario ObtenerPorNombreUsuario(string nombreUsuario)
        {
            SqlConnection con = ConexionBD.Abrir();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Usuarios WHERE NombreUsuario = @N", con);
            cmd.Parameters.AddWithValue("@N", nombreUsuario);
            SqlDataReader r = cmd.ExecuteReader();
            Usuario u = null;
            if (r.Read()) u = Mapear(r);
            con.Close();
            return u;
        }

        public Usuario Insertar(Usuario u)
        {
            SqlConnection con = ConexionBD.Abrir();
            SqlCommand cmd = new SqlCommand(
                "INSERT INTO Usuarios (NombreUsuario, Password, NombreCompleto, Rol, Activo) " +
                "VALUES (@N, @P, @NC, @R, @A); SELECT SCOPE_IDENTITY();", con);
            cmd.Parameters.AddWithValue("@N",  u.NombreUsuario);
            cmd.Parameters.AddWithValue("@P",  u.Password);
            cmd.Parameters.AddWithValue("@NC", u.NombreCompleto);
            cmd.Parameters.AddWithValue("@R",  (int)u.Rol);
            cmd.Parameters.AddWithValue("@A",  u.Activo);
            u.Id = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            return u;
        }

        public bool Desactivar(int id)
        {
            SqlConnection con = ConexionBD.Abrir();
            SqlCommand cmd = new SqlCommand("UPDATE Usuarios SET Activo = 0 WHERE Id = @Id", con);
            cmd.Parameters.AddWithValue("@Id", id);
            int filas = cmd.ExecuteNonQuery();
            con.Close();
            return filas > 0;
        }
    }
}
