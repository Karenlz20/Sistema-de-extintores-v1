using SistemaExtintores.Modelos;

namespace SistemaExtintores.Negocio
{
    public class GestionClientes
    {
        private static List<Cliente> _clientes = new List<Cliente>
        {
            new ClientePersona(1, "Juan Pérez", "1234567", "76543210", "juan@gmail.com", "Av. Cañoto 123"),
            new ClientePersona(2, "María López", "7654321", "77891234", "maria@gmail.com", "Calle Murillo 456"),
            new ClienteEmpresa(3, "Seguridad Total SRL", "987654321", "Carlos Rojas", "33445566", "contacto@seguridadtotal.com", "Av. Busch 789")
        };
        private static int _contadorId = 4;
        private static int _contadorSucursalId = 1;
        private GestionExtintores _gestionExtintores;

        public GestionClientes(GestionExtintores gestionExtintores)
        {
            _gestionExtintores = gestionExtintores;
        }
        public List<Cliente> ObtenerTodos() { return _clientes; }

        // ── Sucursales (HU-06) ──

        public Sucursal AgregarSucursal(int empresaId, Sucursal sucursal)
        {
            Cliente cliente = ObtenerPorId(empresaId);
            if (cliente == null) return null;
            if (!(cliente is ClienteEmpresa)) return null;

            ClienteEmpresa empresa = (ClienteEmpresa)cliente;
            sucursal.Id = _contadorSucursalId++;
            sucursal.ClienteEmpresaId = empresaId;
            sucursal.Activa = true;
            empresa.AgregarSucursal(sucursal);
            return sucursal;
        }

        public Sucursal ActualizarSucursal(int sucursalId, Sucursal sucursalActualizada)
        {
            foreach (Cliente cliente in _clientes)
            {
                if (cliente is ClienteEmpresa)
                {
                    ClienteEmpresa empresa = (ClienteEmpresa)cliente;
                    foreach (Sucursal s in empresa.Sucursales)
                    {
                        if (s.Id == sucursalId)
                        {
                            s.Nombre = sucursalActualizada.Nombre;
                            s.Direccion = sucursalActualizada.Direccion;
                            s.Telefono = sucursalActualizada.Telefono;
                            return s;
                        }
                    }
                }
            }
            return null;
        }

        public Sucursal ObtenerSucursalPorId(int sucursalId)
        {
            foreach (Cliente cliente in _clientes)
            {
                if (cliente is ClienteEmpresa)
                {
                    ClienteEmpresa empresa = (ClienteEmpresa)cliente;
                    foreach (Sucursal s in empresa.Sucursales)
                    {
                        if (s.Id == sucursalId) return s;
                    }
                }
            }
            return null;
        }

        public bool BajaSucursal(int sucursalId)
        {
            Sucursal sucursal = ObtenerSucursalPorId(sucursalId);
            if (sucursal == null) return false;

            // No bajar si es la única sucursal activa de la empresa
            ClienteEmpresa empresa = (ClienteEmpresa)ObtenerPorId(sucursal.ClienteEmpresaId);
            int activas = 0;
            foreach (Sucursal s in empresa.Sucursales)
            {
                if (s.Activa) activas++;
            }
            if (activas <= 1) return false;

            sucursal.Desactivar();
            return true;
        }

        public Cliente ObtenerPorId(int id)
        {
            foreach (Cliente cliente in _clientes)
            {
                if (cliente.Id == id)
                    return cliente;
            }
            return null;
        }

        public Cliente Agregar(Cliente cliente)
        {
            cliente.Id = _contadorId++;
            _clientes.Add(cliente);
            return cliente;
        }

        public Cliente Actualizar(int id, Cliente clienteActualizado)
        {
            Cliente existente = ObtenerPorId(id);
            if (existente == null) return null;

            existente.Telefono = clienteActualizado.Telefono;
            existente.Correo = clienteActualizado.Correo;
            existente.Direccion = clienteActualizado.Direccion;

            return existente;
        }

        public bool Eliminar(int id)
        {
            Cliente cliente = ObtenerPorId(id);
            if (cliente == null) return false;

            List<Extintor> extintores = _gestionExtintores.ObtenerPorCliente(id);
            foreach (Extintor extintor in extintores)
            {
                if (extintor.ObtenerEstado() != EstadoVigencia.FueraDeNorma)
                    return false;
            }

            cliente.Desactivar();
            return true;
        }
    }
}
