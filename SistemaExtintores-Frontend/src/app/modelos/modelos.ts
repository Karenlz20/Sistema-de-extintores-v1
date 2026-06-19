export enum RolUsuario {
  Administrador = 'Administrador',
  Cajera = 'Cajera'
}

export enum EstadoVigencia {
  Vigente = 'Vigente',
  ProximoAVencer = 'ProximoAVencer',
  FueraDeNorma = 'FueraDeNorma'
}

export enum EstadoVenta {
  Pendiente = 'Pendiente',
  Confirmada = 'Confirmada',
  Anulada = 'Anulada'
}

export enum TipoMovimiento {
  Ingreso = 'Ingreso',
  Ajuste = 'Ajuste',
  VentaCilindro = 'VentaCilindro',
  ConsumoRecarga = 'ConsumoRecarga'
}

export interface Usuario {
  id: number;
  nombreUsuario: string;
  nombreCompleto: string;
  password: string;
  rol: RolUsuario;
  activo: boolean;
  clienteId?: number;
}

export interface CilindroInventario {
  id: number;
  nroSerie: string;
  tipoAgente: string;
  capacidad: number;
  precioUnitario: number;
  disponible: boolean;
}

export interface InsumoInventario {
  id: number;
  nombre: string;
  unidad: string;
  stock: number;
  puntoReorden: number;
}

export interface MovimientoInventario {
  id: number;
  fecha: string;
  tipo: TipoMovimiento;
  itemId: number;
  tipoItem: number;
  cantidad: number;
  usuarioId: number;
  observacion: string;
}

export interface Recarga {
  id: number;
  fecha: string;
  agenteUsado: string;
  cantidadKg: number;
  insumoId: number;
  tecnico: string;
  observaciones: string;
}

export interface HojaDeVida {
  id: number;
  fechaRegistroInicial: string;
  recargas: Recarga[];
}

export interface Extintor {
  id: number;
  nroSerie: string;
  tipoAgente: string;
  capacidad: number;
  fechaFabricacion: string;
  fechaUltimaCarga: string;
  fechaVencimiento: string;
  clienteId: number;
  sucursalId?: number;
  cilindroInventarioId?: number;
  hojaDeVida: HojaDeVida;
}

export interface ItemVenta {
  id: number;
  cantidad: number;
  precioUnitario: number;
}

export interface ExtintorItem extends ItemVenta {
  nroSerie: string;
  tipoAgente: string;
  capacidad: number;
}

export interface RecargaItem extends ItemVenta {
  nroSerieExtintor: string;
  insumoId: number;
}

export interface Venta {
  id: number;
  fecha: string;
  clienteId: number;
  sucursalId?: number;
  usuarioId: number;
  descuento: number;
  estado: EstadoVenta;
  numeroComprobante: string;
  items: ItemVenta[];
}

export interface AjusteInsumoRequest {
  cantidad: number;
  observacion: string;
}