using Microsoft.AspNetCore.Mvc;
using SistemaExtintores.Modelos;
using SistemaExtintores.Negocio;

namespace SistemaExtintores.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private GestionUsuarios _gestionUsuarios;

    public AuthController()
    {
        GestionInventario gestionInventario = new GestionInventario();
        GestionExtintores gestionExtintores = new GestionExtintores(gestionInventario);
        GestionClientes gestionClientes = new GestionClientes(gestionExtintores);
        _gestionUsuarios = new GestionUsuarios(gestionClientes);
    }

    [HttpPost("login")]
    public object Login([FromBody] LoginDto datos)
    {
        Usuario usuario = _gestionUsuarios.ValidarCredenciales(datos.NombreUsuario, datos.Password);
        if (usuario == null)
            return BadRequest("Usuario o contraseña incorrectos.");

        return Ok(new
        {
            Id = usuario.Id,
            NombreUsuario = usuario.NombreUsuario,
            NombreCompleto = usuario.NombreCompleto,
            Rol = usuario.Rol.ToString()
        });
    }

    [HttpPost("logout")]
    public object Logout()
    {
        return Ok(new { Mensaje = "Sesión cerrada correctamente." });
    }
}

public class LoginDto
{
    public string NombreUsuario { get; set; }
    public string Password { get; set; }
}
