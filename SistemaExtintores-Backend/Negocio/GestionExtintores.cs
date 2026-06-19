using System;
using System.Collections.Generic;
using SistemaExtintores.Modelos;

namespace SistemaExtintores.Negocio
{
    public class GestionExtintores
    {
        private static List<Extintor> _extintores = new List<Extintor>();
        private static int _contadorId = 1;
        private static int _contadorHojaVida = 1;
        private static int _contadorRecarga = 1;

        private GestionInventario _gestionInventario;

        public GestionExtintores()
        {
            _gestionInventario = new GestionInventario();
        }

        public List<Extintor> ObtenerTodos()
        {
            return _extintores;
        }

        public Extintor ObtenerPorId(int id)
        {
            foreach (Extintor extintor in _extintores)
            {
                if (extintor.Id == id)
                    return extintor;
            }
            return null;
        }

        public Extintor ObtenerPorNroSerie(string nroSerie)
        {
            foreach (Extintor extintor in _extintores)
            {
                if (extintor.NroSerie == nroSerie)
                    return extintor;
            }
            return null;
        }

        public List<Extintor> ObtenerPorCliente(int clienteId)
        {
            List<Extintor> resultado = new List<Extintor>();
            foreach (Extintor extintor in _extintores)
            {
                if (extintor.ClienteId == clienteId)
                    resultado.Add(extintor);
            }
            return resultado;
        }

        public List<Extintor> ObtenerPorEstado(EstadoVigencia estado)
        {
            List<Extintor> resultado = new List<Extintor>();
            foreach (Extintor extintor in _extintores)
            {
                if (extintor.ObtenerEstado() == estado)
                    resultado.Add(extintor);
            }
            return resultado;
        }

        public Extintor Agregar(Extintor extintor)
        {
            // HU-13: N° de serie debe ser único
            Extintor existente = ObtenerPorNroSerie(extintor.NroSerie);
            if (existente != null) return null;

            extintor.Id = _contadorId;
            _contadorId++;

            HojaDeVida hoja = new HojaDeVida();
            hoja.Id = _contadorHojaVida;
            _contadorHojaVida++;
            hoja.FechaRegistroInicial = DateTime.Now;
            extintor.HojaDeVida = hoja;

            _extintores.Add(extintor);
            return extintor;
        }

        public Extintor Actualizar(int id, Extintor extintorActualizado)
        {
            Extintor existente = ObtenerPorId(id);
            if (existente == null) return null;

            existente.TipoAgente = extintorActualizado.TipoAgente;
            existente.Capacidad = extintorActualizado.Capacidad;
            existente.FechaFabricacion = extintorActualizado.FechaFabricacion;
            existente.FechaUltimaCarga = extintorActualizado.FechaUltimaCarga;
            existente.FechaVencimiento = extintorActualizado.FechaVencimiento;
            existente.ClienteId = extintorActualizado.ClienteId;
            existente.SucursalId = extintorActualizado.SucursalId;

            return existente;
        }

        public bool Eliminar(int id)
        {
            Extintor extintor = ObtenerPorId(id);
            if (extintor == null) return false;
            _extintores.Remove(extintor);
            return true;
        }

        public Extintor AgregarRecarga(int extintorId, Recarga recarga, int usuarioId)
        {
            Extintor extintor = ObtenerPorId(extintorId);
            if (extintor == null) return null;

            bool descontado = _gestionInventario.DescontarInsumo(recarga.InsumoId, recarga.CantidadKg, usuarioId);
            if (!descontado) return null;

            recarga.Id = _contadorRecarga;
            _contadorRecarga++;
            recarga.Fecha = DateTime.Now;

            extintor.HojaDeVida.AgregarRecarga(recarga);
            extintor.FechaUltimaCarga = recarga.Fecha;
            extintor.FechaVencimiento = recarga.Fecha.AddYears(1);

            return extintor;
        }
    }
}