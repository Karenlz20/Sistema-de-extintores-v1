namespace SistemaExtintores.Modelos
{
    public class ClienteEmpresa : Cliente
    {
        public string RazonSocial { get; set; }
        public string Nit { get; set; }
        public string NombreContacto { get; set; }
        public List<Sucursal> Sucursales { get; set; } = new List<Sucursal>();

        public ClienteEmpresa() { }

        public ClienteEmpresa(int id, string razonSocial, string nit, string nombreContacto, string telefono, string correo, string direccion)
        {
            Id = id;
            RazonSocial = razonSocial;
            Nit = nit;
            NombreContacto = nombreContacto;
            Telefono = telefono;
            Correo = correo;
            Direccion = direccion;
        }

        public void AgregarSucursal(Sucursal sucursal) { Sucursales.Add(sucursal); }

        public override string ObtenerNombreCompleto() { return RazonSocial; }
        public override string ObtenerDocumento() { return "Nit: " + Nit; }
    }
}