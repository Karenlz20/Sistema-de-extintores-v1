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
        _gestionDashboard = new GestionDashboard();
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
