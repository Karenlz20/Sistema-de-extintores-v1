using Microsoft.AspNetCore.Mvc;
using SistemaExtintores.Modelos;
using SistemaExtintores.Negocio;

namespace SistemaExtintores.Controllers;

[ApiController]
[Route("[controller]")]
public class InventarioController : ControllerBase
{
    private GestionInventario _gestionInventario;

    public InventarioController()
    {
        _gestionInventario = new GestionInventario();
    }

    // ── Cilindros ──

    [HttpGet("lista-cilindros")]
    public IEnumerable<CilindroInventario> ListaCilindros()
    {
        return _gestionInventario.ObtenerCilindros();
    }

    [HttpGet("cilindros-disponibles")]
    public IEnumerable<CilindroInventario> CilindrosDisponibles()
    {
        return _gestionInventario.ObtenerCilindrosDisponibles();
    }

    [HttpGet("cilindros-vendidos")]
    public IEnumerable<CilindroInventario> CilindrosVendidos()
    {
        return _gestionInventario.ObtenerCilindrosVendidos();
    }

    [HttpGet("obtener-cilindro/{id}")]
    public CilindroInventario ObtenerCilindroPorId(int id)
    {
        return _gestionInventario.ObtenerCilindroPorId(id);
    }

    [HttpGet("buscar-cilindro/{nroSerie}")]
    public CilindroInventario BuscarCilindroPorSerie(string nroSerie)
    {
        return _gestionInventario.ObtenerCilindroPorNroSerie(nroSerie);
    }

    [HttpPost("agregar-cilindro")]
    public CilindroInventario AgregarCilindro([FromBody] CilindroInventario cilindro)
    {
        return _gestionInventario.AgregarCilindro(cilindro);
    }

    [HttpPut("marcar-vendido/{id}")]
    public CilindroInventario MarcarVendido(int id)
    {
        return _gestionInventario.MarcarVendido(id);
    }

    [HttpDelete("eliminar-cilindro/{id}")]
    public bool EliminarCilindro(int id)
    {
        return _gestionInventario.EliminarCilindro(id);
    }

    // ── Insumos ──

    [HttpGet("lista-insumos")]
    public IEnumerable<InsumoInventario> ListaInsumos()
    {
        return _gestionInventario.ObtenerInsumos();
    }

    [HttpGet("insumos-bajo-reorden")]
    public IEnumerable<InsumoInventario> InsumosBajoReorden()
    {
        return _gestionInventario.ObtenerInsumosBajoReorden();
    }

    [HttpGet("obtener-insumo/{id}")]
    public InsumoInventario ObtenerInsumoPorId(int id)
    {
        return _gestionInventario.ObtenerInsumoPorId(id);
    }

    [HttpPost("agregar-insumo")]
    public InsumoInventario AgregarInsumo([FromBody] InsumoInventario insumo)
    {
        return _gestionInventario.AgregarInsumo(insumo);
    }

    [HttpPut("actualizar-insumo/{id}")]
    public InsumoInventario ActualizarInsumo(int id, [FromBody] InsumoInventario insumo)
    {
        return _gestionInventario.ActualizarInsumo(id, insumo);
    }

    [HttpPut("actualizar-stock-insumo/{id}")]
    public InsumoInventario ActualizarStockInsumo(int id, [FromBody] double cantidad)
    {
        return _gestionInventario.ActualizarStockInsumo(id, cantidad);
    }

    [HttpDelete("eliminar-insumo/{id}")]
    public bool EliminarInsumo(int id)
    {
        return _gestionInventario.EliminarInsumo(id);
    }
}