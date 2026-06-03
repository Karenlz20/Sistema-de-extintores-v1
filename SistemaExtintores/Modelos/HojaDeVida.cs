namespace SistemaExtintores.Modelos
{
    public class HojaDeVida
    {
        private int _id;
        private DateTime _fechaRegistroInicial;
        private List<Recarga> _recargas = new List<Recarga>();

        public int GetId() { return _id; }
        public void SetId(int id) { _id = id; }

        public DateTime GetFechaRegistroInicial() { return _fechaRegistroInicial; }
        public void SetFechaRegistroInicial(DateTime fecha) { _fechaRegistroInicial = fecha; }

        public List<Recarga> GetRecargas() { return _recargas; }
        public void AgregarRecarga(Recarga recarga) { _recargas.Add(recarga); }
    }
}