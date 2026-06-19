import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiClient } from '../core/http/api-client';
import { Extintor, Recarga, EstadoVigencia } from '../modelos/modelos';

const BASE = 'http://localhost:5057/extintores';

@Injectable({ providedIn: 'root' })
export class ExtintoresService {
  constructor(private api: ApiClient) {}

  listaExtintores(): Observable<Extintor[]> {
    return this.api.get<Extintor[]>(`${BASE}/lista-extintores`);
  }

  obtenerExtintor(id: number): Observable<Extintor> {
    return this.api.get<Extintor>(`${BASE}/obtener-extintor/${id}`);
  }

  buscarPorNroSerie(nroSerie: string): Observable<Extintor> {
    return this.api.get<Extintor>(`${BASE}/buscar-extintor/${nroSerie}`);
  }

  obtenerPorCliente(clienteId: number): Observable<Extintor[]> {
    return this.api.get<Extintor[]>(`${BASE}/por-cliente/${clienteId}`);
  }

  obtenerPorEstado(estado: EstadoVigencia): Observable<Extintor[]> {
    return this.api.get<Extintor[]>(`${BASE}/por-estado/${estado}`);
  }

  agregarExtintor(extintor: Extintor): Observable<Extintor> {
    return this.api.post<Extintor>(`${BASE}/agregar-extintor`, extintor);
  }

  actualizarExtintor(id: number, extintor: Extintor): Observable<Extintor> {
    return this.api.put<Extintor>(`${BASE}/actualizar-extintor/${id}`, extintor);
  }

  agregarRecarga(extintorId: number, usuarioId: number, recarga: Recarga): Observable<Extintor> {
    return this.api.post<Extintor>(`${BASE}/agregar-recarga/${extintorId}/${usuarioId}`, recarga);
  }

  eliminarExtintor(id: number): Observable<boolean> {
    return this.api.delete<boolean>(`${BASE}/eliminar-extintor/${id}`);
  }
}