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
        foreach (var usuario in _usuarios)
        {
            if (usuario.GetId() == id)
            {
                return usuario;
            }
        }
        return null;
    }
    
    public Usuario Agregar(Usuario usuario)
    {
        //si el idCliente no es nulo, hay que verificar que ese id sea válido
        if (usuario.GetClienteId() != null)
        {
            var cliente = _gestionClientes.ObtenerPorId(usuario.GetClienteId().Value);
            //si no es un id válido, se termina
            if (cliente == null) return null;
        }
        
        usuario.SetId(_contadorId++);
        _usuarios.Add(usuario);
        return usuario;
    }

    public Usuario Actualizar(int id, Usuario usuarioActualizado)
    {
        var existente = ObtenerPorId(id);
        if (existente == null) return null;

        existente.SetNombreUsuario(usuarioActualizado.GetNombreUsuario());
        
        return existente;
    }

    public bool Eliminar(int id)
    {
        var usuario = ObtenerPorId(id);
        if (usuario == null) return false;

        usuario.Desactivar();
        return true;
    }
}