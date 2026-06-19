using System;
using System.Collections.Generic;
using SistemaExtintores.Modelos;

namespace SistemaExtintores.Negocio
{
    public class GestionInventario
    {
        private static List<CilindroInventario> _cilindros = new List<CilindroInventario>();
        private static List<InsumoInventario> _insumos = new List<InsumoInventario>();
        private static List<MovimientoInventario> _movimientos = new List<MovimientoInventario>();

        private static int _contadorCilindroId = 1;
        private static int _contadorInsumoId = 1;
        private static int _contadorMovimientoId = 1;

        public GestionInventario() { }

        // CILINDROS

        public List<CilindroInventario> ObtenerCilindros()
        {
            return _cilindros;
        }

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

        public CilindroInventario AgregarCilindro(CilindroInventario cilindro, int usuarioId)
        {
            cilindro.Id = _contadorCilindroId;
            _contadorCilindroId++;
            _cilindros.Add(cilindro);
            RegistrarMovimiento(TipoMovimiento.Ingreso, TipoItemInventario.Cilindro, cilindro.Id, 1, usuarioId, "Ingreso de cilindro");
            return cilindro;
        }

        public bool MarcarVendido(int cilindroId, int usuarioId)
        {
            CilindroInventario cilindro = ObtenerCilindroPorId(cilindroId);
            if (cilindro == null) return false;
            cilindro.MarcarVendido();
            RegistrarMovimiento(TipoMovimiento.VentaCilindro, TipoItemInventario.Cilindro, cilindro.Id, 1, usuarioId, "Venta de cilindro");
            return true;
        }

        public bool MarcarDisponible(int cilindroId)
        {
            CilindroInventario cilindro = ObtenerCilindroPorId(cilindroId);
            if (cilindro == null) return false;
            cilindro.MarcarDisponible();
            return true;
        }

        public bool EliminarCilindro(int id)
        {
            CilindroInventario cilindro = ObtenerCilindroPorId(id);
            if (cilindro == null) return false;
            _cilindros.Remove(cilindro);
            return true;
        }

        // INSUMOS

        public List<InsumoInventario> ObtenerInsumos()
        {
            return _insumos;
        }

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

        public InsumoInventario AgregarInsumo(InsumoInventario insumo, int usuarioId)
        {
            insumo.Id = _contadorInsumoId;
            _contadorInsumoId++;
            _insumos.Add(insumo);
            RegistrarMovimiento(TipoMovimiento.Ingreso, TipoItemInventario.Insumo, insumo.Id, insumo.Stock, usuarioId, "Ingreso de insumo");
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

        public bool RegistrarIngresoInsumo(int insumoId, double cantidad, int usuarioId)
        {
            InsumoInventario insumo = ObtenerInsumoPorId(insumoId);
            if (insumo == null) return false;
            insumo.IncrementarStock(cantidad);
            RegistrarMovimiento(TipoMovimiento.Ingreso, TipoItemInventario.Insumo, insumo.Id, cantidad, usuarioId, "Ingreso de stock");
            return true;
        }

        public bool RegistrarAjusteInsumo(int insumoId, double cantidad, int usuarioId, string observacion)
        {
            GestionUsuarios gestionUsuarios = new GestionUsuarios();
            Usuario usuario = gestionUsuarios.ObtenerPorId(usuarioId);
            if (usuario == null) return false;
            if (usuario.Rol != RolUsuario.Administrador) return false;

            InsumoInventario insumo = ObtenerInsumoPorId(insumoId);
            if (insumo == null) return false;
            if (cantidad > 0)
                insumo.IncrementarStock(cantidad);
            else
                insumo.DisminuirStock(Math.Abs(cantidad));
            RegistrarMovimiento(TipoMovimiento.Ajuste, TipoItemInventario.Insumo, insumo.Id, cantidad, usuarioId, observacion);
            return true;
        }

        public bool DescontarInsumo(int insumoId, double cantidad, int usuarioId)
        {
            InsumoInventario insumo = ObtenerInsumoPorId(insumoId);
            if (insumo == null) return false;
            if (insumo.Stock < cantidad) return false;
            insumo.DisminuirStock(cantidad);
            RegistrarMovimiento(TipoMovimiento.ConsumoRecarga, TipoItemInventario.Insumo, insumo.Id, cantidad, usuarioId, "Consumo por recarga");
            return true;
        }

        // MOVIMIENTOS

        public List<MovimientoInventario> ObtenerMovimientos()
        {
            return _movimientos;
        }

        public MovimientoInventario RegistrarMovimiento(TipoMovimiento tipo, TipoItemInventario tipoItem, int itemId, double cantidad, int usuarioId, string observacion)
        {
            MovimientoInventario movimiento = new MovimientoInventario(_contadorMovimientoId, tipo, itemId, tipoItem, cantidad, usuarioId, observacion);
            _contadorMovimientoId++;
            _movimientos.Add(movimiento);
            return movimiento;
        }
    }
}
