namespace SistemaExtintores.Modelos
{
    public class CilindroInventario
    {
        public int Id { get; set; }
        public string NroSerie { get; set; }
        public string TipoAgente { get; set; }
        public double Capacidad { get; set; }
        public double PrecioUnitario { get; set; }
        public bool Disponible { get; set; } = true;

        public CilindroInventario() { }

        public CilindroInventario(int id, string nroSerie, string tipoAgente, double capacidad, double precioUnitario, bool disponible)
        {
            Id = id;
            NroSerie = nroSerie;
            TipoAgente = tipoAgente;
            Capacidad = capacidad;
            PrecioUnitario = precioUnitario;
            Disponible = disponible;
        }

        public void MarcarVendido() { Disponible = false; }
        public void MarcarDisponible() { Disponible = true; }
    }
}
