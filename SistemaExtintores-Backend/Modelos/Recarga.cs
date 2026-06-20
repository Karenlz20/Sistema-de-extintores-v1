namespace SistemaExtintores.Modelos
{
    public class Recarga
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string AgenteUsado { get; set; }
        public double CantidadKg { get; set; }  //kg de agente a descontar del stock
        public int InsumoId { get; set; }        //id del InsumoInventario del agente
        public string Tecnico { get; set; }
        public string Observaciones { get; set; }

        public Recarga() { }
    }
}