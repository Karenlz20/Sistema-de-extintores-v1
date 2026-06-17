namespace SistemaExtintores.Modelos
{
    public class Extintor
    {
        public int Id { get; set; }
        public string NroSerie { get; set; }
        public string TipoAgente { get; set; }
        public double Capacidad { get; set; }
        public DateTime FechaFabricacion { get; set; }
        public DateTime FechaUltimaCarga { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public int ClienteId { get; set; }
        public int? SucursalId { get; set; }
        public int? CilindroInventarioId { get; set; }
        public HojaDeVida HojaDeVida { get; set; }

        public Extintor() { }

        public Extintor(int id, string nroSerie, string tipoAgente, double capacidad,
            DateTime fechaFabricacion, DateTime fechaUltimaCarga, DateTime fechaVencimiento,
            int clienteId, int? sucursalId, int? cilindroInventarioId)
        {
            Id = id;
            NroSerie = nroSerie;
            TipoAgente = tipoAgente;
            Capacidad = capacidad;
            FechaFabricacion = fechaFabricacion;
            FechaUltimaCarga = fechaUltimaCarga;
            FechaVencimiento = fechaVencimiento;
            ClienteId = clienteId;
            SucursalId = sucursalId;
            CilindroInventarioId = cilindroInventarioId;
        }

        public EstadoVigencia ObtenerEstado()
        {
            int diasRestantes = (FechaVencimiento - DateTime.Now).Days;
            if (diasRestantes < 0)
                return EstadoVigencia.FueraDeNorma;
            else if (diasRestantes <= 30)
                return EstadoVigencia.ProximoAVencer;
            else
                return EstadoVigencia.Vigente;
        }
    }
}
