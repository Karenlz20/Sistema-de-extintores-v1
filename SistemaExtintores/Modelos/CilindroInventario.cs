namespace SistemaExtintores.Modelos
{
    public class CilindroInventario
    {
        private int _id;
        private string _nroSerie;
        private string _tipoAgente;
        private double _capacidad;
        private double _precioUnitario;
        private bool _disponible = true;

        //constructor vacío
        public CilindroInventario() { }
        
        //constructor con setters para futuras validaciones
        public CilindroInventario(int id, string nroSerie, string tipoAgente, double capacidad, double precioUnitario, bool disponible)
        {
            SetId(id);
            SetNroSerie(nroSerie);
            SetTipoAgente(tipoAgente);
            SetCapacidad(capacidad);
            SetPrecioUnitario(precioUnitario);
            if (!disponible) MarcarVendido();
        }

        public int GetId() { return _id; }
        public void SetId(int id) { _id = id; }

        public string GetNroSerie() { return _nroSerie; }
        public void SetNroSerie(string nroSerie) { _nroSerie = nroSerie; }

        public string GetTipoAgente() { return _tipoAgente; }
        public void SetTipoAgente(string tipoAgente) { _tipoAgente = tipoAgente; }

        public double GetCapacidad() { return _capacidad; }
        public void SetCapacidad(double capacidad) { _capacidad = capacidad; }

        public double GetPrecioUnitario() { return _precioUnitario; }
        public void SetPrecioUnitario(double precio) { _precioUnitario = precio; }

        public bool GetDisponible() { return _disponible; }
        public void MarcarVendido() { _disponible = false; }
        public void MarcarDisponible() { _disponible = true; }
    }
}