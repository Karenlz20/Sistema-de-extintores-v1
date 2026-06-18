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
        GestionInventario gestionInventario = new GestionInventario();
        GestionExtintores gestionExtintores = new GestionExtintores(gestionInventario);
        GestionClientes gestionClientes = new GestionClientes(gestionExtintores);
        _gestionDashboard = new GestionDashboard(
            gestionExtintores,
            gestionClientes,
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