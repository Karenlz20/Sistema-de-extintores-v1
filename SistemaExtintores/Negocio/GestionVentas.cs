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
            foreach (Venta venta in _ventas)
            {
                if (venta.ClienteId == clienteId)
                    resultado.Add(venta);
            }
            return resultado;
        }

        public Venta ObtenerPorId(int id)
        {
            foreach (Venta venta in _ventas)
            {
                if (venta.Id == id)
                    return venta;
            }
            return null;
        }

        public Venta CrearVenta(Venta venta)
        {
            venta.Id = _contadorId++;
            venta.Fecha = DateTime.Now;
            _ventas.Add(venta);
            return venta;
        }

        public Venta AgregarItem(int ventaId, ItemVenta item)
        {
            Venta venta = ObtenerPorId(ventaId);
            if (venta == null || venta.Confirmada) return null;

            venta.AgregarItem(item);
            return venta;
        }

        public Venta ConfirmarVenta(int ventaId)
        {
            Venta venta = ObtenerPorId(ventaId);
            if (venta == null || venta.Confirmada) return null;

            foreach (ItemVenta item in venta.Items)
            {
                if (item is ExtintorItem)
                {
                    ExtintorItem extItem = (ExtintorItem)item;
                    CilindroInventario cilindro = _gestionInventario.ObtenerCilindroPorNroSerie(extItem.NroSerie);
                    if (cilindro != null)
                    {
                        _gestionInventario.MarcarVendido(cilindro.Id);

                        Extintor nuevoExtintor = new Extintor();
                        nuevoExtintor.NroSerie = cilindro.NroSerie;
                        nuevoExtintor.TipoAgente = cilindro.TipoAgente;
                        nuevoExtintor.Capacidad = cilindro.Capacidad;
                        nuevoExtintor.FechaFabricacion = DateTime.Now;
                        nuevoExtintor.FechaUltimaCarga = DateTime.Now;
                        nuevoExtintor.FechaVencimiento = DateTime.Now.AddYears(1);
                        nuevoExtintor.ClienteId = venta.ClienteId;
                        nuevoExtintor.SucursalId = venta.SucursalId;
                        nuevoExtintor.CilindroInventarioId = cilindro.Id;

                        _gestionExtintores.Agregar(nuevoExtintor);
                    }
                }
            }

            venta.Confirmar();
            return venta;
        }
    }
}
