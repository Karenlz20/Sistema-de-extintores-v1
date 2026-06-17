namespace SistemaExtintores.Modelos
{
    public class Venta
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public int ClienteId { get; set; }
        public int? SucursalId { get; set; }
        public string Cajera { get; set; }
        public double Descuento { get; set; } = 0;
        public bool Confirmada { get; set; } = false;
        public List<ItemVenta> Items { get; set; } = new List<ItemVenta>();

        public Venta() { }

        public Venta(int id, int clienteId, string cajera, double descuento, DateTime fecha)
        {
            Id = id;
            ClienteId = clienteId;
            Cajera = cajera;
            Descuento = descuento;
            Fecha = fecha;
        }

        public void Confirmar() { Confirmada = true; }
        public void AgregarItem(ItemVenta item) { Items.Add(item); }

        public double ObtenerTotal()
        {
            double subtotal = 0;
            foreach (ItemVenta item in Items)
            {
                subtotal += item.ObtenerSubtotal();
            }
            return subtotal - (subtotal * Descuento / 100);
        }

        public object ObtenerResumen()
        {
            return new
            {
                VentaId = Id,
                Fecha = Fecha,
                ClienteId = ClienteId,
                Cajera = Cajera,
                Confirmada = Confirmada,
                Total = ObtenerTotal()
            };
        }
    }
}
