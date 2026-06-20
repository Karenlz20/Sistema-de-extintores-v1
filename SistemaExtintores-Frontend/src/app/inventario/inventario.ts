import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { InventarioService } from './inventario.service';
import { CilindroInventario, InsumoInventario, Usuario, RolUsuario} from '../modelos/modelos';
import { Router } from '@angular/router';

@Component({
  selector: 'app-inventario',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './inventario.html'
})
export class Inventario implements OnInit {
  usuario: Usuario | null = null;
  esAdmin = false;

  // CILINDROS
  cilindros: CilindroInventario[] = [];
  filtroCilindro = '';
  mostrarFormCilindro = false;
  nuevoCilindro: Partial<CilindroInventario> = {};

  // INSUMOS
  insumos: InsumoInventario[] = [];
  filtroInsumo = '';
  mostrarFormInsumo = false;
  nuevoInsumo: Partial<InsumoInventario> = {};

  // INGRESO DE INSUMO
  mostrarFormIngreso = false;
  insumoSeleccionadoId = 0;
  cantidadIngreso = 0;

  // AJUSTE DE INSUMO
  mostrarFormAjuste = false;
  insumoAjusteId = 0;
  cantidadAjuste = 0;
  observacionAjuste = '';

  mensaje = '';

  constructor(private inventarioService: InventarioService, private router: Router) {}

  ngOnInit() {
    const raw = localStorage.getItem('usuario');
    if (!raw) {
        this.router.navigate(['/login']);
        return;
    }
    this.usuario = JSON.parse(raw);
    this.esAdmin = this.usuario?.rol === RolUsuario.Administrador;
    this.cargarCilindros();
    this.cargarInsumos();
    }

  cargarCilindros() {
    this.inventarioService.listaCilindros().subscribe(data => {
      this.cilindros = data;
    });
  }

  cargarInsumos() {
    this.inventarioService.listaInsumos().subscribe(data => {
      this.insumos = data;
    });
  }

  get cilindrosFiltrados() {
    const f = this.filtroCilindro.toLowerCase();
    const resultado: CilindroInventario[] = [];
    for (const c of this.cilindros) {
      if (
        c.nroSerie.toLowerCase().includes(f) ||
        c.tipoAgente.toLowerCase().includes(f)
      ) {
        resultado.push(c);
      }
    }
    return resultado;
  }

  get insumosFiltrados() {
    const f = this.filtroInsumo.toLowerCase();
    const resultado: InsumoInventario[] = [];
    for (const i of this.insumos) {
      if (i.nombre.toLowerCase().includes(f)) {
        resultado.push(i);
      }
    }
    return resultado;
  }

  guardarCilindro() {
    if (!this.usuario) return;
    const cilindro = this.nuevoCilindro as CilindroInventario;
    if (!cilindro.nroSerie || !cilindro.tipoAgente) {
      this.mensaje = 'Complete N° de serie y tipo de agente.';
      return;
    }
    if (cilindro.capacidad == null || cilindro.capacidad <= 0) {
      this.mensaje = 'La capacidad debe ser mayor a 0.';
      return;
    }
    if (cilindro.precioUnitario == null || cilindro.precioUnitario < 0) {
      this.mensaje = 'El precio no puede ser negativo.';
      return;
    }
    cilindro.disponible = true;
    this.inventarioService.agregarCilindro(this.usuario.id, cilindro).subscribe(result => {
      if (result) {
        this.mensaje = 'Cilindro agregado correctamente.';
        this.mostrarFormCilindro = false;
        this.nuevoCilindro = {};
        this.cargarCilindros();
      } else {
        this.mensaje = 'Error al agregar cilindro.';
      }
    });
  }

  eliminarCilindro(id: number) {
    if (!confirm('¿Eliminar este cilindro?')) return;
    this.inventarioService.eliminarCilindro(id).subscribe(data => {
        this.cargarCilindros();
    });
    }

  guardarInsumo() {
    if (!this.usuario) return;
    const insumo = this.nuevoInsumo as InsumoInventario;
    if (!insumo.nombre || !insumo.unidad) {
      this.mensaje = 'Complete nombre y unidad.';
      return;
    }
    if (insumo.stock == null || insumo.stock < 0) {
      this.mensaje = 'El stock inicial no puede ser negativo.';
      return;
    }
    if (insumo.puntoReorden == null || insumo.puntoReorden < 0) {
      this.mensaje = 'El punto de reorden no puede ser negativo.';
      return;
    }
    this.inventarioService.agregarInsumo(this.usuario.id, insumo).subscribe(result => {
      if (result) {
        this.mensaje = 'Insumo agregado correctamente.';
        this.mostrarFormInsumo = false;
        this.nuevoInsumo = {};
        this.cargarInsumos();
      } else {
        this.mensaje = 'Error al agregar insumo.';
      }
    });
  }

  eliminarInsumo(id: number) {
    if (!confirm('¿Eliminar este insumo?')) return;
    this.inventarioService.eliminarInsumo(id).subscribe(data => {
        this.cargarInsumos();
    });
    }

  guardarIngreso() {
    if (!this.usuario) return;
    if (!this.insumoSeleccionadoId) {
      this.mensaje = 'Seleccione un insumo.';
      return;
    }
    if (this.cantidadIngreso == null || this.cantidadIngreso <= 0) {
      this.mensaje = 'La cantidad ingresada debe ser mayor a 0.';
      return;
    }
    this.inventarioService.registrarIngresoInsumo(
      this.insumoSeleccionadoId,
      this.usuario.id,
      this.cantidadIngreso
    ).subscribe(ok => {
      if (ok) {
        this.mensaje = 'Ingreso registrado correctamente.';
        this.mostrarFormIngreso = false;
        this.cantidadIngreso = 0;
        this.insumoSeleccionadoId = 0;
        this.cargarInsumos();
      } else {
        this.mensaje = 'Error al registrar ingreso.';
      }
    });
  }

  guardarAjuste() {
    if (!this.usuario) return;
    this.inventarioService.registrarAjusteInsumo(
      this.insumoAjusteId,
      this.usuario.id,
      { cantidad: this.cantidadAjuste, observacion: this.observacionAjuste }
    ).subscribe(ok => {
      if (ok) {
        this.mensaje = 'Ajuste registrado correctamente.';
        this.mostrarFormAjuste = false;
        this.cantidadAjuste = 0;
        this.observacionAjuste = '';
        this.insumoAjusteId = 0;
        this.cargarInsumos();
      } else {
        this.mensaje = 'Error al registrar ajuste. Verifica que tengas permisos.';
      }
    });
  }

  requiereReorden(insumo: InsumoInventario) {
    return insumo.stock <= insumo.puntoReorden;
  }
}