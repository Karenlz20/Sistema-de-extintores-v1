namespace SistemaExtintores.Modelos
{
    public class RecargaItem : ItemVenta
    {
        private string _nroSerieExtintor;

        public string GetNroSerieExtintor() { return _nroSerieExtintor; }
        public void SetNroSerieExtintor(string nroSerie) { _nroSerieExtintor = nroSerie; }

        public override string ObtenerDescripcion()
        {
            return "Recarga extintor N° " + _nroSerieExtintor;
        }
    }
}