using SistemaExtintores.Data;
using SistemaExtintores.Modelos;

namespace SistemaExtintores.Negocio
{
    public class GestionUsuarios
    {
        private UsuariosData _data;
        private GestionClientes _gestionClientes;

        public GestionUsuarios()
        {
            _data = new UsuariosData();
            _gestionClientes = new GestionClientes();
        }

        public List<Usuario> ObtenerTodos()
        {
            return _data.ObtenerTodos();
        }

        public Usuario ObtenerPorId(int id)
        {
            return _data.ObtenerPorId(id);
        }

        public Usuario ObtenerPorNombreUsuario(string nombreUsuario)
        {
            return _data.ObtenerPorNombreUsuario(nombreUsuario);
        }

        public Usuario ValidarCredenciales(string nombreUsuario, string password)
        {
            Usuario usuario = ObtenerPorNombreUsuario(nombreUsuario);
            if (usuario == null) return null;
            if (!usuario.Activo) return null;
            if (usuario.Password != password) return null;
            return usuario;
        }

        public Usuario Agregar(Usuario usuario)
        {
            if (usuario.ClienteId != null)
            {
                Cliente cliente = _gestionClientes.ObtenerPorId(usuario.ClienteId.Value);
                if (cliente == null) return null;
            }
            return _data.Insertar(usuario);
        }

        public Usuario Actualizar(int id, Usuario usuarioActualizado)
        {
            Usuario existente = ObtenerPorId(id);
            if (existente == null) return null;
            existente.NombreUsuario = usuarioActualizado.NombreUsuario;
            return existente;
        }

        public bool Eliminar(int id, int usuarioActualId)
        {
            if (id == usuarioActualId) return false;
            Usuario usuario = ObtenerPorId(id);
            if (usuario == null) return false;
            return _data.Desactivar(id);
        }
    }
}
