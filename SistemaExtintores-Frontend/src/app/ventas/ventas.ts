import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { VentasService } from './ventas.service';
import { Venta, ExtintorItem, RecargaItem, EstadoVenta, Usuario, RolUsuario } from '../modelos/modelos';
import { Router } from '@angular/router';
import { ApiClient } from '../core/http/api-client';

@Component({
  selector: 'app-ventas',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './ventas.html'
})
export class Ventas implements OnInit {
  usuario: Usuario | null = null;
  esAdmin = false;

  // HISTORIAL
  ventas: Venta[] = [];
  filtroDesdeFecha = '';
  filtroHastaFecha = '';
  filtroClienteId = '';

  // VENTA ACTIVA
  ventaActual: Venta | null = null;
  mostrarCarrito = false;

  // NUEVO ITEM EXTINTOR
  mostrarFormExtintor = false;
  nuevoItemExtintor: any = {};

  // NUEVO ITEM RECARGA
  mostrarFormRecarga = false;
  nuevoItemRecarga: any = {};

  // DESCUENTO
  porcentajeDescuento = 0;

  // DETALLE
  ventaDetalle: Venta | null = null;
  mostrarDetalle = false;

  // COMPROBANTE
  mostrarComprobante = false;
  ventaComprobante: Venta | null = null;

  clienteActual: any = null;
  mensaje = '';

  constructor(private ventasService: VentasService, private router: Router, private api: ApiClient) {}

  ngOnInit() {
    const raw = localStorage.getItem('usuario');
    if (!raw) {
      this.router.navigate(['/login']);
      return;
    }
    this.usuario = JSON.parse(raw);
    this.esAdmin = this.usuario?.rol === RolUsuario.Administrador;
    this.cargarVentas();
  }

  cargarVentas() {
    this.ventasService.listaVentas().subscribe(data => {
      this.ventas = data;
    });
  }

  get ventasFiltradas() {
    const resultado: Venta[] = [];
    for (const v of this.ventas) {
      let coincide = true;
      if (this.filtroClienteId !== '' && v.clienteId !== Number(this.filtroClienteId)) {
        coincide = false;
      }
      if (this.filtroDesdeFecha !== '' && v.fecha < this.filtroDesdeFecha) {
        coincide = false;
      }
      if (this.filtroHastaFecha !== '' && v.fecha > this.filtroHastaFecha) {
        coincide = false;
      }
      if (coincide) {
        resultado.push(v);
      }
    }
    return resultado;
  }

  iniciarVenta() {
    if (!this.usuario) return;
    const nueva: any = {
      clienteId: 0,
      sucursalId: null,
      usuarioId: this.usuario.id,
      descuento: 0
    };
    this.ventasService.crearVenta(nueva).subscribe(data => {
      if (data) {
        this.ventaActual = data;
        this.mostrarCarrito = true;
        this.mostrarDetalle = false;
        this.mostrarComprobante = false;
        this.mensaje = '';
      }
    });
  }

  agregarExtintor() {
    if (!this.ventaActual) return;
    this.ventasService.agregarItemExtintor(this.ventaActual.id, this.nuevoItemExtintor).subscribe(data => {
      if (data) {
        this.ventaActual = data;
        this.mostrarFormExtintor = false;
        this.nuevoItemExtintor = {};
      } else {
        this.mensaje = 'Error al agregar extintor. Verifique el N° de serie.';
      }
    });
  }

  agregarRecarga() {
    if (!this.ventaActual) return;
    this.ventasService.agregarItemRecarga(this.ventaActual.id, this.nuevoItemRecarga).subscribe(data => {
      if (data) {
        this.ventaActual = data;
        this.mostrarFormRecarga = false;
        this.nuevoItemRecarga = {};
      } else {
        this.mensaje = 'Error al agregar recarga.';
      }
    });
  }

  aplicarDescuento() {
    if (!this.ventaActual || !this.usuario) return;
    this.ventasService.aplicarDescuento(this.ventaActual.id, this.usuario.id, this.porcentajeDescuento).subscribe(data => {
      if (data) {
        this.ventaActual = data;
        this.mensaje = 'Descuento aplicado.';
      } else {
        this.mensaje = 'Error al aplicar descuento. Solo el administrador puede aplicar más del 15%.';
      }
    });
  }

  confirmarVenta() {
    if (!this.ventaActual || !this.usuario) return;
    if (this.ventaActual.items.length === 0) {
      this.mensaje = 'Agregue al menos un item antes de confirmar.';
      return;
    }
    if (!confirm('¿Confirmar la venta?')) return;
    this.ventasService.confirmarVenta(this.ventaActual.id, this.usuario.id).subscribe({
      next: (data) => {
        this.ventaComprobante = data;
        this.mostrarCarrito = false;
        this.mostrarComprobante = true;
        this.cargarCliente(data.clienteId);
        this.ventaActual = null;
        this.cargarVentas();
      },
      error: () => {
        this.mensaje = 'Error al confirmar la venta. Verifique que los N° de serie no estén duplicados.';
      }
    });
  }

  anularVenta(id: number) {
    if (!confirm('¿Anular esta venta?')) return;
    this.ventasService.anularVenta(id).subscribe(data => {
      this.cargarVentas();
    });
  }

  cargarCliente(clienteId: number) {
    this.clienteActual = null;
    if (!clienteId) return;
    this.api.get<any>(`http://localhost:5057/Clientes/obtener-cliente/${clienteId}`).subscribe({
      next: (c) => this.clienteActual = c,
      error: () => {}
    });
  }

  nombreCliente() {
    if (!this.clienteActual) return '';
    return this.clienteActual.nombreCompleto || this.clienteActual.razonSocial || '';
  }

  identificacionCliente() {
    if (!this.clienteActual) return '';
    if (this.clienteActual.ci) return 'CI: ' + this.clienteActual.ci;
    if (this.clienteActual.nit) return 'NIT: ' + this.clienteActual.nit;
    return '';
  }

  verDetalle(venta: Venta) {
    this.ventaDetalle = venta;
    this.mostrarDetalle = true;
    this.mostrarCarrito = false;
    this.mostrarComprobante = false;
    this.cargarCliente(venta.clienteId);
  }

  verComprobante(venta: Venta) {
    this.ventaComprobante = venta;
    this.mostrarComprobante = true;
    this.mostrarDetalle = false;
    this.mostrarCarrito = false;
    this.cargarCliente(venta.clienteId);
  }

  imprimir() {
    window.print();
  }

  volver() {
    this.mostrarCarrito = false;
    this.mostrarDetalle = false;
    this.mostrarComprobante = false;
    this.ventaActual = null;
    this.ventaDetalle = null;
    this.ventaComprobante = null;
    this.clienteActual = null;
  }

  obtenerSubtotal(venta: Venta) {
    let subtotal = 0;
    for (const item of venta.items) {
      subtotal += item.cantidad * item.precioUnitario;
    }
    return subtotal;
  }

  obtenerDescuento(venta: Venta) {
    return this.obtenerSubtotal(venta) * (venta.descuento / 100);
  }

  obtenerIVA(venta: Venta) {
    return (this.obtenerSubtotal(venta) - this.obtenerDescuento(venta)) * 0.13;
  }

  obtenerTotal(venta: Venta) {
    return this.obtenerSubtotal(venta) - this.obtenerDescuento(venta) + this.obtenerIVA(venta);
  }

  obtenerTextoEstado(estado: EstadoVenta) {
    if (estado === EstadoVenta.Pendiente) return 'Pendiente';
    if (estado === EstadoVenta.Confirmada) return 'Confirmada';
    return 'Anulada';
  }

  obtenerClaseEstado(estado: EstadoVenta) {
    if (estado === EstadoVenta.Pendiente) return 'bg-warning';
    if (estado === EstadoVenta.Confirmada) return 'bg-success';
    return 'bg-danger';
  }
}