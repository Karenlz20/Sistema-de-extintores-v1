namespace SistemaExtintores.Modelos
{
    public class ClientePersona : Cliente
    {
        private string _nombreCompleto;
        private string _ci;
        
        //constructor vacío
        public ClientePersona() { }
    
        //constructor con setters para futuras validaciones
        public ClientePersona(int id, string nombreCompleto, string ci, string telefono, string correo, string direccion)
        {
            SetId(id);
            SetNombreCompleto(nombreCompleto);
            SetCI(ci);
            SetTelefono(telefono);
            SetCorreo(correo);
            SetDireccion(direccion);
        }

        public string GetNombreCompleto() { return _nombreCompleto; }
        public void SetNombreCompleto(string nombre) { _nombreCompleto = nombre; }

        public string GetCI() { return _ci; }
        public void SetCI(string ci) { _ci = ci; }

        public override string ObtenerNombreCompleto() { return _nombreCompleto; }
        public override string ObtenerDocumento() { return "CI: " + _ci; }
    }
}