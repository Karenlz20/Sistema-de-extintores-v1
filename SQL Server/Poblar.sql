USE SistemaExtintores;
GO

-- CLIENTES
INSERT INTO Clientes (TipoCliente, Telefono, Correo, Direccion, Activo, NombreCompleto, CI)
VALUES ('Persona', '76543210', 'roberto.mamani@gmail.com', 'Av. Cañoto 123, Santa Cruz', 1, 'Roberto Mamani', '3456789');

INSERT INTO Clientes (TipoCliente, Telefono, Correo, Direccion, Activo, NombreCompleto, CI)
VALUES ('Persona', '77891234', 'carmen.vargas@gmail.com', 'Calle Murillo 456, Santa Cruz', 1, 'Carmen Vargas', '8765432');

INSERT INTO Clientes (TipoCliente, Telefono, Correo, Direccion, Activo, RazonSocial, NIT, NombreContacto)
VALUES ('Empresa', '33445566', 'contacto@hoteltajibos.com', 'Av. San Martín 455, Santa Cruz', 1, 'Hotel Los Tajibos SRL', '1234567890', 'Jorge Pérez');
GO

-- SUCURSALES 
INSERT INTO Sucursales (ClienteEmpresaId, Nombre, Direccion, Telefono, Activa)
VALUES (3, 'Sucursal Central', 'Av. San Martín 455, Santa Cruz', '33445566', 1);

INSERT INTO Sucursales (ClienteEmpresaId, Nombre, Direccion, Telefono, Activa)
VALUES (3, 'Sucursal Norte', 'Calle Beni 123, Santa Cruz', '33556677', 1);
GO

-- INSUMOS DE INVENTARIO 
INSERT INTO InsumosInventario (Nombre, Unidad, Stock, PuntoReorden)
VALUES ('Polvo Químico Seco ABC (PQS)', 'kg', 80, 15);         

INSERT INTO InsumosInventario (Nombre, Unidad, Stock, PuntoReorden)
VALUES ('Gas CO2', 'kg', 40, 8);                               

INSERT INTO InsumosInventario (Nombre, Unidad, Stock, PuntoReorden)
VALUES ('Nitrógeno (gas presurizante)', 'kg', 25, 5);        

INSERT INTO InsumosInventario (Nombre, Unidad, Stock, PuntoReorden)
VALUES ('Agua destilada', 'litros', 150, 30);                

INSERT INTO InsumosInventario (Nombre, Unidad, Stock, PuntoReorden)
VALUES ('Espuma AFFF', 'litros', 30, 8);                    
GO

-- CILINDROS DE INVENTARIO
-- EXT-RB-001 ya fue vendido (Disponible=0), los demás disponibles
INSERT INTO CilindrosInventario (NroSerie, TipoAgente, Capacidad, PrecioUnitario, Disponible)
VALUES ('EXT-RB-001', 'PQS ABC', 6, 450.00, 0);   -- Id=1 (vendido, se convirtió en extintor)

INSERT INTO CilindrosInventario (NroSerie, TipoAgente, Capacidad, PrecioUnitario, Disponible)
VALUES ('CIL-002', 'PQS ABC', 12, 750.00, 1);      -- Id=2

INSERT INTO CilindrosInventario (NroSerie, TipoAgente, Capacidad, PrecioUnitario, Disponible)
VALUES ('CIL-003', 'CO2', 5, 850.00, 1);           -- Id=3

INSERT INTO CilindrosInventario (NroSerie, TipoAgente, Capacidad, PrecioUnitario, Disponible)
VALUES ('CIL-004', 'PQS ABC', 6, 450.00, 1);       -- Id=4

INSERT INTO CilindrosInventario (NroSerie, TipoAgente, Capacidad, PrecioUnitario, Disponible)
VALUES ('CIL-005', 'Agua', 9, 380.00, 1);          -- Id=5
GO

-- EXTINTORES
-- EXT-RB-001: Vigente, Roberto Mamani (ClienteId=1), viene del cilindro Id=1
INSERT INTO Extintores (NroSerie, TipoAgente, Capacidad, FechaFabricacion, FechaUltimaCarga, FechaVencimiento, ClienteId, SucursalId, CilindroInventarioId)
VALUES ('EXT-RB-001', 'PQS ABC', 6, '2024-01-15', '2025-06-10', '2027-01-15', 1, NULL, 1);   

-- EXT-CV-001: Próximo a vencer (menos de 30 días), Carmen Vargas (ClienteId=2)
INSERT INTO Extintores (NroSerie, TipoAgente, Capacidad, FechaFabricacion, FechaUltimaCarga, FechaVencimiento, ClienteId, SucursalId, CilindroInventarioId)
VALUES ('EXT-CV-001', 'CO2', 5, '2023-07-01', '2025-07-01', '2026-07-08', 2, NULL, NULL);   

-- EXT-HT-001: Fuera de norma (vencido), Hotel Sucursal Central (SucursalId=1)
INSERT INTO Extintores (NroSerie, TipoAgente, Capacidad, FechaFabricacion, FechaUltimaCarga, FechaVencimiento, ClienteId, SucursalId, CilindroInventarioId)
VALUES ('EXT-HT-001', 'PQS ABC', 12, '2022-03-01', '2024-03-01', '2025-03-01', 3, 1, NULL); 

-- EXT-HT-002: Vigente, Hotel Sucursal Norte (SucursalId=2)
INSERT INTO Extintores (NroSerie, TipoAgente, Capacidad, FechaFabricacion, FechaUltimaCarga, FechaVencimiento, ClienteId, SucursalId, CilindroInventarioId)
VALUES ('EXT-HT-002', 'Agua', 9, '2024-11-01', '2024-11-01', '2027-11-01', 3, 2, NULL);      
GO

-- HOJAS DE VIDA (una por extintor, UNIQUE ExtintorId)
INSERT INTO HojasDeVida (ExtintorId, FechaRegistroInicial) VALUES (1, '2024-01-15');  
INSERT INTO HojasDeVida (ExtintorId, FechaRegistroInicial) VALUES (2, '2023-07-01');  
INSERT INTO HojasDeVida (ExtintorId, FechaRegistroInicial) VALUES (3, '2022-03-01');  
INSERT INTO HojasDeVida (ExtintorId, FechaRegistroInicial) VALUES (4, '2024-11-01'); 
GO

-- RECARGAS
-- EXT-RB-001 tuvo una recarga (HojaDeVidaId=1), usó PQS (InsumoId=1)
INSERT INTO Recargas (HojaDeVidaId, Fecha, AgenteUsado, CantidadKg, InsumoId, Tecnico, Observaciones)
VALUES (1, '2025-06-10', 'PQS ABC', 6, 1, 'Carlos Díaz', 'Recarga anual de mantenimiento');

-- EXT-HT-001 también tuvo recarga pero ya venció (HojaDeVidaId=3)
INSERT INTO Recargas (HojaDeVidaId, Fecha, AgenteUsado, CantidadKg, InsumoId, Tecnico, Observaciones)
VALUES (3, '2024-03-01', 'PQS ABC', 12, 1, 'Carlos Díaz', 'Recarga anual - pendiente renovación');
GO

-- MOVIMIENTOS DE INVENTARIO
-- TipoMovimiento: 0=Ingreso, 1=Ajuste, 2=VentaCilindro, 3=ConsumoRecarga
-- TipoItemInventario: 0=Cilindro, 1=Insumo
INSERT INTO MovimientosInventario (Fecha, Tipo, TipoItem, ItemId, Cantidad, UsuarioId, Observacion)
VALUES ('2026-01-10', 0, 0, 1, 1, 1, 'Ingreso de cilindro EXT-RB-001'),
       ('2026-01-10', 0, 0, 2, 1, 1, 'Ingreso de cilindro CIL-002'),
       ('2026-01-10', 0, 0, 3, 1, 1, 'Ingreso de cilindro CIL-003'),
       ('2026-01-10', 0, 0, 4, 1, 1, 'Ingreso de cilindro CIL-004'),
       ('2026-01-10', 0, 0, 5, 1, 1, 'Ingreso de cilindro CIL-005'),
       ('2026-01-10', 0, 1, 1, 80, 1, 'Stock inicial PQS ABC'),
       ('2026-01-10', 0, 1, 2, 40, 1, 'Stock inicial CO2'),
       ('2026-01-10', 0, 1, 3, 25, 1, 'Stock inicial Nitrógeno'),
       ('2026-01-10', 0, 1, 4, 150, 1, 'Stock inicial Agua destilada'),
       ('2026-01-10', 0, 1, 5, 30, 1, 'Stock inicial Espuma AFFF'),
       ('2026-03-01', 2, 0, 1, 1, 1, 'Venta de cilindro EXT-RB-001'),
       ('2025-06-10', 3, 1, 1, 6, 1, 'Consumo por recarga EXT-RB-001'),
       ('2024-03-01', 3, 1, 1, 12, 1, 'Consumo por recarga EXT-HT-001');
GO

-- VENTAS
-- Venta confirmada: Roberto compró EXT-RB-001
INSERT INTO Ventas (Fecha, ClienteId, SucursalId, UsuarioId, Descuento, Estado, NumeroComprobante)
VALUES ('2026-03-01', 1, NULL, 1, 0, 1, 'V-2026-000001');   -- Id=1, Estado=1 Confirmada

-- Venta pendiente: Hotel quiere un extintor más
INSERT INTO Ventas (Fecha, ClienteId, SucursalId, UsuarioId, Descuento, Estado, NumeroComprobante)
VALUES ('2026-06-18', 3, 1, 2, 5, 0, '');                   -- Id=2, Estado=0 Pendiente
GO

-- ITEMS DE VENTA
-- Venta 1 (Confirmada): ExtintorItem
INSERT INTO ItemsVenta (VentaId, TipoItem, Cantidad, PrecioUnitario, NroSerie, TipoAgente, Capacidad, NroSerieExtintor, InsumoId)
VALUES (1, 'Extintor', 1, 450.00, 'EXT-RB-001', 'PQS ABC', 6, NULL, NULL);

-- Venta 2 (Pendiente): ExtintorItem pendiente de confirmar
INSERT INTO ItemsVenta (VentaId, TipoItem, Cantidad, PrecioUnitario, NroSerie, TipoAgente, Capacidad, NroSerieExtintor, InsumoId)
VALUES (2, 'Extintor', 1, 750.00, 'CIL-002', 'PQS ABC', 12, NULL, NULL);
GO