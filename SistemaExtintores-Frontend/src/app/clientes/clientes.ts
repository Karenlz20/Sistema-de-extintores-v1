import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ApiClient } from '../core/http/api-client';

const BASE = 'http://localhost:5057/Clientes';

@Component({
  selector: 'app-clientes',
  imports: [FormsModule],
  templateUrl: './clientes.html',
})
export class Clientes implements OnInit {
  lista: any[] = [];
  error = '';
  exito = '';

  mostrarFormulario = false;
  modoEdicion = false;
  tipoSeleccionado = 'Persona';

  formPersona: any = { nombreCompleto: '', ci: '', telefono: '', correo: '', direccion: '' };
  formEmpresa: any = { razonSocial: '', nit: '', nombreContacto: '', telefono: '', correo: '', direccion: '' };
  formEdicion: any = { id: 0, telefono: '', correo: '', direccion: '' };

  clienteSeleccionado: any = null;
  mostrarSucursales = false;
  formSucursal: any = { nombre: '', direccion: '', telefono: '' };
  sucursalEditando: any = null;

  constructor(private api: ApiClient, private router: Router) {}

  verPerfil(id: number) { this.router.navigate(['/clientes', id]); }

  ngOnInit() {
    const guardado = localStorage.getItem('usuario');
    if (!guardado) { this.router.navigate(['/login']); return; }
    this.cargarLista();
  }

  cargarLista() {
    this.api.get<any[]>(`${BASE}/lista-clientes`).subscribe({
      next: (res) => this.lista = res,
      error: () => this.error = 'Error al cargar clientes.'
    });
  }

  esEmpresa(c: any) {
    return c.nit !== undefined;
  }

  abrirNuevo() {
    this.formPersona = { nombreCompleto: '', ci: '', telefono: '', correo: '', direccion: '' };
    this.formEmpresa = { razonSocial: '', nit: '', nombreContacto: '', telefono: '', correo: '', direccion: '' };
    this.tipoSeleccionado = 'Persona';
    this.modoEdicion = false;
    this.mostrarFormulario = true;
    this.error = '';
    this.exito = '';
  }

  abrirEdicion(c: any) {
    this.formEdicion = { id: c.id, telefono: c.telefono, correo: c.correo, direccion: c.direccion };
    this.modoEdicion = true;
    this.mostrarFormulario = true;
    this.error = '';
    this.exito = '';
  }

  cancelar() {
    this.mostrarFormulario = false;
    this.mostrarSucursales = false;
    this.clienteSeleccionado = null;
  }

  guardar() {
    this.error = '';
    this.exito = '';
    if (this.modoEdicion) {
      this.api.put<any>(`${BASE}/actualizar-cliente/${this.formEdicion.id}`, this.formEdicion).subscribe({
        next: () => { this.exito = 'Cliente actualizado.'; this.mostrarFormulario = false; this.cargarLista(); },
        error: () => this.error = 'Error al actualizar.'
      });
    } else if (this.tipoSeleccionado === 'Persona') {
      this.api.post<any>(`${BASE}/agregar-persona`, this.formPersona).subscribe({
        next: () => { this.exito = 'Cliente registrado.'; this.mostrarFormulario = false; this.cargarLista(); },
        error: () => this.error = 'Error al registrar.'
      });
    } else {
      this.api.post<any>(`${BASE}/agregar-empresa`, this.formEmpresa).subscribe({
        next: () => { this.exito = 'Empresa registrada.'; this.mostrarFormulario = false; this.cargarLista(); },
        error: () => this.error = 'Error al registrar.'
      });
    }
  }

  darDeBaja(id: number) {
    this.error = '';
    this.exito = '';
    this.mostrarFormulario = false;
    this.api.delete<any>(`${BASE}/eliminar-cliente/${id}`).subscribe({
      next: () => { this.exito = 'Cliente dado de baja correctamente.'; this.cargarLista(); },
      error: (err) => {
        const msg = err?.error?.message || err?.error || '';
        this.error = typeof msg === 'string' && msg.length > 0
          ? msg
          : 'No se puede dar de baja. El cliente tiene extintores activos.';
      }
    });
  }

  // ── Sucursales ──

  abrirSucursales(c: any) {
    this.clienteSeleccionado = c;
    this.mostrarSucursales = true;
    this.mostrarFormulario = false;
    this.formSucursal = { nombre: '', direccion: '', telefono: '' };
    this.sucursalEditando = null;
    this.error = '';
    this.exito = '';
  }

  editarSucursal(s: any) {
    this.sucursalEditando = s;
    this.formSucursal = { nombre: s.nombre, direccion: s.direccion, telefono: s.telefono };
  }

  guardarSucursal() {
    this.error = '';
    this.exito = '';
    if (this.sucursalEditando) {
      this.api.put<any>(`${BASE}/actualizar-sucursal/${this.sucursalEditando.id}`, this.formSucursal).subscribe({
        next: () => { this.exito = 'Sucursal actualizada.'; this.sucursalEditando = null; this.recargarCliente(); },
        error: () => this.error = 'Error al actualizar sucursal.'
      });
    } else {
      this.api.post<any>(`${BASE}/agregar-sucursal/${this.clienteSeleccionado.id}`, this.formSucursal).subscribe({
        next: () => { this.exito = 'Sucursal agregada.'; this.formSucursal = { nombre: '', direccion: '', telefono: '' }; this.recargarCliente(); },
        error: () => this.error = 'Error al agregar sucursal.'
      });
    }
  }

  bajaSucursal(sucursalId: number) {
    this.error = '';
    this.api.delete<any>(`${BASE}/baja-sucursal/${sucursalId}`).subscribe({
      next: () => { this.exito = 'Sucursal dada de baja.'; this.recargarCliente(); },
      error: () => this.error = 'No se puede dar de baja: debe quedar al menos una sucursal activa.'
    });
  }

  recargarCliente() {
    this.api.get<any>(`${BASE}/obtener-cliente/${this.clienteSeleccionado.id}`).subscribe({
      next: (c) => { this.clienteSeleccionado = c; this.cargarLista(); }
    });
  }
}
