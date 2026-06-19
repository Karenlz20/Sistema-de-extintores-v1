import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiClient } from '../core/http/api-client';
import {
  CilindroInventario,
  InsumoInventario,
  MovimientoInventario,
  AjusteInsumoRequest
} from '../modelos/modelos';

const BASE = 'http://localhost:5057/inventario';

@Injectable({ providedIn: 'root' })
export class InventarioService {
  constructor(private api: ApiClient) {}

  // CILINDROS
  listaCilindros(): Observable<CilindroInventario[]> {
    return this.api.get<CilindroInventario[]>(`${BASE}/lista-cilindros`);
  }

  cilindrosDisponibles(): Observable<CilindroInventario[]> {
    return this.api.get<CilindroInventario[]>(`${BASE}/cilindros-disponibles`);
  }

  cilindrosVendidos(): Observable<CilindroInventario[]> {
    return this.api.get<CilindroInventario[]>(`${BASE}/cilindros-vendidos`);
  }

  obtenerCilindro(id: number): Observable<CilindroInventario> {
    return this.api.get<CilindroInventario>(`${BASE}/obtener-cilindro/${id}`);
  }

  buscarCilindroPorSerie(nroSerie: string): Observable<CilindroInventario> {
    return this.api.get<CilindroInventario>(`${BASE}/buscar-cilindro/${nroSerie}`);
  }

  agregarCilindro(usuarioId: number, cilindro: CilindroInventario): Observable<CilindroInventario> {
    return this.api.post<CilindroInventario>(`${BASE}/agregar-cilindro/${usuarioId}`, cilindro);
  }

  eliminarCilindro(id: number): Observable<boolean> {
    return this.api.delete<boolean>(`${BASE}/eliminar-cilindro/${id}`);
  }

  // INSUMOS
  listaInsumos(): Observable<InsumoInventario[]> {
    return this.api.get<InsumoInventario[]>(`${BASE}/lista-insumos`);
  }

  insumosBajoReorden(): Observable<InsumoInventario[]> {
    return this.api.get<InsumoInventario[]>(`${BASE}/insumos-bajo-reorden`);
  }

  obtenerInsumo(id: number): Observable<InsumoInventario> {
    return this.api.get<InsumoInventario>(`${BASE}/obtener-insumo/${id}`);
  }

  agregarInsumo(usuarioId: number, insumo: InsumoInventario): Observable<InsumoInventario> {
    return this.api.post<InsumoInventario>(`${BASE}/agregar-insumo/${usuarioId}`, insumo);
  }

  actualizarInsumo(id: number, insumo: InsumoInventario): Observable<InsumoInventario> {
    return this.api.put<InsumoInventario>(`${BASE}/actualizar-insumo/${id}`, insumo);
  }

  registrarIngresoInsumo(id: number, usuarioId: number, cantidad: number): Observable<boolean> {
    return this.api.put<boolean>(`${BASE}/ingreso-insumo/${id}/${usuarioId}`, cantidad);
  }

  registrarAjusteInsumo(id: number, usuarioId: number, request: AjusteInsumoRequest): Observable<boolean> {
    return this.api.put<boolean>(`${BASE}/ajuste-insumo/${id}/${usuarioId}`, request);
  }

  eliminarInsumo(id: number): Observable<boolean> {
    return this.api.delete<boolean>(`${BASE}/eliminar-insumo/${id}`);
  }

  // MOVIMIENTOS
  listaMovimientos(): Observable<MovimientoInventario[]> {
    return this.api.get<MovimientoInventario[]>(`${BASE}/movimientos`);
  }
}