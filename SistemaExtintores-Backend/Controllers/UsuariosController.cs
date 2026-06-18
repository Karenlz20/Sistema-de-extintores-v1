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
        GestionInventario gestionInventario = new GestionInventario();
        GestionExtintores gestionExtintores = new GestionExtintores(gestionInventario);
        GestionClientes gestionClientes = new GestionClientes(gestionExtintores);
        _gestionUsuarios = new GestionUsuarios(gestionClientes);
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
    public object Eliminar(int id, [FromQuery] int usuarioActualId)
    {
        if (id == usuarioActualId)
            return BadRequest("No podés desactivarte a vos mismo.");

        Usuario usuario = _gestionUsuarios.ObtenerPorId(id);
        if (usuario == null)
            return BadRequest("Usuario no encontrado.");

        bool ok = _gestionUsuarios.Eliminar(id, usuarioActualId);
        if (!ok)
            return BadRequest("No se pudo desactivar el usuario.");

        return Ok(new { Mensaje = "Usuario desactivado correctamente." });
    }
}