using System;
using System.Collections.Generic;

namespace SistemaExtintores.Modelos
{
    public class Venta
    {
        private const double IVA = 0.13;

        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public int ClienteId { get; set; }
        public int? SucursalId { get; set; }
        public int UsuarioId { get; set; }
        public double Descuento { get; set; }
        public EstadoVenta Estado { get; set; }
        public string NumeroComprobante { get; set; }
        public List<ItemVenta> Items { get; set; }

        public Venta()
        {
            Fecha = DateTime.Now;
            Estado = EstadoVenta.Pendiente;
            NumeroComprobante = "";
            Items = new List<ItemVenta>();
        }

        public Venta(int id, int clienteId, int usuarioId)
        {
            Id = id;
            ClienteId = clienteId;
            UsuarioId = usuarioId;
            Fecha = DateTime.Now;
            Estado = EstadoVenta.Pendiente;
            NumeroComprobante = "";
            Descuento = 0;
            Items = new List<ItemVenta>();
        }

        public void AgregarItem(ItemVenta item) { if (item != null) Items.Add(item); }

        public double ObtenerSubtotal()
        {
            double subtotal = 0;
            foreach (ItemVenta item in Items) subtotal += item.ObtenerSubtotal();
            return subtotal;
        }

        public double ObtenerMontoDescuento() { return ObtenerSubtotal() * (Descuento / 100); }

        public double ObtenerMontoIVA()
        {
            double subtotal = ObtenerSubtotal();
            double descuento = ObtenerMontoDescuento();
            return (subtotal - descuento) * IVA;
        }

        public double ObtenerTotal()
        {
            return ObtenerSubtotal() - ObtenerMontoDescuento() + ObtenerMontoIVA();
        }

        public string GenerarComprobante()
        {
            string correlativo = Id.ToString();
            while (correlativo.Length < 6) correlativo = "0" + correlativo;
            NumeroComprobante = "V-" + Fecha.Year + "-" + correlativo;
            return NumeroComprobante;
        }
    }
}
