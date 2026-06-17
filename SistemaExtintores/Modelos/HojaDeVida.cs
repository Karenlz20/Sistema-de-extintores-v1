namespace SistemaExtintores.Modelos
{
    public class HojaDeVida
    {
        public int Id { get; set; }
        public DateTime FechaRegistroInicial { get; set; }
        public List<Recarga> Recargas { get; set; } = new List<Recarga>();

        public void AgregarRecarga(Recarga recarga) { Recargas.Add(recarga); }
    }
}
