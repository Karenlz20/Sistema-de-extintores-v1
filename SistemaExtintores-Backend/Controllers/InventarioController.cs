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

    [HttpPost("agregar-cilindro/{usuarioId}")]
    public CilindroInventario AgregarCilindro(int usuarioId, [FromBody] CilindroInventario cilindro)
    {
        return _gestionInventario.AgregarCilindro(cilindro, usuarioId);
    }

    [HttpDelete("eliminar-cilindro/{id}")]
    public bool EliminarCilindro(int id)
    {
        return _gestionInventario.EliminarCilindro(id);
    }

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

    [HttpPost("agregar-insumo/{usuarioId}")]
    public InsumoInventario AgregarInsumo(int usuarioId, [FromBody] InsumoInventario insumo)
    {
        return _gestionInventario.AgregarInsumo(insumo, usuarioId);
    }

    [HttpPut("actualizar-insumo/{id}")]
    public InsumoInventario ActualizarInsumo(int id, [FromBody] InsumoInventario insumo)
    {
        return _gestionInventario.ActualizarInsumo(id, insumo);
    }

    [HttpPut("ingreso-insumo/{id}/{usuarioId}")]
    public bool RegistrarIngresoInsumo(int id, int usuarioId, [FromBody] double cantidad)
    {
        return _gestionInventario.RegistrarIngresoInsumo(id, cantidad, usuarioId);
    }

    [HttpPut("ajuste-insumo/{id}/{usuarioId}")]
    public bool RegistrarAjusteInsumo(int id, int usuarioId, [FromBody] AjusteInsumoRequest request)
    {
        return _gestionInventario.RegistrarAjusteInsumo(id, request.Cantidad, usuarioId, request.Observacion);
    }

    [HttpDelete("eliminar-insumo/{id}")]
    public bool EliminarInsumo(int id)
    {
        return _gestionInventario.EliminarInsumo(id);
    }

    [HttpGet("movimientos")]
    public IEnumerable<MovimientoInventario> ListaMovimientos()
    {
        return _gestionInventario.ObtenerMovimientos();
    }
}

public class AjusteInsumoRequest
{
    public double Cantidad { get; set; }
    public string Observacion { get; set; }
}