import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ApiClient } from '../core/http/api-client';

@Component({
  selector: 'app-dashboard',
  imports: [],
  templateUrl: './dashboard.html',
})
export class Dashboard implements OnInit {
  usuario: any = null;
  datos: any = null;

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
  }

  cerrarSesion() {
    localStorage.removeItem('usuario');
    this.router.navigate(['/login']);
  }
}
