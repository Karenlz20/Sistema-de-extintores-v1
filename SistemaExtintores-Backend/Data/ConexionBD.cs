using Microsoft.Data.SqlClient;

namespace SistemaExtintores.Data
{
    public class ConexionBD
    {
        private static readonly string _cadena =
            "Server=localhost;Database=SistemaExtintores;Trusted_Connection=True;TrustServerCertificate=True;";

        public static SqlConnection Abrir()
        {
            SqlConnection con = new SqlConnection(_cadena);
            con.Open();
            return con;
        }
    }
}
