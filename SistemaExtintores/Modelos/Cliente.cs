namespace SistemaExtintores.Modelos
{
    public abstract class Cliente
    {
        private int _id;
        private string _telefono;
        private string _correo;
        protected string _direccion;
        private bool _activo = true;

        public int GetId() { return _id; }
        public void SetId(int id) { _id = id; }

        public string GetTelefono() { return _telefono; }
        public void SetTelefono(string telefono) { _telefono = telefono; }

        public string GetCorreo() { return _correo; }
        public void SetCorreo(string correo) { _correo = correo; }

        public string GetDireccion() { return _direccion; }
        public void SetDireccion(string direccion) { _direccion = direccion; }

        public bool GetActivo() { return _activo; }
        public void Activar() { _activo = true; }
        public void Desactivar() { _activo = false; }

        public abstract string ObtenerNombreCompleto();
        public abstract string ObtenerDocumento();
    }
}