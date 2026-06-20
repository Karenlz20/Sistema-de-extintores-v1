using SistemaExtintores.Data;
using SistemaExtintores.Modelos;

namespace SistemaExtintores.Negocio
{
    public class GestionExtintores
    {
        private ExtintoresData _data;
        private GestionInventario _gestionInventario;

        public GestionExtintores()
        {
            _data = new ExtintoresData();
            _gestionInventario = new GestionInventario();
        }

        public List<Extintor> ObtenerTodos()
        {
            return _data.ObtenerTodos();
        }

        public Extintor ObtenerPorId(int id)
        {
            return _data.ObtenerPorId(id);
        }

        public Extintor ObtenerPorNroSerie(string nroSerie)
        {
            return _data.ObtenerPorNroSerie(nroSerie);
        }

        public List<Extintor> ObtenerPorCliente(int clienteId)
        {
            return _data.ObtenerPorCliente(clienteId);
        }

        public List<Extintor> ObtenerPorEstado(EstadoVigencia estado)
        {
            List<Extintor> todos = _data.ObtenerTodos();
            List<Extintor> resultado = new List<Extintor>();
            foreach (Extintor e in todos)
            {
                if (e.ObtenerEstado() == estado) resultado.Add(e);
            }
            return resultado;
        }

        public Extintor Agregar(Extintor extintor)
        {
            if (_data.ObtenerPorNroSerie(extintor.NroSerie) != null) return null;
            return _data.Insertar(extintor);
        }

        public Extintor Actualizar(int id, Extintor extintorActualizado)
        {
            Extintor existente = ObtenerPorId(id);
            if (existente == null) return null;
            existente.TipoAgente       = extintorActualizado.TipoAgente;
            existente.Capacidad        = extintorActualizado.Capacidad;
            existente.FechaFabricacion = extintorActualizado.FechaFabricacion;
            existente.FechaUltimaCarga = extintorActualizado.FechaUltimaCarga;
            existente.FechaVencimiento = extintorActualizado.FechaVencimiento;
            existente.ClienteId        = extintorActualizado.ClienteId;
            existente.SucursalId       = extintorActualizado.SucursalId;
            return existente;
        }

        public bool Eliminar(int id)
        {
            return _data.Eliminar(id);
        }

        public Extintor AgregarRecarga(int extintorId, Recarga recarga, int usuarioId)
        {
            Extintor extintor = ObtenerPorId(extintorId);
            if (extintor == null) return null;

            // Pre-validación: chequear stock de nitrógeno ANTES de descontar el agente principal
            // (para no quedar con inventario inconsistente si falla el segundo descuento)
            InsumoInventario presurizante = null;
            double cantidadN2 = 0;
            bool necesitaNitrogeno = !extintor.TipoAgente.Equals("CO2", StringComparison.OrdinalIgnoreCase);
            if (necesitaNitrogeno)
            {
                presurizante = _gestionInventario.ObtenerInsumoPresurizante();
                if (presurizante == null) return null;
                cantidadN2 = Math.Round(recarga.CantidadKg * 0.1, 2);
                if (presurizante.Stock < cantidadN2) return null;
            }

            bool descontado = _gestionInventario.DescontarInsumo(recarga.InsumoId, recarga.CantidadKg, usuarioId);
            if (!descontado) return null;

            if (necesitaNitrogeno && presurizante != null)
            {
                bool descontadoN2 = _gestionInventario.DescontarInsumo(presurizante.Id, cantidadN2, usuarioId);
                if (!descontadoN2) return null;
            }

            recarga.Fecha = DateTime.Now;
            Recarga insertada = _data.InsertarRecarga(extintorId, recarga);

            DateTime nuevaUltimaCarga = recarga.Fecha;
            DateTime nuevaVencimiento = recarga.Fecha.AddYears(1);
            _data.ActualizarFechas(extintorId, nuevaUltimaCarga, nuevaVencimiento);

            extintor.FechaUltimaCarga = nuevaUltimaCarga;
            extintor.FechaVencimiento = nuevaVencimiento;
            extintor.HojaDeVida.AgregarRecarga(insertada);
            return extintor;
        }
    }
}
