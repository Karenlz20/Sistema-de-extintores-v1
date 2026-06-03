using SistemaExtintores.Modelos;

namespace SistemaExtintores.Negocio;
 

public class GestionUsuarios
{
    private static List<Usuario> _usuarios = new List<Usuario>
    {
        new Usuario(1, "jPerez01", "Abc123", RolUsuario.Cliente, null, 1),
        new Usuario(2, "MLopez20", "Abc123", RolUsuario.Cliente, null, 2),
        new Usuario(3, "Seguridad.SRL", "Abc123", RolUsuario.Cliente, null, 3),
        new Usuario(4, "Cajera1",  "Abc123", RolUsuario.Cajero, "Ana Flores", null),
        new Usuario(5, "Tecnico1",  "Abc123", RolUsuario.Tecnico, "Carlos Mendez", null),
    };
    private static int _contadorId = 6;

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