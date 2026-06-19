using System;

namespace SistemaExtintores.Modelos
{
    public class MovimientoInventario
    {
        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        public TipoMovimiento Tipo { get; set; } 

        public int ItemId { get; set; }

        public TipoItemInventario TipoItem { get; set; }

        public double Cantidad { get; set; }

        public int UsuarioId { get; set; }

        public string Observacion { get; set; }

        public MovimientoInventario()
        {
        }

        public MovimientoInventario(int id, TipoMovimiento tipo, int itemId, TipoItemInventario tipoItem, double cantidad, int usuarioId, string observacion)
        {
            Id = id;
            Fecha = DateTime.Now;
            Tipo = tipo;
            ItemId = itemId;
            TipoItem = tipoItem;
            Cantidad = cantidad;
            UsuarioId = usuarioId;
            Observacion = observacion;
        }
    }
}