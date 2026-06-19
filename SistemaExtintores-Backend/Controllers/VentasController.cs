using Microsoft.AspNetCore.Mvc;
using SistemaExtintores.Modelos;
using SistemaExtintores.Negocio;

namespace SistemaExtintores.Controllers;

[ApiController]
[Route("[controller]")]
public class VentasController : ControllerBase
{
    private GestionVentas _gestionVentas;

    public VentasController()
    {
        _gestionVentas = new GestionVentas();
    }

    [HttpGet("lista-ventas")]
    public IEnumerable<Venta> ListaVentas()
    {
        return _gestionVentas.ObtenerTodas();
    }

    [HttpGet("obtener-venta/{id}")]
    public Venta ObtenerPorId(int id)
    {
        return _gestionVentas.ObtenerPorId(id);
    }

    [HttpGet("por-cliente/{clienteId}")]
    public IEnumerable<Venta> ObtenerPorCliente(int clienteId)
    {
        return _gestionVentas.ObtenerPorCliente(clienteId);
    }

    [HttpGet("por-fecha")]
    public IEnumerable<Venta> ObtenerPorFecha([FromQuery] DateTime desde, [FromQuery] DateTime hasta)
    {
        return _gestionVentas.ObtenerPorFecha(desde, hasta);
    }

    [HttpPost("crear-venta")]
    public Venta CrearVenta([FromBody] Venta venta)
    {
        return _gestionVentas.CrearVenta(venta);
    }

    [HttpPost("agregar-item/{ventaId}")]
    public Venta AgregarItemExtintor(int ventaId, [FromBody] ExtintorItem item)
    {
        return _gestionVentas.AgregarItem(ventaId, item);
    }

    [HttpPost("agregar-item-recarga/{ventaId}")]
    public Venta AgregarItemRecarga(int ventaId, [FromBody] RecargaItem item)
    {
        return _gestionVentas.AgregarItem(ventaId, item);
    }

    [HttpPut("aplicar-descuento/{ventaId}/{usuarioId}")]
    public Venta AplicarDescuento(int ventaId, int usuarioId, [FromBody] double porcentaje)
    {
        return _gestionVentas.AplicarDescuento(ventaId, porcentaje, usuarioId);
    }

    [HttpPut("confirmar-venta/{ventaId}/{usuarioId}")]
    public Venta ConfirmarVenta(int ventaId, int usuarioId)
    {
        return _gestionVentas.ConfirmarVenta(ventaId, usuarioId);
    }

    [HttpPut("anular-venta/{ventaId}")]
    public bool AnularVenta(int ventaId)
    {
        return _gestionVentas.AnularVenta(ventaId);
    }
}
