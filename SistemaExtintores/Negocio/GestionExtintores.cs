using SistemaExtintores.Modelos;

namespace SistemaExtintores.Negocio
{
    public class GestionExtintores
    {
        private static List<Extintor> _extintores = new List<Extintor>
        {
            new Extintor(1, "EXT-001", "PQS", 6, new DateTime(2020, 1, 15), new DateTime(2023, 1, 15), DateTime.Now.AddDays(60), 2, null, null),
            new Extintor(2, "EXT-002", "PQS", 4, new DateTime(2019, 6, 10), new DateTime(2022, 6, 10), DateTime.Now.AddDays(-10), 3, null, null),
            new Extintor(3, "CIL-004", "CO2", 10, DateTime.Now.AddDays(-5), DateTime.Now.AddDays(-5), DateTime.Now.AddDays(-5).AddYears(1), 1, null, 4)
        };
        private static int _contadorId = 4;
        private static int _contadorHojaVida = 4;
        private static int _contadorRecarga = 2;

        private GestionInventario _gestionInventario;

        public GestionExtintores(GestionInventario gestionInventario)
        {
            _gestionInventario = gestionInventario;
        }

        public List<Extintor> ObtenerTodos() { return _extintores; }

        public Extintor ObtenerPorId(int id)
        {
            foreach (var extintor in _extintores)
            {
                if (extintor.GetId() == id)
                    return extintor;
            }
            return null;
        }

        public List<Extintor> ObtenerPorCliente(int clienteId)
        {
            List<Extintor> resultado = new List<Extintor>();
            foreach (var extintor in _extintores)
            {
                if (extintor.GetClienteId() == clienteId)
                    resultado.Add(extintor);
            }
            return resultado;
        }

        public List<Extintor> ObtenerPorEstado(EstadoVigencia estado)
        {
            List<Extintor> resultado = new List<Extintor>();
            foreach (var extintor in _extintores)
            {
                if (extintor.ObtenerEstado() == estado)
                    resultado.Add(extintor);
            }
            return resultado;
        }

        public Extintor Agregar(Extintor extintor)
        {
            extintor.SetId(_contadorId++);
            HojaDeVida hoja = new HojaDeVida();
            hoja.SetId(_contadorHojaVida++);
            hoja.SetFechaRegistroInicial(DateTime.Now);
            extintor.SetHojaDeVida(hoja);
            _extintores.Add(extintor);
            return extintor;
        }

        public Extintor Actualizar(int id, Extintor extintorActualizado)
        {
            var existente = ObtenerPorId(id);
            if (existente == null) return null;

            existente.SetTipoAgente(extintorActualizado.GetTipoAgente());
            existente.SetCapacidad(extintorActualizado.GetCapacidad());
            existente.SetFechaUltimaCarga(extintorActualizado.GetFechaUltimaCarga());
            existente.SetFechaVencimiento(extintorActualizado.GetFechaVencimiento());

            return existente;
        }

        public bool Eliminar(int id)
        {
            var extintor = ObtenerPorId(id);
            if (extintor == null) return false;

            _extintores.Remove(extintor);
            return true;
        }

        public Extintor AgregarRecarga(int extintorId, Recarga recarga)
        {
            var extintor = ObtenerPorId(extintorId);
            if (extintor == null) return null;

            foreach (var insumo in _gestionInventario.ObtenerInsumos())
            {
                if (insumo.GetNombre() == recarga.GetAgenteUsado())
                {
                    _gestionInventario.ActualizarStockInsumo(
                        insumo.GetId(),
                        -extintor.GetCapacidad()
                    );
                    break;
                }
            }

            recarga.SetId(_contadorRecarga++);
            recarga.SetFecha(DateTime.Now);
            extintor.GetHojaDeVida().AgregarRecarga(recarga);
            extintor.SetFechaUltimaCarga(recarga.GetFecha());

            return extintor;
        }
    }
}