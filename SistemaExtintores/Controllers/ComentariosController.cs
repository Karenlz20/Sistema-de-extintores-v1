using Microsoft.AspNetCore.Mvc;
using SistemaExtintores.Modelos;
using SistemaExtintores.Negocio;

namespace SistemaExtintores.Controllers;

[ApiController]
[Route("[controller]")]
public class ComentariosController : ControllerBase
{
    private GestionComentarios _gestionComentarios;

    public ComentariosController()
    {
        var gestionInventario = new GestionInventario();
        var gestionExtintores = new GestionExtintores(gestionInventario);
        _gestionComentarios = new GestionComentarios(gestionExtintores);
    }

    [HttpGet("lista-comentarios")]
    public IEnumerable<object> ListaComentarios()
    {
        return _gestionComentarios.ObtenerTodos().Cast<object>();
    }

    [HttpGet("obtener-porExtintor/{id}")]
    public IEnumerable<object> ListaComentarioPorExtintor(int id)
    {
        return _gestionComentarios.ObtenerPorExtintor(id).Cast<object>();
    }

    [HttpGet("obtener-comentario/{id}")]
    public Comentario ObtenerComentario(int id)
    {
        return _gestionComentarios.ObtenerPorId(id);
    }

    [HttpPost("crear-comentario")]
    public Comentario CrearComentario([FromBody] Comentario comentario)
    {
        return _gestionComentarios.Agregar(comentario);
    }

    [HttpPut("editar-comentario/{id}")]
    public Comentario EditarComentario(int id, [FromBody] string nuevoTexto)
    {
        return _gestionComentarios.Actualizar(id, nuevoTexto);
    }

    [HttpDelete("eliminar-comentario/{id}")]
    public bool EliminarComentario(int id)
    {
        return _gestionComentarios.Eliminar(id);
    }
}