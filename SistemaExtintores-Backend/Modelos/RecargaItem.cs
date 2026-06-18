namespace SistemaExtintores.Modelos
{
    public class RecargaItem : ItemVenta
    {
        public string NroSerieExtintor { get; set; }

        public override string ObtenerDescripcion()
        {
            return "Recarga extintor N° " + NroSerieExtintor;
        }
    }
}
