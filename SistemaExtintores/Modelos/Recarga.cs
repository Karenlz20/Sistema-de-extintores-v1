namespace SistemaExtintores.Modelos
{
    public class Recarga
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string AgenteUsado { get; set; }
        public string Tecnico { get; set; }
        public string Observaciones { get; set; }
    }
}
