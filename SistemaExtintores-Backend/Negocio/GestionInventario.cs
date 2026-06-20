using SistemaExtintores.Data;
using SistemaExtintores.Modelos;

namespace SistemaExtintores.Negocio
{
    public class GestionInventario
    {
        private InventarioData _data;

        public GestionInventario()
        {
            _data = new InventarioData();
        }

        // CILINDROS
        public List<CilindroInventario> ObtenerCilindros()
        {
            return _data.ObtenerCilindros();
        }

        public List<CilindroInventario> ObtenerCilindrosDisponibles()
        {
            List<CilindroInventario> resultado = new List<CilindroInventario>();
            foreach (CilindroInventario c in _data.ObtenerCilindros())
            {
                if (c.Disponible) resultado.Add(c);
            }
            return resultado;
        }

        public List<CilindroInventario> ObtenerCilindrosVendidos()
        {
            List<CilindroInventario> resultado = new List<CilindroInventario>();
            foreach (CilindroInventario c in _data.ObtenerCilindros())
            {
                if (!c.Disponible) resultado.Add(c);
            }
            return resultado;
        }

        public CilindroInventario ObtenerCilindroPorId(int id)
        {
            return _data.ObtenerCilindroPorId(id);
        }

        public CilindroInventario ObtenerCilindroPorNroSerie(string nroSerie)
        {
            return _data.ObtenerCilindroPorNroSerie(nroSerie);
        }

        public CilindroInventario AgregarCilindro(CilindroInventario cilindro, int usuarioId)
        {
            CilindroInventario insertado = _data.InsertarCilindro(cilindro);
            _data.InsertarMovimiento(TipoMovimiento.Ingreso, TipoItemInventario.Cilindro,
                insertado.Id, 1, usuarioId, "Ingreso de cilindro");
            return insertado;
        }

        public bool MarcarVendido(int cilindroId, int usuarioId)
        {
            CilindroInventario cilindro = _data.ObtenerCilindroPorId(cilindroId);
            if (cilindro == null) return false;
            bool ok = _data.ActualizarDisponibilidad(cilindroId, false);
            if (ok) _data.InsertarMovimiento(TipoMovimiento.VentaCilindro, TipoItemInventario.Cilindro,
                cilindroId, 1, usuarioId, "Venta de cilindro");
            return ok;
        }

        public bool MarcarDisponible(int cilindroId)
        {
            CilindroInventario cilindro = _data.ObtenerCilindroPorId(cilindroId);
            if (cilindro == null) return false;
            return _data.ActualizarDisponibilidad(cilindroId, true);
        }

        public bool EliminarCilindro(int id)
        {
            return _data.EliminarCilindro(id);
        }

        // INSUMOS
        public List<InsumoInventario> ObtenerInsumos()
        {
            return _data.ObtenerInsumos();
        }

        public List<InsumoInventario> ObtenerInsumosBajoReorden()
        {
            List<InsumoInventario> resultado = new List<InsumoInventario>();
            foreach (InsumoInventario i in _data.ObtenerInsumos())
            {
                if (i.RequiereReorden()) resultado.Add(i);
            }
            return resultado;
        }

        public InsumoInventario ObtenerInsumoPorId(int id)
        {
            return _data.ObtenerInsumoPorId(id);
        }

        public InsumoInventario ObtenerInsumoPresurizante()
        {
            return _data.ObtenerInsumoPresurizante();
        }

        public InsumoInventario AgregarInsumo(InsumoInventario insumo, int usuarioId)
        {
            InsumoInventario insertado = _data.InsertarInsumo(insumo);
            _data.InsertarMovimiento(TipoMovimiento.Ingreso, TipoItemInventario.Insumo,
                insertado.Id, insertado.Stock, usuarioId, "Ingreso de insumo");
            return insertado;
        }

        public InsumoInventario ActualizarInsumo(int id, InsumoInventario insumoActualizado)
        {
            InsumoInventario existente = ObtenerInsumoPorId(id);
            if (existente == null) return null;
            existente.Nombre       = insumoActualizado.Nombre;
            existente.Unidad       = insumoActualizado.Unidad;
            existente.PuntoReorden = insumoActualizado.PuntoReorden;
            return existente;
        }

        public bool EliminarInsumo(int id)
        {
            return _data.EliminarInsumo(id);
        }

        public bool RegistrarIngresoInsumo(int insumoId, double cantidad, int usuarioId)
        {
            InsumoInventario insumo = _data.ObtenerInsumoPorId(insumoId);
            if (insumo == null) return false;
            double nuevoStock = insumo.Stock + cantidad;
            bool ok = _data.ActualizarStock(insumoId, nuevoStock);
            if (ok) _data.InsertarMovimiento(TipoMovimiento.Ingreso, TipoItemInventario.Insumo,
                insumoId, cantidad, usuarioId, "Ingreso de stock");
            return ok;
        }

        public bool RegistrarAjusteInsumo(int insumoId, double cantidad, int usuarioId, string observacion)
        {
            GestionUsuarios gestionUsuarios = new GestionUsuarios();
            Usuario usuario = gestionUsuarios.ObtenerPorId(usuarioId);
            if (usuario == null) return false;
            if (usuario.Rol != RolUsuario.Administrador) return false;

            InsumoInventario insumo = _data.ObtenerInsumoPorId(insumoId);
            if (insumo == null) return false;

            double nuevoStock = insumo.Stock + cantidad;
            if (nuevoStock < 0) return false;

            bool ok = _data.ActualizarStock(insumoId, nuevoStock);
            if (ok) _data.InsertarMovimiento(TipoMovimiento.Ajuste, TipoItemInventario.Insumo,
                insumoId, cantidad, usuarioId, observacion);
            return ok;
        }

        public bool DescontarInsumo(int insumoId, double cantidad, int usuarioId)
        {
            InsumoInventario insumo = _data.ObtenerInsumoPorId(insumoId);
            if (insumo == null) return false;
            if (insumo.Stock < cantidad) return false;

            double nuevoStock = insumo.Stock - cantidad;
            bool ok = _data.ActualizarStock(insumoId, nuevoStock);
            if (ok) _data.InsertarMovimiento(TipoMovimiento.ConsumoRecarga, TipoItemInventario.Insumo,
                insumoId, cantidad, usuarioId, "Consumo por recarga");
            return ok;
        }

        // MOVIMIENTOS
        public List<MovimientoInventario> ObtenerMovimientos()
        {
            return _data.ObtenerMovimientos();
        }

        public MovimientoInventario RegistrarMovimiento(TipoMovimiento tipo, TipoItemInventario tipoItem,
            int itemId, double cantidad, int usuarioId, string observacion)
        {
            return _data.InsertarMovimiento(tipo, tipoItem, itemId, cantidad, usuarioId, observacion);
        }
    }
}
