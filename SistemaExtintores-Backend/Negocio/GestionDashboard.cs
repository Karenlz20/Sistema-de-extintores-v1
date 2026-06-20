using SistemaExtintores.Modelos;

namespace SistemaExtintores.Negocio
{
    public class GestionDashboard
    {
        private GestionExtintores _gestionExtintores;
        private GestionClientes _gestionClientes;
        private GestionInventario _gestionInventario;

        public GestionDashboard()
        {
            _gestionExtintores = new GestionExtintores();
            _gestionClientes = new GestionClientes();
            _gestionInventario = new GestionInventario();
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
            GestionClientes gestionClientes = new GestionClientes();
            Dictionary<int, ClienteUrgenteDto> agrupados = new Dictionary<int, ClienteUrgenteDto>();

            foreach (Extintor e in _gestionExtintores.ObtenerTodos())
            {
                EstadoVigencia estado = e.ObtenerEstado();
                if (estado == EstadoVigencia.Vigente) continue;

                if (!agrupados.ContainsKey(e.ClienteId))
                {
                    Cliente c = gestionClientes.ObtenerPorId(e.ClienteId);
                    if (c == null) continue;

                    ClienteUrgenteDto dto = new ClienteUrgenteDto();
                    dto.ClienteId = c.Id;
                    dto.Nombre    = c is ClienteEmpresa ? ((ClienteEmpresa)c).RazonSocial : ((ClientePersona)c).NombreCompleto;
                    dto.Telefono  = c.Telefono;
                    dto.Cantidad  = 0;
                    dto.FechaVencimientoMasProxima = e.FechaVencimiento;
                    agrupados[e.ClienteId] = dto;
                }

                ClienteUrgenteDto urgente = agrupados[e.ClienteId];
                urgente.Cantidad++;
                if (e.FechaVencimiento < urgente.FechaVencimientoMasProxima)
                    urgente.FechaVencimientoMasProxima = e.FechaVencimiento;
            }

            List<ClienteUrgenteDto> lista = new List<ClienteUrgenteDto>();
            foreach (KeyValuePair<int, ClienteUrgenteDto> par in agrupados)
                lista.Add(par.Value);

            // Bubble sort por fecha de vencimiento más próxima
            for (int i = 0; i < lista.Count - 1; i++)
            {
                for (int j = 0; j < lista.Count - i - 1; j++)
                {
                    if (lista[j].FechaVencimientoMasProxima > lista[j + 1].FechaVencimientoMasProxima)
                    {
                        ClienteUrgenteDto temp = lista[j];
                        lista[j] = lista[j + 1];
                        lista[j + 1] = temp;
                    }
                }
            }

            List<object> resultado = new List<object>();
            foreach (ClienteUrgenteDto u in lista) resultado.Add(u);
            return resultado;
        }
    }

    public class ClienteUrgenteDto
    {
        public int ClienteId { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaVencimientoMasProxima { get; set; }
    }
}