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

  // BUSCADOR DE CLIENTES
  clientes: any[] = [];
  busquedaCliente = '';
  mostrarListaClientes = false;

  // USUARIOS (para mostrar cajera en historial)
  usuarios: any[] = [];

  // CATÁLOGOS PARA VENTA
  cilindrosDisponibles: any[] = [];
  extintoresDelCliente: any[] = [];
  insumos: any[] = [];
  cilindroSeleccionadoId = 0;
  extintorSeleccionadoId = 0;

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
    this.cargarClientes();
    this.cargarCilindrosDisponibles();
    this.cargarInsumos();
    this.cargarUsuarios();
  }

  cargarUsuarios() {
    this.api.get<any[]>('http://localhost:5057/Usuarios/lista-usuarios').subscribe({
      next: (data) => this.usuarios = data,
      error: () => {}
    });
  }

  nombreCajera(usuarioId: number): string {
    for (const u of this.usuarios) {
      if (u.id === usuarioId) return u.nombreCompleto || u.nombreUsuario;
    }
    return '#' + usuarioId;
  }

  cargarClientes() {
    this.api.get<any[]>('http://localhost:5057/Clientes/lista-clientes').subscribe({
      next: (data) => this.clientes = data,
      error: () => {}
    });
  }

  cargarCilindrosDisponibles() {
    this.api.get<any[]>('http://localhost:5057/Inventario/cilindros-disponibles').subscribe({
      next: (data) => this.cilindrosDisponibles = data,
      error: () => {}
    });
  }

  cargarInsumos() {
    this.api.get<any[]>('http://localhost:5057/Inventario/lista-insumos').subscribe({
      next: (data) => this.insumos = data.filter(i => !i.esPresurizante && i.stock > 0),
      error: () => {}
    });
  }

  buscarInsumoPorTipoAgente(tipoAgente: string): any {
    if (!tipoAgente) return null;
    const t = this.normalizar(tipoAgente);
    for (const i of this.insumos) {
      const n = this.normalizar(i.nombre);
      if (n.includes(t) || t.includes(n)) return i;
    }
    const palabras = t.split(/\s+/).filter(p => p.length > 1);
    for (const i of this.insumos) {
      const n = this.normalizar(i.nombre);
      for (const p of palabras) {
        if (n.includes(p)) return i;
      }
    }
    return null;
  }

  normalizar(s: string): string {
    return s.toLowerCase().normalize('NFD').replace(/[̀-ͯ]/g, '').trim();
  }

  cargarExtintoresDelCliente(clienteId: number) {
    this.api.get<any[]>(`http://localhost:5057/Extintores/por-cliente/${clienteId}`).subscribe({
      next: (data) => this.extintoresDelCliente = data,
      error: () => this.extintoresDelCliente = []
    });
  }

  get clientesFiltrados() {
    if (!this.busquedaCliente.trim()) return [];
    const f = this.busquedaCliente.toLowerCase();
    const resultado: any[] = [];
    for (const c of this.clientes) {
      const nombre = (c.nombreCompleto || c.razonSocial || '').toLowerCase();
      const id = (c.ci || c.nit || '').toLowerCase();
      if (nombre.includes(f) || id.includes(f)) resultado.push(c);
    }
    return resultado.slice(0, 6);
  }

  seleccionarCliente(cliente: any) {
    if (!this.ventaActual || !this.usuario) return;
    this.busquedaCliente = cliente.nombreCompleto || cliente.razonSocial || '';
    this.mostrarListaClientes = false;
    this.clienteActual = cliente;
    this.cargarExtintoresDelCliente(cliente.id);

    if (this.ventaActual.id === 0) {
      const nueva: any = {
        clienteId: cliente.id,
        sucursalId: this.ventaActual.sucursalId,
        usuarioId: this.usuario.id,
        descuento: 0
      };
      this.ventasService.crearVenta(nueva).subscribe({
        next: (data) => {
          if (data) this.ventaActual = data;
        },
        error: () => {
          this.mensaje = 'Error al crear la venta en la base de datos.';
        }
      });
    } else {
      this.ventaActual.clienteId = cliente.id;
    }
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
    this.ventaActual = {
      id: 0,
      fecha: new Date().toISOString(),
      clienteId: 0,
      sucursalId: undefined,
      usuarioId: this.usuario.id,
      descuento: 0,
      estado: EstadoVenta.Pendiente,
      numeroComprobante: '',
      items: []
    };
    this.busquedaCliente = '';
    this.clienteActual = null;
    this.mostrarCarrito = true;
    this.mostrarDetalle = false;
    this.mostrarComprobante = false;
    this.mensaje = '';
  }

  agregarExtintor() {
    if (!this.ventaActual) return;
    if (this.ventaActual.id === 0) {
      this.mensaje = 'Seleccione un cliente antes de agregar items.';
      return;
    }
    if (!this.cilindroSeleccionadoId) {
      this.mensaje = 'Seleccione un cilindro del inventario.';
      return;
    }
    const cilindro = this.cilindrosDisponibles.find(c => c.id == this.cilindroSeleccionadoId);
    if (!cilindro) {
      this.mensaje = 'Cilindro no encontrado.';
      return;
    }
    const item: any = {
      cantidad: 1,
      precioUnitario: this.nuevoItemExtintor.precioUnitario || cilindro.precioUnitario,
      nroSerie: cilindro.nroSerie,
      tipoAgente: cilindro.tipoAgente,
      capacidad: cilindro.capacidad
    };
    const clienteId = this.ventaActual.clienteId;
    this.ventasService.agregarItemExtintor(this.ventaActual.id, item).subscribe(data => {
      if (data) {
        this.ventaActual = data;
        this.ventaActual.clienteId = clienteId;
        this.mostrarFormExtintor = false;
        this.nuevoItemExtintor = {};
        this.cilindroSeleccionadoId = 0;
        this.cargarCilindrosDisponibles();
      } else {
        this.mensaje = 'Error al agregar extintor.';
      }
    });
  }

  agregarRecarga() {
    if (!this.ventaActual) return;
    if (this.ventaActual.id === 0) {
      this.mensaje = 'Seleccione un cliente antes de agregar items.';
      return;
    }
    if (!this.extintorSeleccionadoId) {
      this.mensaje = 'Seleccione un extintor del cliente para recargar.';
      return;
    }
    const extintor = this.extintoresDelCliente.find(e => e.id == this.extintorSeleccionadoId);
    if (!extintor) {
      this.mensaje = 'Extintor no encontrado.';
      return;
    }
    if (!this.nuevoItemRecarga.insumoId) {
      const insumoMatch = this.buscarInsumoPorTipoAgente(extintor.tipoAgente);
      if (insumoMatch) this.nuevoItemRecarga.insumoId = insumoMatch.id;
    }
    if (!this.nuevoItemRecarga.insumoId) {
      this.mensaje = 'No se encontró un insumo que coincida con "' + extintor.tipoAgente + '". Seleccione el insumo manualmente.';
      return;
    }
    const item: any = {
      cantidad: 1,
      precioUnitario: this.nuevoItemRecarga.precioUnitario || 0,
      nroSerieExtintor: extintor.nroSerie,
      insumoId: this.nuevoItemRecarga.insumoId
    };
    const clienteId = this.ventaActual.clienteId;
    this.ventasService.agregarItemRecarga(this.ventaActual.id, item).subscribe(data => {
      if (data) {
        this.ventaActual = data;
        this.ventaActual.clienteId = clienteId;
        this.mostrarFormRecarga = false;
        this.nuevoItemRecarga = {};
        this.extintorSeleccionadoId = 0;
      } else {
        this.mensaje = 'Error al agregar recarga.';
      }
    });
  }

  eliminarItem(itemId: number) {
    if (!this.ventaActual || this.ventaActual.id === 0) return;
    if (!confirm('¿Eliminar este item del carrito?')) return;
    const clienteId = this.ventaActual.clienteId;
    this.ventasService.eliminarItem(this.ventaActual.id, itemId).subscribe({
      next: (data) => {
        if (data) {
          this.ventaActual = data;
          this.ventaActual.clienteId = clienteId;
        }
      },
      error: () => {
        this.mensaje = 'Error al eliminar el item.';
      }
    });
  }

  confirmarDesdeHistorial(venta: Venta) {
    if (!this.usuario) return;
    if (!confirm('¿Confirmar esta venta?')) return;
    this.ventasService.confirmarVenta(venta.id, this.usuario.id, venta.clienteId).subscribe({
      next: (data) => {
        this.mensaje = 'Venta confirmada. Comprobante: ' + data.numeroComprobante;
        this.cargarVentas();
      },
      error: () => {
        this.mensaje = 'Error al confirmar. Verifique que la venta tenga items y N° de serie válidos.';
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
    if (!this.ventaActual.clienteId || this.ventaActual.clienteId === 0) {
      this.mensaje = 'Ingrese el ID del cliente antes de confirmar.';
      return;
    }
    if (this.ventaActual.items.length === 0) {
      this.mensaje = 'Agregue al menos un item antes de confirmar.';
      return;
    }
    if (!confirm('¿Confirmar la venta?')) return;
    this.ventasService.confirmarVenta(this.ventaActual.id, this.usuario.id, this.ventaActual.clienteId).subscribe({
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

  continuarVenta(venta: Venta) {
    this.ventaActual = venta;
    this.mostrarCarrito = true;
    this.mostrarDetalle = false;
    this.mostrarComprobante = false;
    this.mensaje = '';
    this.cargarExtintoresDelCliente(venta.clienteId);
    this.api.get<any>(`http://localhost:5057/Clientes/obtener-cliente/${venta.clienteId}`).subscribe({
      next: (c) => {
        if (c) {
          this.clienteActual = c;
          this.busquedaCliente = c.nombreCompleto || c.razonSocial || '';
        }
      },
      error: () => {}
    });
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