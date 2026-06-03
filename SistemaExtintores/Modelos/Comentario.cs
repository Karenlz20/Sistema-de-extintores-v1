namespace SistemaExtintores.Modelos
{
    public class Comentario
    {
        private int _id;
        private string _texto;
        private DateTime _fechaCreacion;
        private string _autor;
        private int _extintorId;

        public Comentario() { }

        public Comentario(int id, string texto, string autor, DateTime fechaCreacion, int extintorId)
        {
            SetId(id);
            SetTexto(texto);
            SetAutor(autor);
            SetFechaCreacion(fechaCreacion);
            SetExtintorId(extintorId);
        }

        public int GetId() { return _id; }
        public void SetId(int id) { _id = id; }

        public string GetTexto() { return _texto; }
        public void SetTexto(string texto) { _texto = texto; }

        public DateTime GetFechaCreacion() { return _fechaCreacion; }
        public void SetFechaCreacion(DateTime fecha) { _fechaCreacion = fecha; }

        public string GetAutor() { return _autor; }
        public void SetAutor(string autor) { _autor = autor; }

        public int GetExtintorId() { return _extintorId; }
        public void SetExtintorId(int extintorId) { _extintorId = extintorId; }
    }
}