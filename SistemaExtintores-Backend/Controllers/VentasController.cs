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
        var gestionInventario = new GestionInventario();
        var gestionExtintores = new GestionExtintores(gestionInventario);
        _gestionVentas = new GestionVentas(gestionInventario, gestionExtintores);
    }

    [HttpGet("lista-ventas")]
    public IEnumerable<object> ListaVentas()
    {
        return _gestionVentas.ObtenerTodas().Cast<object>();
    }

    [HttpGet("obtener-venta/{id}")]
    public Venta ObtenerPorId(int id)
    {
        return _gestionVentas.ObtenerPorId(id);
    }

    [HttpGet("factura/{id}")]
    public object Factura(int id)
    {
        var venta = _gestionVentas.ObtenerPorId(id);
        if (venta == null) return null;
        return venta.ObtenerResumen();
    }

    [HttpGet("por-cliente/{clienteId}")]
    public IEnumerable<Venta> ObtenerPorCliente(int clienteId)
    {
        return _gestionVentas.ObtenerPorCliente(clienteId);
    }

    [HttpPost("crear-venta")]
    public Venta CrearVenta([FromBody] Venta venta)
    {
        return _gestionVentas.CrearVenta(venta);
    }

    [HttpPost("agregar-item/{ventaId}")]
    public Venta AgregarItem(int ventaId, [FromBody] ItemVenta item)
    {
        return _gestionVentas.AgregarItem(ventaId, item);
    }

    [HttpPut("confirmar-venta/{ventaId}")]
    public Venta ConfirmarVenta(int ventaId)
    {
        return _gestionVentas.ConfirmarVenta(ventaId);
    }
}