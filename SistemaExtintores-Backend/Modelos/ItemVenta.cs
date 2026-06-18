namespace SistemaExtintores.Modelos
{
    public abstract class ItemVenta
    {
        public int Id { get; set; }
        public int Cantidad { get; set; }
        public double PrecioUnitario { get; set; }

        public double ObtenerSubtotal() { return Cantidad * PrecioUnitario; }
        public abstract string ObtenerDescripcion();
    }
}
