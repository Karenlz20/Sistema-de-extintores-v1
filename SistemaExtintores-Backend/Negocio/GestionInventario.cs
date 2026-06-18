using SistemaExtintores.Modelos;

namespace SistemaExtintores.Negocio
{
    public class GestionInventario
    {
        private static List<CilindroInventario> _cilindros = new List<CilindroInventario>
        {
            new CilindroInventario(1, "CIL-001", "PQS", 6, 150.0, true),
            new CilindroInventario(2, "CIL-002", "PQS", 6, 150.0, true),
            new CilindroInventario(3, "CIL-003", "CO2", 10, 280.0, true),
            new CilindroInventario(4, "CIL-004", "CO2", 10, 280.0, false)
        };
        private static int _contadorCilindroId = 5;

        private static List<InsumoInventario> _insumos = new List<InsumoInventario>
        {
            new InsumoInventario(1, "Agente PQS", "kg", 50, 10),
            new InsumoInventario(2, "Agente CO2", "kg", 8, 10),
            new InsumoInventario(3, "Válvula estándar", "unidad", 30, 5),
            new InsumoInventario(4, "Manguera", "unidad", 20, 5),
            new InsumoInventario(5, "Precinto", "unidad", 4, 5)
        };
        private static int _contadorInsumoId = 6;

        // ── Cilindros ──

        public List<CilindroInventario> ObtenerCilindros() { return _cilindros; }

        public List<CilindroInventario> ObtenerCilindrosDisponibles()
        {
            List<CilindroInventario> resultado = new List<CilindroInventario>();
            foreach (CilindroInventario cilindro in _cilindros)
            {
                if (cilindro.Disponible)
                    resultado.Add(cilindro);
            }
            return resultado;
        }

        public List<CilindroInventario> ObtenerCilindrosVendidos()
        {
            List<CilindroInventario> resultado = new List<CilindroInventario>();
            foreach (CilindroInventario cilindro in _cilindros)
            {
                if (!cilindro.Disponible)
                    resultado.Add(cilindro);
            }
            return resultado;
        }

        public CilindroInventario ObtenerCilindroPorId(int id)
        {
            foreach (CilindroInventario cilindro in _cilindros)
            {
                if (cilindro.Id == id)
                    return cilindro;
            }
            return null;
        }

        public CilindroInventario ObtenerCilindroPorNroSerie(string nroSerie)
        {
            foreach (CilindroInventario cilindro in _cilindros)
            {
                if (cilindro.NroSerie == nroSerie)
                    return cilindro;
            }
            return null;
        }

        public CilindroInventario AgregarCilindro(CilindroInventario cilindro)
        {
            cilindro.Id = _contadorCilindroId++;
            cilindro.MarcarDisponible();
            _cilindros.Add(cilindro);
            return cilindro;
        }

        public CilindroInventario MarcarVendido(int id)
        {
            CilindroInventario cilindro = ObtenerCilindroPorId(id);
            if (cilindro == null || !cilindro.Disponible) return null;

            cilindro.MarcarVendido();
            return cilindro;
        }

        public bool EliminarCilindro(int id)
        {
            CilindroInventario cilindro = ObtenerCilindroPorId(id);
            if (cilindro == null) return false;

            _cilindros.Remove(cilindro);
            return true;
        }

        // ── Insumos ──

        public List<InsumoInventario> ObtenerInsumos() { return _insumos; }

        public List<InsumoInventario> ObtenerInsumosBajoReorden()
        {
            List<InsumoInventario> resultado = new List<InsumoInventario>();
            foreach (InsumoInventario insumo in _insumos)
            {
                if (insumo.RequiereReorden())
                    resultado.Add(insumo);
            }
            return resultado;
        }

        public InsumoInventario ObtenerInsumoPorId(int id)
        {
            foreach (InsumoInventario insumo in _insumos)
            {
                if (insumo.Id == id)
                    return insumo;
            }
            return null;
        }

        public InsumoInventario AgregarInsumo(InsumoInventario insumo)
        {
            insumo.Id = _contadorInsumoId++;
            _insumos.Add(insumo);
            return insumo;
        }

        public InsumoInventario ActualizarStockInsumo(int id, double cantidad)
        {
            InsumoInventario insumo = ObtenerInsumoPorId(id);
            if (insumo == null) return null;

            insumo.ActualizarStock(cantidad);
            return insumo;
        }

        public InsumoInventario ActualizarInsumo(int id, InsumoInventario insumoActualizado)
        {
            InsumoInventario existente = ObtenerInsumoPorId(id);
            if (existente == null) return null;

            existente.Nombre = insumoActualizado.Nombre;
            existente.Unidad = insumoActualizado.Unidad;
            existente.PuntoReorden = insumoActualizado.PuntoReorden;

            return existente;
        }

        public bool EliminarInsumo(int id)
        {
            InsumoInventario insumo = ObtenerInsumoPorId(id);
            if (insumo == null) return false;

            _insumos.Remove(insumo);
            return true;
        }
    }
}
