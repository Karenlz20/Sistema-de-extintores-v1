using Microsoft.AspNetCore.Mvc;
using SistemaExtintores.Negocio;

namespace SistemaExtintores.Controllers;

[ApiController]
[Route("[controller]")]
public class DashboardController : ControllerBase
{
    private GestionDashboard _gestionDashboard;

    public DashboardController()
    {
        var gestionInventario = new GestionInventario();
        var gestionExtintores = new GestionExtintores(gestionInventario);
        _gestionDashboard = new GestionDashboard(
            gestionExtintores,
            new GestionClientes(),
            gestionInventario
        );
    }

    [HttpGet("resumen")]
    public object Resumen()
    {
        return _gestionDashboard.ObtenerResumen();
    }

    [HttpGet("clientes-urgentes")]
    public IEnumerable<object> ClientesUrgentes()
    {
        return _gestionDashboard.ObtenerClientesUrgentes();
    }
}