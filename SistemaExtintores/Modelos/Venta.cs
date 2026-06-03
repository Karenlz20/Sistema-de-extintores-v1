namespace SistemaExtintores.Modelos
{
    public class Venta
    {
        private int _id;
        private DateTime _fecha;
        private int _clienteId;
        private int? _sucursalId; //puede ser null porque el cliente puede ser una persona
        private string _cajera;
        private double _descuento = 0;
        private bool _confirmada = false;
        private List<ItemVenta> _items = new List<ItemVenta>();
        
        public Venta(int id, int clienteId, string cajera, double descuento, DateTime fecha)
        {
            _id = id;
            _clienteId = clienteId;
            _cajera = cajera;
            _descuento = descuento;
            _fecha = fecha;
        }
        
        public int GetId() { return _id; }
        public void SetId(int id) { _id = id; }

        public DateTime GetFecha() { return _fecha; }
        public void SetFecha(DateTime fecha) { _fecha = fecha; }

        public int GetClienteId() { return _clienteId; }
        public void SetClienteId(int clienteId) { _clienteId = clienteId; }

        public int? GetSucursalId() { return _sucursalId; }
        public void SetSucursalId(int? sucursalId) { _sucursalId = sucursalId; }

        public string GetCajera() { return _cajera; }
        public void SetCajera(string cajera) { _cajera = cajera; }

        public double GetDescuento() { return _descuento; }
        public void SetDescuento(double descuento) { _descuento = descuento; }

        public bool GetConfirmada() { return _confirmada; }
        public void Confirmar() { _confirmada = true; }

        public List<ItemVenta> GetItems() { return _items; }
        public void AgregarItem(ItemVenta item) { _items.Add(item); }

        public double ObtenerTotal()
        {
            double subtotal = 0;
            foreach (var item in _items)
            {
                subtotal += item.ObtenerSubtotal();
            }
            return subtotal - (subtotal * _descuento / 100);
        }

        public object ObtenerResumen()
        {
            return new
            {
                VentaId = _id,
                Fecha = _fecha,
                ClienteId = _clienteId,
                Cajera = _cajera,
                Confirmada = _confirmada,
                Total = ObtenerTotal()
            };
        }
    }
}