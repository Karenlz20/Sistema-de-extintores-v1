using Microsoft.AspNetCore.Mvc;
using SistemaExtintores.Modelos;
using SistemaExtintores.Negocio;

namespace SistemaExtintores.Controllers;

[ApiController]
[Route("[controller]")]
public class ClientesController : ControllerBase
{
    private GestionClientes _gestionClientes;

    public ClientesController()
    {
        _gestionClientes = new GestionClientes();
    }

    [HttpGet("lista-clientes")]
    public IEnumerable<object> ListaClientes()
    {
        return _gestionClientes.ObtenerTodos().Cast<object>();
    }

    [HttpGet("obtener-cliente/{id}")]
    public Cliente ObtenerPorId(int id)
    {
        return _gestionClientes.ObtenerPorId(id);
    }

    [HttpPost("agregar-persona")]
    public Cliente AgregarPersona([FromBody] ClientePersona cliente)
    {
        return _gestionClientes.Agregar(cliente);
    }

    [HttpPost("agregar-empresa")]
    public Cliente AgregarEmpresa([FromBody] ClienteEmpresa cliente)
    {
        return _gestionClientes.Agregar(cliente);
    }

    [HttpPut("actualizar-cliente/{id}")]
    public Cliente Actualizar(int id, [FromBody] Cliente cliente)
    {
        return _gestionClientes.Actualizar(id, cliente);
    }

    [HttpDelete("eliminar-cliente/{id}")]
    public bool Eliminar(int id)
    {
        return _gestionClientes.Eliminar(id);
    }
}