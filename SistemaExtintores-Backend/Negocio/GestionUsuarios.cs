using SistemaExtintores.Modelos;

namespace SistemaExtintores.Negocio;


public class GestionUsuarios
{
    private static List<Usuario> _usuarios = new List<Usuario>
    {
        new Usuario(1, "admin", "Abc123", RolUsuario.Administrador, "Administrador Principal", null),
        new Usuario(2, "Cajera1", "Abc123", RolUsuario.Cajera, "Ana Flores", null),
    };
    private static int _contadorId = 3;

    private GestionClientes _gestionClientes;

    public GestionUsuarios(GestionClientes gestionClientes)
    {
        _gestionClientes = gestionClientes;
    }

    public List<Usuario> ObtenerTodos()
    {
        return _usuarios;
    }

    public Usuario ObtenerPorId(int id)
    {
        foreach (Usuario usuario in _usuarios)
        {
            if (usuario.Id == id)
            {
                return usuario;
            }
        }
        return null;
    }

    public Usuario ObtenerPorNombreUsuario(string nombreUsuario)
    {
        foreach (Usuario usuario in _usuarios)
        {
            if (usuario.NombreUsuario == nombreUsuario)
            {
                return usuario;
            }
        }
        return null;
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

        usuario.Id = _contadorId++;
        _usuarios.Add(usuario);
        return usuario;
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
        // HU-03: el administrador no puede desactivarse a sí mismo
        if (id == usuarioActualId) return false;

        Usuario usuario = ObtenerPorId(id);
        if (usuario == null) return false;

        usuario.Desactivar();
        return true;
    }
}
