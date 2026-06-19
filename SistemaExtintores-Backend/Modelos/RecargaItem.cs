namespace SistemaExtintores.Modelos
{
    public class RecargaItem : ItemVenta
    {
        public string NroSerieExtintor { get; set; }
        public int InsumoId { get; set; }

        public RecargaItem() { }

        public RecargaItem(int id, int cantidad, double precioUnitario, string nroSerieExtintor, int insumoId) : base(id, cantidad, precioUnitario)
        {
            NroSerieExtintor = nroSerieExtintor;
            InsumoId = insumoId;
        }

        public override string ObtenerDescripcion() { return "Recarga del extintor " + NroSerieExtintor; }
    }
}
