namespace SistemaExtintores.Modelos
{
    public class ExtintorItem : ItemVenta
    {
        private string _nroSerie;
        private string _tipoAgente;
        private double _capacidad;

        public string GetNroSerie() { return _nroSerie; }
        public void SetNroSerie(string nroSerie) { _nroSerie = nroSerie; }

        public string GetTipoAgente() { return _tipoAgente; }
        public void SetTipoAgente(string tipoAgente) { _tipoAgente = tipoAgente; }

        public double GetCapacidad() { return _capacidad; }
        public void SetCapacidad(double capacidad) { _capacidad = capacidad; }

        public override string ObtenerDescripcion()
        {
            return "Extintor " + _tipoAgente + " " + _capacidad + "kg - N° " + _nroSerie;
        }
    }
}