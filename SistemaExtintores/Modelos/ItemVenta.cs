namespace SistemaExtintores.Modelos
{
    public abstract class ItemVenta
    {
        private int _id;
        private int _cantidad;
        private double _precioUnitario;

        public int GetId() { return _id; }
        public void SetId(int id) { _id = id; }

        public int GetCantidad() { return _cantidad; }
        public void SetCantidad(int cantidad) { _cantidad = cantidad; }

        public double GetPrecioUnitario() { return _precioUnitario; }
        public void SetPrecioUnitario(double precio) { _precioUnitario = precio; }

        public double ObtenerSubtotal() { return _cantidad * _precioUnitario; }
        public abstract string ObtenerDescripcion();
    }
}