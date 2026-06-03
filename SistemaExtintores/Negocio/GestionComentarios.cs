using SistemaExtintores.Modelos;

namespace SistemaExtintores.Negocio
{
    public class GestionComentarios
    {
        private static List<Comentario> _comentarios = new List<Comentario>
        {
            new Comentario(1, "Bueno", "Carlos Mendez", DateTime.Now, 1),
            new Comentario(2, "Excelente estado", "Ana Flores", DateTime.Now,2)
        };
        private static int _contadorId = 3;

        private GestionExtintores _gestionExtintores; //Para ver si tenemos el extintor con id del comentario

        public GestionComentarios(GestionExtintores gestionExtintores) //Agregamos la gestión de extintores para verificar
        {
            _gestionExtintores = gestionExtintores;
        }

        public List<Comentario> ObtenerTodos()
        {
            return _comentarios;
        }

        public List<Comentario> ObtenerPorExtintor(int extintorId)
        {
            List<Comentario> resultado = new List<Comentario>();
            foreach (var comentario in _comentarios)
            {
                if (comentario.GetExtintorId() == extintorId)
                {
                    resultado.Add(comentario);
                }
            }
            return resultado;
        }

        public Comentario ObtenerPorId(int id)
        {
            foreach (var comentario in _comentarios)
            {
                if (comentario.GetId() == id)
                {
                    return comentario;
                }
            }
            return null;
        }

        public Comentario Agregar(Comentario comentario)
        {
            if (comentario.GetTexto() == null)
            {
                return null;
            }
            var extintor = _gestionExtintores.ObtenerPorId(comentario.GetExtintorId());
            if (extintor == null)
            {
                return null;
            } 
            comentario.SetId(_contadorId++);
            comentario.SetFechaCreacion(DateTime.Now);
            _comentarios.Add(comentario);
            return comentario;
        }

        public Comentario Actualizar(int id, string nuevoTexto)
        {
            if (nuevoTexto == null)
            {
                return null;
            }

            var comentario = ObtenerPorId(id);
            if (comentario == null)
            {
                return null;
            }
            comentario.SetTexto(nuevoTexto);
            return comentario;
        }

        public bool Eliminar(int id)
        {
            var comentario = ObtenerPorId(id);
            if (comentario == null)
            {
                return false;
            }
            _comentarios.Remove(comentario);
            return true;
        }
    }
}