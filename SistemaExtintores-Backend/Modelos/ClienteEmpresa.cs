namespace SistemaExtintores.Modelos
{
    public class ClienteEmpresa : Cliente
    {
        public string RazonSocial { get; set; }
        public string NIT { get; set; }
        public string NombreContacto { get; set; }
        public List<Sucursal> Sucursales { get; set; } = new List<Sucursal>();

        public ClienteEmpresa() { }

        public ClienteEmpresa(int id, string razonSocial, string nit, string nombreContacto, string telefono, string correo, string direccion)
        {
            Id = id;
            RazonSocial = razonSocial;
            NIT = nit;
            NombreContacto = nombreContacto;
            Telefono = telefono;
            Correo = correo;
            Direccion = direccion;
        }

        public void AgregarSucursal(Sucursal sucursal) { Sucursales.Add(sucursal); }

        public override string ObtenerNombreCompleto() { return RazonSocial; }
        public override string ObtenerDocumento() { return "NIT: " + NIT; }
    }
}