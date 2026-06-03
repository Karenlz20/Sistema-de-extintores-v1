namespace SistemaExtintores.Modelos
{
    public class Extintor
    {
        private int _id;
        private string _nroSerie;
        private string _tipoAgente;
        private double _capacidad;
        private DateTime _fechaFabricacion;
        private DateTime _fechaUltimaCarga;
        private DateTime _fechaVencimiento;
        private int _clienteId;
        private int? _sucursalId; // Puede ser null, porque el cliente puede ser una persona
        private int? _cilindroInventarioId; //Puede ser null porque el cliente pudo haber llegado a hacer una recarga del extintor (osea que llegó con uno, no necesariamente se vendió aquí)
        private HojaDeVida _hojaDeVida;

        public Extintor() { }

        
        //constructor con setters para futuras validaciones
        public Extintor(int id, string nroSerie, string tipoAgente, double capacidad,
            DateTime fechaFabricacion, DateTime fechaUltimaCarga, DateTime fechaVencimiento,
            int clienteId, int? sucursalId, int? cilindroInventarioId)
        {
            SetId(id);
            SetNroSerie(nroSerie);
            SetTipoAgente(tipoAgente);
            SetCapacidad(capacidad);
            SetFechaFabricacion(fechaFabricacion);
            SetFechaUltimaCarga(fechaUltimaCarga);
            SetFechaVencimiento(fechaVencimiento);
            SetClienteId(clienteId);
            SetSucursalId(sucursalId);
            SetCilindroInventarioId(cilindroInventarioId);
        }

        public int GetId() { return _id; }
        public void SetId(int id) { _id = id; }

        public string GetNroSerie() { return _nroSerie; }
        public void SetNroSerie(string nroSerie) { _nroSerie = nroSerie; }

        public string GetTipoAgente() { return _tipoAgente; }
        public void SetTipoAgente(string tipoAgente) { _tipoAgente = tipoAgente; }

        public double GetCapacidad() { return _capacidad; }
        public void SetCapacidad(double capacidad) { _capacidad = capacidad; }

        public DateTime GetFechaFabricacion() { return _fechaFabricacion; }
        public void SetFechaFabricacion(DateTime fecha) { _fechaFabricacion = fecha; }

        public DateTime GetFechaUltimaCarga() { return _fechaUltimaCarga; }
        public void SetFechaUltimaCarga(DateTime fecha) { _fechaUltimaCarga = fecha; }

        public DateTime GetFechaVencimiento() { return _fechaVencimiento; }
        public void SetFechaVencimiento(DateTime fecha) { _fechaVencimiento = fecha; }

        public int GetClienteId() { return _clienteId; }
        public void SetClienteId(int clienteId) { _clienteId = clienteId; }

        public int? GetSucursalId() { return _sucursalId; }
        public void SetSucursalId(int? sucursalId) { _sucursalId = sucursalId; }

        public int? GetCilindroInventarioId() { return _cilindroInventarioId; }
        public void SetCilindroInventarioId(int? id) { _cilindroInventarioId = id; }

        public HojaDeVida GetHojaDeVida() { return _hojaDeVida; }
        public void SetHojaDeVida(HojaDeVida hojaDeVida) { _hojaDeVida = hojaDeVida; }

        public EstadoVigencia ObtenerEstado()
        {
            var diasRestantes = (_fechaVencimiento - DateTime.Now).Days;
            if (diasRestantes < 0)
                return EstadoVigencia.FueraDeNorma;
            else if (diasRestantes <= 30)
                return EstadoVigencia.ProximoAVencer;
            else
                return EstadoVigencia.Vigente;
        }
    }
}