namespace SistemaExtintores.Modelos
{
    public class InsumoInventario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Unidad { get; set; }
        public double Stock { get; set; }
        public double PuntoReorden { get; set; }

        public InsumoInventario() { }

        public InsumoInventario(int id, string nombre, string unidad, double stock, double puntoReorden)
        {
            Id = id;
            Nombre = nombre;
            Unidad = unidad;
            Stock = stock;
            PuntoReorden = puntoReorden;
        }

        public void ActualizarStock(double cantidad) { Stock += cantidad; }

        public bool RequiereReorden() { return Stock <= PuntoReorden; }
    }
}
