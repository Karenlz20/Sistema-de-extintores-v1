using System;
using System.Collections.Generic;
using SistemaExtintores.Modelos;

namespace SistemaExtintores.Negocio
{
    public class GestionVentas
    {
        private static List<Venta> _ventas = new List<Venta>();
        private static int _contadorId = 1;

        private GestionInventario _gestionInventario;
        private GestionExtintores _gestionExtintores;

        public GestionVentas()
        {
            _gestionInventario = new GestionInventario();
            _gestionExtintores = new GestionExtintores();
        }

        public List<Venta> ObtenerTodas()
        {
            return _ventas;
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

        public List<Venta> ObtenerPorFecha(DateTime desde, DateTime hasta)
        {
            List<Venta> resultado = new List<Venta>();
            foreach (Venta venta in _ventas)
            {
                if (venta.Fecha >= desde && venta.Fecha <= hasta)
                    resultado.Add(venta);
            }
            return resultado;
        }

        public Venta CrearVenta(Venta venta)
        {
            venta.Id = _contadorId;
            _contadorId++;
            venta.Fecha = DateTime.Now;
            venta.Estado = EstadoVenta.Pendiente;
            _ventas.Add(venta);
            return venta;
        }

        public Venta AgregarItem(int ventaId, ItemVenta item)
        {
            Venta venta = ObtenerPorId(ventaId);
            if (venta == null) return null;
            if (venta.Estado != EstadoVenta.Pendiente) return null;
            venta.AgregarItem(item);
            return venta;
        }

        public Venta AplicarDescuento(int ventaId, double porcentaje, int usuarioId)
        {
            Venta venta = ObtenerPorId(ventaId);
            if (venta == null) return null;
            if (venta.Estado != EstadoVenta.Pendiente) return null;

            if (porcentaje > 15)
            {
                GestionUsuarios gestionUsuarios = new GestionUsuarios();
                Usuario usuario = gestionUsuarios.ObtenerPorId(usuarioId);
                if (usuario == null) return null;
                if (usuario.Rol != RolUsuario.Administrador) return null;
            }

            venta.Descuento = porcentaje;
            return venta;
        }

        public Venta ConfirmarVenta(int ventaId, int usuarioId)
        {
            Venta venta = ObtenerPorId(ventaId);
            if (venta == null) return null;
            if (venta.Estado != EstadoVenta.Pendiente) return null;
            if (venta.Items.Count == 0) return null;

            foreach (ItemVenta item in venta.Items)
            {
                if (item is ExtintorItem)
                {
                    ExtintorItem extItem = (ExtintorItem)item;

                    Extintor extintorExistente = _gestionExtintores.ObtenerPorNroSerie(extItem.NroSerie);
                    if (extintorExistente != null) return null;

                    CilindroInventario cilindro = _gestionInventario.ObtenerCilindroPorNroSerie(extItem.NroSerie);
                    if (cilindro != null && cilindro.Disponible)
                        _gestionInventario.MarcarVendido(cilindro.Id, usuarioId);

                    Extintor nuevoExtintor = new Extintor();
                    nuevoExtintor.NroSerie = extItem.NroSerie;
                    nuevoExtintor.TipoAgente = extItem.TipoAgente;
                    nuevoExtintor.Capacidad = extItem.Capacidad;
                    nuevoExtintor.FechaFabricacion = DateTime.Now;
                    nuevoExtintor.FechaUltimaCarga = DateTime.Now;
                    nuevoExtintor.FechaVencimiento = DateTime.Now.AddYears(1);
                    nuevoExtintor.ClienteId = venta.ClienteId;
                    nuevoExtintor.SucursalId = venta.SucursalId;
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
                    recarga.AgenteUsado = "";
                    recarga.CantidadKg = item.Cantidad;
                    recarga.InsumoId = recItem.InsumoId;
                    recarga.Tecnico = "";
                    recarga.Observaciones = "Recarga via venta " + venta.Id;

                    Extintor resultado = _gestionExtintores.AgregarRecarga(extintor.Id, recarga, usuarioId);
                    if (resultado == null) return null;
                }
            }

            venta.Estado = EstadoVenta.Confirmada;
            venta.GenerarComprobante();
            return venta;
        }

        public bool AnularVenta(int ventaId)
        {
            Venta venta = ObtenerPorId(ventaId);
            if (venta == null) return false;
            if (venta.Estado == EstadoVenta.Confirmada) return false;
            venta.Estado = EstadoVenta.Anulada;
            return true;
        }
    }
}
