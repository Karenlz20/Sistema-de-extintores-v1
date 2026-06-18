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
            foreach (Extintor extintor in _extintores)
            {
                if (extintor.Id == id)
                    return extintor;
            }
            return null;
        }

        public List<Extintor> ObtenerPorCliente(int clienteId)
        {
            List<Extintor> resultado = new List<Extintor>();
            foreach (Extintor extintor in _extintores)
            {
                if (extintor.ClienteId == clienteId)
                    resultado.Add(extintor);
            }
            return resultado;
        }

        public List<Extintor> ObtenerPorEstado(EstadoVigencia estado)
        {
            List<Extintor> resultado = new List<Extintor>();
            foreach (Extintor extintor in _extintores)
            {
                if (extintor.ObtenerEstado() == estado)
                    resultado.Add(extintor);
            }
            return resultado;
        }

        public Extintor Agregar(Extintor extintor)
        {
            extintor.Id = _contadorId++;
            HojaDeVida hoja = new HojaDeVida();
            hoja.Id = _contadorHojaVida++;
            hoja.FechaRegistroInicial = DateTime.Now;
            extintor.HojaDeVida = hoja;
            _extintores.Add(extintor);
            return extintor;
        }

        public Extintor Actualizar(int id, Extintor extintorActualizado)
        {
            Extintor existente = ObtenerPorId(id);
            if (existente == null) return null;

            existente.TipoAgente = extintorActualizado.TipoAgente;
            existente.Capacidad = extintorActualizado.Capacidad;
            existente.FechaUltimaCarga = extintorActualizado.FechaUltimaCarga;
            existente.FechaVencimiento = extintorActualizado.FechaVencimiento;

            return existente;
        }

        public bool Eliminar(int id)
        {
            Extintor extintor = ObtenerPorId(id);
            if (extintor == null) return false;

            _extintores.Remove(extintor);
            return true;
        }

        public Extintor AgregarRecarga(int extintorId, Recarga recarga)
        {
            Extintor extintor = ObtenerPorId(extintorId);
            if (extintor == null) return null;

            foreach (InsumoInventario insumo in _gestionInventario.ObtenerInsumos())
            {
                if (insumo.Nombre == recarga.AgenteUsado)
                {
                    _gestionInventario.ActualizarStockInsumo(insumo.Id, -extintor.Capacidad);
                    break;
                }
            }

            recarga.Id = _contadorRecarga++;
            recarga.Fecha = DateTime.Now;
            extintor.HojaDeVida.AgregarRecarga(recarga);
            extintor.FechaUltimaCarga = recarga.Fecha;

            return extintor;
        }
    }
}
