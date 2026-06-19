import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiClient } from '../core/http/api-client';
import { Venta, ExtintorItem, RecargaItem } from '../modelos/modelos';

const BASE = 'http://localhost:5057/ventas';

@Injectable({ providedIn: 'root' })
export class VentasService {
  constructor(private api: ApiClient) {}

  listaVentas(): Observable<Venta[]> {
    return this.api.get<Venta[]>(`${BASE}/lista-ventas`);
  }

  obtenerVenta(id: number): Observable<Venta> {
    return this.api.get<Venta>(`${BASE}/obtener-venta/${id}`);
  }

  obtenerPorCliente(clienteId: number): Observable<Venta[]> {
    return this.api.get<Venta[]>(`${BASE}/por-cliente/${clienteId}`);
  }

  obtenerPorFecha(desde: string, hasta: string): Observable<Venta[]> {
    return this.api.get<Venta[]>(`${BASE}/por-fecha?desde=${desde}&hasta=${hasta}`);
  }

  crearVenta(venta: any): Observable<Venta> {
    return this.api.post<Venta>(`${BASE}/crear-venta`, venta);
  }

  agregarItemExtintor(ventaId: number, item: ExtintorItem): Observable<Venta> {
    return this.api.post<Venta>(`${BASE}/agregar-item/${ventaId}`, item);
  }

  agregarItemRecarga(ventaId: number, item: RecargaItem): Observable<Venta> {
    return this.api.post<Venta>(`${BASE}/agregar-item-recarga/${ventaId}`, item);
  }

  aplicarDescuento(ventaId: number, usuarioId: number, porcentaje: number): Observable<Venta> {
    return this.api.put<Venta>(`${BASE}/aplicar-descuento/${ventaId}/${usuarioId}`, porcentaje);
  }

  confirmarVenta(ventaId: number, usuarioId: number): Observable<Venta> {
    return this.api.put<Venta>(`${BASE}/confirmar-venta/${ventaId}/${usuarioId}`, {});
  }

  anularVenta(ventaId: number): Observable<boolean> {
    return this.api.put<boolean>(`${BASE}/anular-venta/${ventaId}`, {});
  }
}