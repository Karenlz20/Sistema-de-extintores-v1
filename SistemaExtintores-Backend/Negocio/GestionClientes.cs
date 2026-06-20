using SistemaExtintores.Data;
using SistemaExtintores.Modelos;

namespace SistemaExtintores.Negocio
{
    public class GestionClientes
    {
        private ClientesData _data;
        private GestionExtintores _gestionExtintores;

        public GestionClientes()
        {
            _data = new ClientesData();
            _gestionExtintores = new GestionExtintores();
        }

        public List<Cliente> ObtenerTodos()
        {
            return _data.ObtenerTodos();
        }

        public Cliente ObtenerPorId(int id)
        {
            return _data.ObtenerPorId(id);
        }

        public Cliente Agregar(Cliente cliente)
        {
            if (cliente is ClientePersona) return _data.InsertarPersona((ClientePersona)cliente);
            if (cliente is ClienteEmpresa) return _data.InsertarEmpresa((ClienteEmpresa)cliente);
            return null;
        }

        public Cliente Actualizar(int id, Cliente clienteActualizado)
        {
            Cliente existente = ObtenerPorId(id);
            if (existente == null) return null;
            bool ok = _data.ActualizarContacto(id, clienteActualizado.Telefono, clienteActualizado.Correo, clienteActualizado.Direccion);
            if (!ok) return null;
            existente.Telefono  = clienteActualizado.Telefono;
            existente.Correo    = clienteActualizado.Correo;
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
                if (extintor.ObtenerEstado() != EstadoVigencia.FueraDeNorma) return false;
            }

            return _data.Desactivar(id);
        }

        // SUCURSALES
        public Sucursal AgregarSucursal(int empresaId, Sucursal sucursal)
        {
            Cliente cliente = ObtenerPorId(empresaId);
            if (cliente == null) return null;
            if (!(cliente is ClienteEmpresa)) return null;

            sucursal.ClienteEmpresaId = empresaId;
            sucursal.Activa = true;
            return _data.InsertarSucursal(sucursal);
        }

        public Sucursal ActualizarSucursal(int sucursalId, Sucursal sucursalActualizada)
        {
            Sucursal existente = _data.ObtenerSucursalPorId(sucursalId);
            if (existente == null) return null;
            bool ok = _data.ActualizarSucursal(sucursalId,
                sucursalActualizada.Nombre, sucursalActualizada.Direccion, sucursalActualizada.Telefono);
            if (!ok) return null;
            existente.Nombre    = sucursalActualizada.Nombre;
            existente.Direccion = sucursalActualizada.Direccion;
            existente.Telefono  = sucursalActualizada.Telefono;
            return existente;
        }

        public Sucursal ObtenerSucursalPorId(int sucursalId)
        {
            return _data.ObtenerSucursalPorId(sucursalId);
        }

        public bool BajaSucursal(int sucursalId)
        {
            Sucursal sucursal = _data.ObtenerSucursalPorId(sucursalId);
            if (sucursal == null) return false;

            int activas = _data.ContarSucursalesActivas(sucursal.ClienteEmpresaId);
            if (activas <= 1) return false;

            return _data.DesactivarSucursal(sucursalId);
        }
    }
}
