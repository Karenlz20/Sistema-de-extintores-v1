namespace SistemaExtintores.Modelos
{
    public class InsumoInventario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Unidad { get; set; }
        public double Stock { get; set; }
        public double PuntoReorden { get; set; }
        public bool EsPresurizante { get; set; }

        public InsumoInventario() { }

        public InsumoInventario(int id, string nombre, string unidad, double stock, double puntoReorden, bool esPresurizante = false)
        {
            Id = id;
            Nombre = nombre;
            Unidad = unidad;
            Stock = stock;
            PuntoReorden = puntoReorden;
            EsPresurizante = esPresurizante;
        }

        public bool RequiereReorden() { return Stock <= PuntoReorden; }

        public bool IncrementarStock(double cantidad)
        {
            if (cantidad <= 0) return false;
            Stock += cantidad;
            return true;
        }

        public bool DisminuirStock(double cantidad)
        {
            if (cantidad <= 0) return false;
            if (cantidad > Stock) return false;
            Stock -= cantidad;
            return true;
        }
    }
}
