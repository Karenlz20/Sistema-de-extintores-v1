namespace SistemaExtintores.Modelos
{
    public class InsumoInventario
    {
        private int _id;
        private string _nombre;
        private string _unidad;
        private double _stock;
        private double _puntoReorden;

        public InsumoInventario() { }

        //constructor con setters para futuras validaciones
        public InsumoInventario(int id, string nombre, string unidad, double stock, double puntoReorden)
        {
            SetId(id);
            SetNombre(nombre);
            SetUnidad(unidad);
            _stock = stock;
            SetPuntoReorden(puntoReorden);
        }

        public int GetId() { return _id; }
        public void SetId(int id) { _id = id; }

        public string GetNombre() { return _nombre; }
        public void SetNombre(string nombre) { _nombre = nombre; }

        public string GetUnidad() { return _unidad; }
        public void SetUnidad(string unidad) { _unidad = unidad; }

        public double GetStock() { return _stock; }
        public void ActualizarStock(double cantidad) { _stock += cantidad; }

        public double GetPuntoReorden() { return _puntoReorden; }
        public void SetPuntoReorden(double punto) { _puntoReorden = punto; }

        public bool RequiereReorden() { return _stock <= _puntoReorden; }
    }
}