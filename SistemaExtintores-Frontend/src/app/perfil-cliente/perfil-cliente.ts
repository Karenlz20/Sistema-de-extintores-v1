import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DatePipe } from '@angular/common';
import { ApiClient } from '../core/http/api-client';

@Component({
  selector: 'app-perfil-cliente',
  imports: [DatePipe],
  templateUrl: './perfil-cliente.html',
})
export class PerfilCliente implements OnInit {
  cliente: any = null;
  extintores: any[] = [];
  error = '';

  constructor(private api: ApiClient, private route: ActivatedRoute, private router: Router) {}

  ngOnInit() {
    const guardado = localStorage.getItem('usuario');
    if (!guardado) { this.router.navigate(['/login']); return; }

    const id = this.route.snapshot.paramMap.get('id');
    if (!id) { this.router.navigate(['/clientes']); return; }

    this.api.get<any>(`http://localhost:5057/Clientes/obtener-cliente/${id}`).subscribe({
      next: (res) => this.cliente = res,
      error: () => this.error = 'No se pudo cargar el cliente.'
    });

    this.api.get<any[]>(`http://localhost:5057/Extintores/por-cliente/${id}`).subscribe({
      next: (res) => this.extintores = res,
      error: () => this.extintores = []
    });
  }

  esEmpresa() { return this.cliente?.nit !== undefined; }

  obtenerEstadoBadge(estado: string) {
    if (estado === 'Vigente') return 'bg-success';
    if (estado === 'ProximoAVencer') return 'bg-warning text-dark';
    return 'bg-danger';
  }

  volver() { this.router.navigate(['/clientes']); }
}
