using Microsoft.AspNetCore.Mvc;
using SistemaExtintores.Modelos;
using SistemaExtintores.Negocio;

namespace SistemaExtintores.Controllers;

[ApiController]
[Route("[controller]")]
public class UsuariosController : ControllerBase
{
    private GestionUsuarios _gestionUsuarios;
    
    public UsuariosController()
    {
        var _gestionClientes = new GestionClientes();
        _gestionUsuarios = new GestionUsuarios(_gestionClientes);
    }

    [HttpGet("lista-usuarios")]
    public IEnumerable<object> ListaUsuarios()
    {
        return _gestionUsuarios.ObtenerTodos().Cast<object>();
    }

    [HttpGet("obtener-usuario/{id}")]
    public Usuario ObtenerPorId(int id)
    {
        return _gestionUsuarios.ObtenerPorId(id);
    }
    
    [HttpPost("agregar-usuario")]
    public Usuario AgregarUsuario([FromBody] Usuario usuario)
    {
        return _gestionUsuarios.Agregar(usuario);
    }
    
    [HttpPut("actualizar-usuario/{id}")]
    public Usuario Actualizar(int id, [FromBody] Usuario usuario)
    {
        return _gestionUsuarios.Actualizar(id, usuario);
    }

    [HttpDelete("eliminar-usuario/{id}")]
    public bool Eliminar(int id)
    {
        return _gestionUsuarios.Eliminar(id);
    }
}