using SistemaExtintores.Modelos;

namespace SistemaExtintores.Negocio
{
    public class GestionDashboard
    {
        private GestionExtintores _gestionExtintores;
        private GestionClientes _gestionClientes;
        private GestionInventario _gestionInventario;

        public GestionDashboard(GestionExtintores gestionExtintores,
            GestionClientes gestionClientes,
            GestionInventario gestionInventario)
        {
            _gestionExtintores = gestionExtintores;
            _gestionClientes = gestionClientes;
            _gestionInventario = gestionInventario;
        }

        public object ObtenerResumen()
        {
            List<Extintor> extintores = _gestionExtintores.ObtenerTodos();

            int vigentes = 0;
            int proximos = 0;
            int fueraNorma = 0;

            foreach (Extintor e in extintores)
            {
                if (e.ObtenerEstado() == EstadoVigencia.Vigente) vigentes++;
                else if (e.ObtenerEstado() == EstadoVigencia.ProximoAVencer) proximos++;
                else fueraNorma++;
            }

            return new
            {
                TotalExtintores = extintores.Count,
                Vigentes = vigentes,
                ProximosAVencer = proximos,
                FueraDeNorma = fueraNorma,
                CilindrosDisponibles = _gestionInventario.ObtenerCilindrosDisponibles().Count,
                InsumosBajoReorden = _gestionInventario.ObtenerInsumosBajoReorden().Count,
                TotalClientes = _gestionClientes.ObtenerTodos().Count
            };
        }

        public List<object> ObtenerClientesUrgentes()
        {
            List<object> resultado = new List<object>();

            foreach (Extintor e in _gestionExtintores.ObtenerTodos())
            {
                if (e.ObtenerEstado() != EstadoVigencia.Vigente)
                {
                    resultado.Add(new
                    {
                        ExtintorId = e.Id,
                        NroSerie = e.NroSerie,
                        Estado = e.ObtenerEstado().ToString(),
                        ClienteId = e.ClienteId,
                        FechaVencimiento = e.FechaVencimiento
                    });
                }
            }

            return resultado;
        }
    }
}
