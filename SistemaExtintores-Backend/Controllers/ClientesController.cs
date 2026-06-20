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
    public object Actualizar(int id, [FromBody] ActualizarClienteDto datos)
    {
        Cliente existente = _gestionClientes.ObtenerPorId(id);
        if (existente == null) return BadRequest("Cliente no encontrado.");
        existente.Telefono  = datos.Telefono;
        existente.Correo    = datos.Correo;
        existente.Direccion = datos.Direccion;
        Cliente actualizado = _gestionClientes.Actualizar(id, existente);
        if (actualizado == null) return BadRequest("No se pudo actualizar el cliente.");
        return Ok(actualizado);
    }

    [HttpDelete("eliminar-cliente/{id}")]
    public object Eliminar(int id)
    {
        bool ok = _gestionClientes.Eliminar(id);
        if (!ok) return BadRequest("No se puede dar de baja. El cliente tiene extintores activos.");
        return Ok(new { Mensaje = "Cliente dado de baja correctamente." });
    }

    // ── Sucursales (HU-06) ──

    [HttpPost("agregar-sucursal/{empresaId}")]
    public object AgregarSucursal(int empresaId, [FromBody] Sucursal sucursal)
    {
        Cliente cliente = _gestionClientes.ObtenerPorId(empresaId);
        if (cliente == null)
            return BadRequest("Cliente no encontrado.");
        if (!(cliente is ClienteEmpresa))
            return BadRequest("El cliente no es una empresa, no admite sucursales.");

        Sucursal nueva = _gestionClientes.AgregarSucursal(empresaId, sucursal);
        return Ok(nueva);
    }

    [HttpPut("actualizar-sucursal/{sucursalId}")]
    public object ActualizarSucursal(int sucursalId, [FromBody] Sucursal sucursal)
    {
        Sucursal existente = _gestionClientes.ObtenerSucursalPorId(sucursalId);
        if (existente == null)
            return BadRequest("Sucursal no encontrada.");

        Sucursal actualizada = _gestionClientes.ActualizarSucursal(sucursalId, sucursal);
        return Ok(actualizada);
    }

    [HttpDelete("baja-sucursal/{sucursalId}")]
    public object BajaSucursal(int sucursalId)
    {
        Sucursal sucursal = _gestionClientes.ObtenerSucursalPorId(sucursalId);
        if (sucursal == null)
            return BadRequest("Sucursal no encontrada.");

        bool ok = _gestionClientes.BajaSucursal(sucursalId);
        if (!ok)
            return BadRequest("No se puede dar de baja: la empresa debe tener al menos una sucursal activa.");

        return Ok(new { Mensaje = "Sucursal dada de baja correctamente." });
    }
}

public class ActualizarClienteDto
{
    public string Telefono { get; set; }
    public string Correo { get; set; }
    public string Direccion { get; set; }
}
