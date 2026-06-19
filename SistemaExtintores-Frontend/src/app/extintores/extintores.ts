import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ExtintoresService } from './extintores.service';
import { Extintor, Recarga, EstadoVigencia, Usuario, RolUsuario } from '../modelos/modelos';
import { Router } from '@angular/router';

@Component({
  selector: 'app-extintores',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './extintores.html'
})
export class Extintores implements OnInit {
  usuario: Usuario | null = null;
  esAdmin = false;

  extintores: Extintor[] = [];
  filtroSerie = '';
  filtroEstado = '';

  // FORMULARIO NUEVO EXTINTOR
  mostrarFormExtintor = false;
  nuevoExtintor: any = {};

  // HOJA DE VIDA
  extintorSeleccionado: Extintor | null = null;
  mostrarHojaDeVida = false;

  // FORMULARIO RECARGA
  mostrarFormRecarga = false;
  nuevaRecarga: any = {};

  mensaje = '';

  constructor(private extintoresService: ExtintoresService, private router: Router) {}

  ngOnInit() {
    const raw = localStorage.getItem('usuario');
    if (!raw) {
      this.router.navigate(['/login']);
      return;
    }
    this.usuario = JSON.parse(raw);
    this.esAdmin = this.usuario?.rol === RolUsuario.Administrador;
    this.cargarExtintores();
  }

  cargarExtintores() {
    this.extintoresService.listaExtintores().subscribe(data => {
      this.extintores = data;
    });
  }

  get extintoresFiltrados() {
    const serie = this.filtroSerie.toLowerCase();
    const resultado: Extintor[] = [];
    for (const e of this.extintores) {
      const coincideSerie = e.nroSerie.toLowerCase().includes(serie);
      const coincideEstado = this.filtroEstado === '' || e.estado === this.filtroEstado;
      if (coincideSerie && coincideEstado) {
        resultado.push(e);
      }
    }
    return resultado;
  }

  obtenerClaseEstado(estado: string) {
    if (estado === 'Vigente') return 'bg-success';
    if (estado === 'ProximoAVencer') return 'bg-warning text-dark';
    return 'bg-danger';
  }

    guardarExtintor() {
        if (!this.usuario) return;
        const extintor = {
            ...this.nuevoExtintor,
            fechaFabricacion: new Date(this.nuevoExtintor.fechaFabricacion).toISOString(),
            fechaUltimaCarga: new Date(this.nuevoExtintor.fechaUltimaCarga).toISOString(),
            fechaVencimiento: new Date(this.nuevoExtintor.fechaVencimiento).toISOString(),
            hojaDeVida: { id: 0, fechaRegistroInicial: new Date().toISOString(), recargas: [] }
        };
        this.extintoresService.agregarExtintor(extintor).subscribe(data => {
            if (data) {
            this.mensaje = 'Extintor registrado correctamente.';
            this.mostrarFormExtintor = false;
            this.nuevoExtintor = {};
            this.cargarExtintores();
            } else {
            this.mensaje = 'Error al registrar extintor. Verifique que el N° de serie sea único.';
            }
        });
    }

  verHojaDeVida(extintor: Extintor) {
    this.extintorSeleccionado = extintor;
    this.mostrarHojaDeVida = true;
    this.mostrarFormRecarga = false;
  }

  guardarRecarga() {
    if (!this.usuario || !this.extintorSeleccionado) return;
    this.extintoresService.agregarRecarga(
      this.extintorSeleccionado.id,
      this.usuario.id,
      this.nuevaRecarga
    ).subscribe(data => {
      if (data) {
        this.mensaje = 'Recarga registrada correctamente.';
        this.mostrarFormRecarga = false;
        this.nuevaRecarga = {};
        this.cargarExtintores();
        this.extintorSeleccionado = data;
      } else {
        this.mensaje = 'Error al registrar recarga. Verifique el stock del insumo.';
      }
    });
  }

  eliminarExtintor(id: number) {
    if (!confirm('¿Eliminar este extintor?')) return;
    this.extintoresService.eliminarExtintor(id).subscribe(data => {
      this.cargarExtintores();
    });
  }

  volver() {
    this.mostrarHojaDeVida = false;
    this.mostrarFormRecarga = false;
    this.extintorSeleccionado = null;
  }
}