namespace SistemaExtintores.Modelos
{
    public class ClienteEmpresa : Cliente
    {
        private string _razonSocial;
        private string _nit;
        private string _nombreContacto;
        private List<Sucursal> _sucursales = new List<Sucursal>();
        
        //constructor vacío
        public ClienteEmpresa() { }
        
        //constructor con setters para futuras validaciones
        public ClienteEmpresa(int id, string razonSocial, string nit, string nombreContacto, string telefono, string correo, string direccion)
        {
            SetId(id);
            SetRazonSocial(razonSocial);
            SetNIT(nit);
            SetNombreContacto(nombreContacto);
            SetTelefono(telefono);
            SetCorreo(correo);
            SetDireccion(direccion);
        }

        public string GetRazonSocial() { return _razonSocial; }
        public void SetRazonSocial(string razonSocial) { _razonSocial = razonSocial; }

        public string GetNIT() { return _nit; }
        public void SetNIT(string nit) { _nit = nit; }

        public string GetNombreContacto() { return _nombreContacto; }
        public void SetNombreContacto(string nombreContacto) { _nombreContacto = nombreContacto; }

        public string GetDireccionEmpresa() { return _direccion; }
        public void SetDireccionEmpresa(string direccion) { _direccion = direccion; }

        public List<Sucursal> GetSucursales() { return _sucursales; }
        public void AgregarSucursal(Sucursal sucursal) { _sucursales.Add(sucursal); }

        public override string ObtenerNombreCompleto() { return _razonSocial; }
        public override string ObtenerDocumento() { return "NIT: " + _nit; }
    }
}