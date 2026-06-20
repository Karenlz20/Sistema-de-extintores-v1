import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DatePipe } from '@angular/common';
import { ApiClient } from '../core/http/api-client';

@Component({
  selector: 'app-dashboard',
  imports: [DatePipe],
  templateUrl: './dashboard.html',
})
export class Dashboard implements OnInit {
  usuario: any = null;
  datos: any = null;
  clientesUrgentes: any[] = [];
  insumosReorden: any[] = [];

  constructor(private api: ApiClient, private router: Router) {}

  ngOnInit() {
    const guardado = localStorage.getItem('usuario');
    if (!guardado) {
      this.router.navigate(['/login']);
      return;
    }
    this.usuario = JSON.parse(guardado);

    this.api.get<any>('http://localhost:5057/dashboard/resumen').subscribe({
      next: (res) => this.datos = res,
      error: () => {}
    });

    this.api.get<any[]>('http://localhost:5057/dashboard/clientes-urgentes').subscribe({
      next: (res) => this.clientesUrgentes = res,
      error: () => {}
    });

    this.api.get<any[]>('http://localhost:5057/Inventario/insumos-bajo-reorden').subscribe({
      next: (res) => this.insumosReorden = res,
      error: () => {}
    });
  }

  cerrarSesion() {
    localStorage.removeItem('usuario');
    this.router.navigate(['/login']);
  }
}
