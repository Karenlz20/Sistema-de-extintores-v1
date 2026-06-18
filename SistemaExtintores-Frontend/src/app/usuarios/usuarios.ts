import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ApiClient } from '../core/http/api-client';

const BASE = 'http://localhost:5057/Usuarios';

@Component({
  selector: 'app-usuarios',
  imports: [FormsModule],
  templateUrl: './usuarios.html',
})
export class Usuarios implements OnInit {
  lista: any[] = [];
  usuarioActual: any = null;
  error = '';
  exito = '';

  mostrarFormulario = false;
  modoEdicion = false;

  form: any = { id: 0, nombreUsuario: '', nombreCompleto: '', password: '', rol: 'Cajera', activo: true };

  constructor(private api: ApiClient, private router: Router) {}

  ngOnInit() {
    const guardado = localStorage.getItem('usuario');
    if (!guardado) { this.router.navigate(['/login']); return; }
    this.usuarioActual = JSON.parse(guardado);
    if (this.usuarioActual.rol !== 'Administrador') { this.router.navigate(['/dashboard']); return; }
    this.cargarLista();
  }

  cargarLista() {
    this.api.get<any[]>(`${BASE}/lista-usuarios`).subscribe({
      next: (res) => this.lista = res,
      error: () => this.error = 'Error al cargar usuarios.'
    });
  }

  abrirNuevo() {
    this.form = { id: 0, nombreUsuario: '', nombreCompleto: '', password: '', rol: 'Cajera', activo: true };
    this.modoEdicion = false;
    this.mostrarFormulario = true;
    this.error = '';
    this.exito = '';
  }

  abrirEdicion(u: any) {
    this.form = { ...u, password: '' };
    this.modoEdicion = true;
    this.mostrarFormulario = true;
    this.error = '';
    this.exito = '';
  }

  cancelar() {
    this.mostrarFormulario = false;
  }

  guardar() {
    this.error = '';
    this.exito = '';
    if (this.modoEdicion) {
      this.api.put<any>(`${BASE}/actualizar-usuario/${this.form.id}`, this.form).subscribe({
        next: () => { this.exito = 'Usuario actualizado.'; this.mostrarFormulario = false; this.cargarLista(); },
        error: () => this.error = 'Error al actualizar.'
      });
    } else {
      this.api.post<any>(`${BASE}/agregar-usuario`, this.form).subscribe({
        next: () => { this.exito = 'Usuario creado.'; this.mostrarFormulario = false; this.cargarLista(); },
        error: () => this.error = 'Error al crear usuario.'
      });
    }
  }

  darDeBaja(id: number) {
    this.error = '';
    this.exito = '';
    this.api.delete<any>(`${BASE}/eliminar-usuario/${id}?usuarioActualId=${this.usuarioActual.id}`).subscribe({
      next: () => { this.exito = 'Usuario desactivado.'; this.cargarLista(); },
      error: () => this.error = 'No se pudo desactivar el usuario.'
    });
  }
}
