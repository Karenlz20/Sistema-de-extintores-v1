using System.Text.Json.Serialization;

namespace SistemaExtintores.Modelos
{
    [JsonPolymorphic]
    [JsonDerivedType(typeof(ExtintorItem))]
    [JsonDerivedType(typeof(RecargaItem))]
    public abstract class ItemVenta
    {
        public int Id { get; set; }
        public int Cantidad { get; set; }
        public double PrecioUnitario { get; set; }

        public string Descripcion { get { return ObtenerDescripcion(); } }

        public ItemVenta() { }

        public ItemVenta(int id, int cantidad, double precioUnitario)
        {
            Id = id;
            Cantidad = cantidad;
            PrecioUnitario = precioUnitario;
        }

        public virtual double ObtenerSubtotal() { return Cantidad * PrecioUnitario; }

        public abstract string ObtenerDescripcion();
    }
}
