using SistemaExtintores.Modelos;

namespace SistemaExtintores.Negocio
{
    public class GestionVentas
    {
        private static List<Venta> _ventas = new List<Venta>
        {
            new Venta(1, 1, "Ana Flores", 0, DateTime.Now.AddDays(-5))
        };
        private static int _contadorId = 2;

        private GestionInventario _gestionInventario;
        private GestionExtintores _gestionExtintores;

        public GestionVentas(GestionInventario gestionInventario, GestionExtintores gestionExtintores)
        {
            _gestionInventario = gestionInventario;
            _gestionExtintores = gestionExtintores;
        }

        public List<Venta> ObtenerTodas() { return _ventas; }

        public List<Venta> ObtenerPorCliente(int clienteId)
        {
            List<Venta> resultado = new List<Venta>();
            foreach (var venta in _ventas)
            {
                if (venta.GetClienteId() == clienteId)
                    resultado.Add(venta);
            }
            return resultado;
        }

        public Venta ObtenerPorId(int id)
        {
            foreach (var venta in _ventas)
            {
                if (venta.GetId() == id)
                    return venta;
            }
            return null;
        }

        public Venta CrearVenta(Venta venta)
        {
            venta.SetId(_contadorId++);
            venta.SetFecha(DateTime.Now);
            _ventas.Add(venta);
            return venta;
        }

        public Venta AgregarItem(int ventaId, ItemVenta item)
        {
            var venta = ObtenerPorId(ventaId);
            if (venta == null || venta.GetConfirmada()) return null;

            venta.AgregarItem(item);
            return venta;
        }

        public Venta ConfirmarVenta(int ventaId)
        {
            var venta = ObtenerPorId(ventaId);
            if (venta == null || venta.GetConfirmada()) return null;

            foreach (var item in venta.GetItems())
            {
                if (item is ExtintorItem extItem)
                {
                    var cilindro = _gestionInventario.ObtenerCilindroPorNroSerie(extItem.GetNroSerie());
                    if (cilindro != null)
                    {
                        _gestionInventario.MarcarVendido(cilindro.GetId());

                        Extintor nuevoExtintor = new Extintor();
                        nuevoExtintor.SetNroSerie(cilindro.GetNroSerie());
                        nuevoExtintor.SetTipoAgente(cilindro.GetTipoAgente());
                        nuevoExtintor.SetCapacidad(cilindro.GetCapacidad());
                        nuevoExtintor.SetFechaFabricacion(DateTime.Now);
                        nuevoExtintor.SetFechaUltimaCarga(DateTime.Now);
                        nuevoExtintor.SetFechaVencimiento(DateTime.Now.AddYears(1));
                        nuevoExtintor.SetClienteId(venta.GetClienteId());
                        nuevoExtintor.SetSucursalId(venta.GetSucursalId());
                        nuevoExtintor.SetCilindroInventarioId(cilindro.GetId());

                        _gestionExtintores.Agregar(nuevoExtintor);
                    }
                }
            }

            venta.Confirmar();
            return venta;
        }
    }
}