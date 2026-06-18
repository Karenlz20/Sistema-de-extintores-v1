namespace SistemaExtintores.Modelos
{
    public class ExtintorItem : ItemVenta
    {
        public string NroSerie { get; set; }
        public string TipoAgente { get; set; }
        public double Capacidad { get; set; }

        public override string ObtenerDescripcion()
        {
            return "Extintor " + TipoAgente + " " + Capacidad + "kg - N° " + NroSerie;
        }
    }
}
