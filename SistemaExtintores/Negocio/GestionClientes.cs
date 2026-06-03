using SistemaExtintores.Modelos;

namespace SistemaExtintores.Negocio
{
    public class GestionClientes
    {
        //lista estátida: cada vez que se cree un nuevo GestionClientes, se comparte la misma lista
        private static List<Cliente> _clientes = new List<Cliente>
        {
            new ClientePersona(1, "Juan Pérez", "1234567", "76543210", "juan@gmail.com", "Av. Cañoto 123"),
            new ClientePersona(2, "María López", "7654321", "77891234", "maria@gmail.com", "Calle Murillo 456"),
            new ClienteEmpresa(3, "Seguridad Total SRL", "987654321", "Carlos Rojas", "33445566", "contacto@seguridadtotal.com", "Av. Busch 789")
        };
        private static int _contadorId = 4;

        public List<Cliente> ObtenerTodos() { return _clientes; }

        public Cliente ObtenerPorId(int id)
        {
            foreach (var cliente in _clientes)
            {
                if (cliente.GetId() == id)
                    return cliente;
            }
            return null;
        }

        public Cliente Agregar(Cliente cliente)
        {
            cliente.SetId(_contadorId++);
            _clientes.Add(cliente);
            return cliente;
        }

        public Cliente Actualizar(int id, Cliente clienteActualizado)
        {
            var existente = ObtenerPorId(id);
            if (existente == null) return null;

            existente.SetTelefono(clienteActualizado.GetTelefono());
            existente.SetCorreo(clienteActualizado.GetCorreo());
            existente.SetDireccion(clienteActualizado.GetDireccion());

            return existente;
        }

        public bool Eliminar(int id)
        {
            var cliente = ObtenerPorId(id);
            if (cliente == null) return false;

            cliente.Desactivar();
            return true;
        }
    }
}