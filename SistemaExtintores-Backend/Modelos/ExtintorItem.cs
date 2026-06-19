namespace SistemaExtintores.Modelos
{
    public class ExtintorItem : ItemVenta
    {
        public string NroSerie { get; set; }

        public string TipoAgente { get; set; }

        public double Capacidad { get; set; }

        public ExtintorItem()
        {
        }

        public ExtintorItem(int id, int cantidad, double precioUnitario, string nroSerie, string tipoAgente, double capacidad) : base(id, cantidad, precioUnitario)
        {
            NroSerie = nroSerie;
            TipoAgente = tipoAgente;
            Capacidad = capacidad;
        }

        public override string ObtenerDescripcion()
        {
            return "Extintor " + TipoAgente + " " + Capacidad + " Kg";
        }
    }
}