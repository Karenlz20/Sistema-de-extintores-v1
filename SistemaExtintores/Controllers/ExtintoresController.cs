using Microsoft.AspNetCore.Mvc;
using SistemaExtintores.Modelos;
using SistemaExtintores.Negocio;

namespace SistemaExtintores.Controllers;

[ApiController]
[Route("[controller]")]
public class ExtintoresController : ControllerBase
{
    private GestionExtintores _gestionExtintores;

    public ExtintoresController()
    {
        var gestionInventario = new GestionInventario();
        _gestionExtintores = new GestionExtintores(gestionInventario);
    }

    [HttpGet("lista-extintores")]
    public IEnumerable<Extintor> ListaExtintores()
    {
        return _gestionExtintores.ObtenerTodos();
    }

    [HttpGet("obtener-extintor/{id}")]
    public Extintor ObtenerPorId(int id)
    {
        return _gestionExtintores.ObtenerPorId(id);
    }

    [HttpGet("por-cliente/{clienteId}")]
    public IEnumerable<Extintor> ObtenerPorCliente(int clienteId)
    {
        return _gestionExtintores.ObtenerPorCliente(clienteId);
    }

    [HttpGet("por-estado/{estado}")]
    public IEnumerable<Extintor> ObtenerPorEstado(EstadoVigencia estado)
    {
        return _gestionExtintores.ObtenerPorEstado(estado);
    }

    [HttpPost("agregar-extintor")]
    public Extintor AgregarExtintor([FromBody] Extintor extintor)
    {
        return _gestionExtintores.Agregar(extintor);
    }

    [HttpPut("actualizar-extintor/{id}")]
    public Extintor Actualizar(int id, [FromBody] Extintor extintor)
    {
        return _gestionExtintores.Actualizar(id, extintor);
    }

    [HttpPost("agregar-recarga/{extintorId}")]
    public Extintor AgregarRecarga(int extintorId, [FromBody] Recarga recarga)
    {
        return _gestionExtintores.AgregarRecarga(extintorId, recarga);
    }

    [HttpDelete("eliminar-extintor/{id}")]
    public bool Eliminar(int id)
    {
        return _gestionExtintores.Eliminar(id);
    }
}