namespace SistemaExtintores.Modelos
{
    public class Sucursal
    {
        private int _id;
        private string _nombre;
        private string _direccion;
        private string _telefono;
        private bool _activa = true;
        private int _clienteEmpresaId;

        public int GetId() { return _id; }
        public void SetId(int id) { _id = id; }

        public string GetNombre() { return _nombre; }
        public void SetNombre(string nombre) { _nombre = nombre; }

        public string GetDireccion() { return _direccion; }
        public void SetDireccion(string direccion) { _direccion = direccion; }

        public string GetTelefono() { return _telefono; }
        public void SetTelefono(string telefono) { _telefono = telefono; }

        public bool GetActiva() { return _activa; }
        public void Activar() { _activa = true; }
        public void Desactivar() { _activa = false; }

        public int GetClienteEmpresaId() { return _clienteEmpresaId; }
        public void SetClienteEmpresaId(int id) { _clienteEmpresaId = id; }
    }
}