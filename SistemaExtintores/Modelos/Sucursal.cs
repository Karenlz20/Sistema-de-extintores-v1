namespace SistemaExtintores.Modelos
{
    public class Sucursal
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public bool Activa { get; set; } = true;
        public int ClienteEmpresaId { get; set; }

        public Sucursal() { }

        public Sucursal(int id, string nombre, string direccion, string telefono, int clienteEmpresaId)
        {
            Id = id;
            Nombre = nombre;
            Direccion = direccion;
            Telefono = telefono;
            ClienteEmpresaId = clienteEmpresaId;
        }

        public void Activar() { Activa = true; }
        public void Desactivar() { Activa = false; }
    }
}
