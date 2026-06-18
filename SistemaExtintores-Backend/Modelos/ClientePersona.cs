namespace SistemaExtintores.Modelos
{
    public class ClientePersona : Cliente
    {
        public string NombreCompleto { get; set; }
        public string Ci { get; set; }

        public ClientePersona() { }

        public ClientePersona(int id, string nombreCompleto, string ci, string telefono, string correo, string direccion)
        {
            Id = id;
            NombreCompleto = nombreCompleto;
            Ci = ci;
            Telefono = telefono;
            Correo = correo;
            Direccion = direccion;
        }

        public override string ObtenerNombreCompleto() { return NombreCompleto; }
        public override string ObtenerDocumento() { return "Ci: " + Ci; }
    }
}