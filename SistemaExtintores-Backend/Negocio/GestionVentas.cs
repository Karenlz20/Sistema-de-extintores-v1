using SistemaExtintores.Data;
using SistemaExtintores.Modelos;

namespace SistemaExtintores.Negocio
{
    public class GestionVentas
    {
        private VentasData _data;
        private GestionInventario _gestionInventario;
        private GestionExtintores _gestionExtintores;

        public GestionVentas()
        {
            _data = new VentasData();
            _gestionInventario = new GestionInventario();
            _gestionExtintores = new GestionExtintores();
        }

        public List<Venta> ObtenerTodas()
        {
            return _data.ObtenerTodas();
        }

        public Venta ObtenerPorId(int id)
        {
            return _data.ObtenerPorId(id);
        }

        public List<Venta> ObtenerPorCliente(int clienteId)
        {
            return _data.ObtenerPorCliente(clienteId);
        }

        public List<Venta> ObtenerPorFecha(DateTime desde, DateTime hasta)
        {
            return _data.ObtenerPorFecha(desde, hasta);
        }

        public Venta CrearVenta(Venta venta)
        {
            venta.Fecha  = DateTime.Now;
            venta.Estado = EstadoVenta.Pendiente;
            venta.NumeroComprobante = "";
            return _data.InsertarVenta(venta);
        }

        public Venta AgregarItem(int ventaId, ItemVenta item)
        {
            Venta venta = _data.ObtenerPorId(ventaId);
            if (venta == null) return null;
            if (venta.Estado != EstadoVenta.Pendiente) return null;
            _data.InsertarItem(ventaId, item);
            return _data.ObtenerPorId(ventaId);
        }

        public Venta EliminarItem(int ventaId, int itemId)
        {
            Venta venta = _data.ObtenerPorId(ventaId);
            if (venta == null) return null;
            if (venta.Estado != EstadoVenta.Pendiente) return null;
            _data.EliminarItem(itemId);
            return _data.ObtenerPorId(ventaId);
        }

        public Venta AplicarDescuento(int ventaId, double porcentaje, int usuarioId)
        {
            Venta venta = _data.ObtenerPorId(ventaId);
            if (venta == null) return null;
            if (venta.Estado != EstadoVenta.Pendiente) return null;

            if (porcentaje > 15)
            {
                GestionUsuarios gestionUsuarios = new GestionUsuarios();
                Usuario usuario = gestionUsuarios.ObtenerPorId(usuarioId);
                if (usuario == null) return null;
                if (usuario.Rol != RolUsuario.Administrador) return null;
            }

            _data.ActualizarDescuento(ventaId, porcentaje);
            venta.Descuento = porcentaje;
            return venta;
        }

        public Venta ConfirmarVenta(int ventaId, int usuarioId, int clienteId)
        {
            Venta venta = _data.ObtenerPorId(ventaId);
            if (venta == null) return null;
            if (venta.Estado != EstadoVenta.Pendiente) return null;
            if (venta.Items.Count == 0) return null;

            if (clienteId > 0 && clienteId != venta.ClienteId)
            {
                _data.ActualizarCliente(ventaId, clienteId);
                venta.ClienteId = clienteId;
            }

            foreach (ItemVenta item in venta.Items)
            {
                if (item is ExtintorItem)
                {
                    ExtintorItem extItem = (ExtintorItem)item;

                    if (_gestionExtintores.ObtenerPorNroSerie(extItem.NroSerie) != null) return null;

                    CilindroInventario cilindro = _gestionInventario.ObtenerCilindroPorNroSerie(extItem.NroSerie);
                    if (cilindro != null && cilindro.Disponible)
                        _gestionInventario.MarcarVendido(cilindro.Id, usuarioId);

                    Extintor nuevoExtintor = new Extintor();
                    nuevoExtintor.NroSerie            = extItem.NroSerie;
                    nuevoExtintor.TipoAgente           = extItem.TipoAgente;
                    nuevoExtintor.Capacidad            = extItem.Capacidad;
                    nuevoExtintor.FechaFabricacion     = DateTime.Now;
                    nuevoExtintor.FechaUltimaCarga     = DateTime.Now;
                    nuevoExtintor.FechaVencimiento     = DateTime.Now.AddYears(1);
                    nuevoExtintor.ClienteId            = venta.ClienteId;
                    nuevoExtintor.SucursalId           = venta.SucursalId;
                    nuevoExtintor.CilindroInventarioId = cilindro != null ? cilindro.Id : (int?)null;

                    Extintor agregado = _gestionExtintores.Agregar(nuevoExtintor);
                    if (agregado == null) return null;
                }
                else if (item is RecargaItem)
                {
                    RecargaItem recItem = (RecargaItem)item;

                    Extintor extintor = _gestionExtintores.ObtenerPorNroSerie(recItem.NroSerieExtintor);
                    if (extintor == null) return null;

                    Recarga recarga = new Recarga();
                    recarga.AgenteUsado   = extintor.TipoAgente;
                    recarga.CantidadKg    = extintor.Capacidad;
                    recarga.InsumoId      = recItem.InsumoId;
                    recarga.Tecnico       = "";
                    recarga.Observaciones = "Recarga via venta " + venta.Id;

                    Extintor resultado = _gestionExtintores.AgregarRecarga(extintor.Id, recarga, usuarioId);
                    if (resultado == null) return null;
                }
            }

            venta.GenerarComprobante();
            _data.ActualizarEstado(ventaId, EstadoVenta.Confirmada, venta.NumeroComprobante);
            venta.Estado = EstadoVenta.Confirmada;
            return venta;
        }

        public bool AnularVenta(int ventaId)
        {
            Venta venta = _data.ObtenerPorId(ventaId);
            if (venta == null) return false;
            if (venta.Estado == EstadoVenta.Confirmada) return false;
            _data.ActualizarEstado(ventaId, EstadoVenta.Anulada, "");
            return true;
        }
    }
}
