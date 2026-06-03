namespace SistemaExtintores.Modelos
{
    public class Recarga
    {
        private int _id;
        private DateTime _fecha;
        private string _agenteUsado;
        private string _tecnico;
        public string Observaciones { get; set; }

        public int GetId() { return _id; }
        public void SetId(int id) { _id = id; }

        public DateTime GetFecha() { return _fecha; }
        public void SetFecha(DateTime fecha) { _fecha = fecha; }

        public string GetAgenteUsado() { return _agenteUsado; }
        public void SetAgenteUsado(string agente) { _agenteUsado = agente; }

        public string GetTecnico() { return _tecnico; }
        public void SetTecnico(string tecnico) { _tecnico = tecnico; }
    }
}