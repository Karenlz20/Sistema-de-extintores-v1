CREATE DATABASE SistemaExtintores;
GO

USE SistemaExtintores;
GO

CREATE TABLE Usuarios (
    Id INT PRIMARY KEY IDENTITY,
    NombreUsuario NVARCHAR(50) NOT NULL,
    Password NVARCHAR(100) NOT NULL,
    NombreCompleto NVARCHAR(100) NOT NULL,
    Rol INT NOT NULL,
    Activo BIT NOT NULL DEFAULT 1
);

CREATE TABLE Clientes (
    Id INT PRIMARY KEY IDENTITY,
    TipoCliente NVARCHAR(10) NOT NULL,
    Telefono NVARCHAR(20),
    Correo NVARCHAR(100),
    Direccion NVARCHAR(200),
    Activo BIT NOT NULL DEFAULT 1,
    -- ClientePersona
    NombreCompleto NVARCHAR(100) NULL,
    CI NVARCHAR(20) NULL,
    -- ClienteEmpresa
    RazonSocial NVARCHAR(100) NULL,
    NIT NVARCHAR(20) NULL,
    NombreContacto NVARCHAR(100) NULL
);

CREATE TABLE Sucursales (
    Id INT PRIMARY KEY IDENTITY,
    ClienteEmpresaId INT NOT NULL FOREIGN KEY REFERENCES Clientes(Id),
    Nombre NVARCHAR(100) NOT NULL,
    Direccion NVARCHAR(200),
    Telefono NVARCHAR(20),
    Activa BIT NOT NULL DEFAULT 1
);

CREATE TABLE CilindrosInventario (
    Id INT PRIMARY KEY IDENTITY,
    NroSerie NVARCHAR(50) NOT NULL UNIQUE,
    TipoAgente NVARCHAR(50) NOT NULL,
    Capacidad FLOAT NOT NULL,
    PrecioUnitario FLOAT NOT NULL,
    Disponible BIT NOT NULL DEFAULT 1
);

CREATE TABLE InsumosInventario (
    Id INT PRIMARY KEY IDENTITY,
    Nombre NVARCHAR(100) NOT NULL,
    Unidad NVARCHAR(20) NOT NULL,
    Stock FLOAT NOT NULL DEFAULT 0,
    PuntoReorden FLOAT NOT NULL DEFAULT 0
);

CREATE TABLE MovimientosInventario (
    Id INT PRIMARY KEY IDENTITY,
    Fecha DATETIME NOT NULL DEFAULT GETDATE(),
    Tipo INT NOT NULL,
    TipoItem INT NOT NULL,
    ItemId INT NOT NULL,
    Cantidad FLOAT NOT NULL,
    UsuarioId INT NOT NULL,
    Observacion NVARCHAR(200)
);

CREATE TABLE Extintores (
    Id INT PRIMARY KEY IDENTITY,
    NroSerie NVARCHAR(50) NOT NULL UNIQUE,
    TipoAgente NVARCHAR(50) NOT NULL,
    Capacidad FLOAT NOT NULL,
    FechaFabricacion DATETIME NOT NULL,
    FechaUltimaCarga DATETIME NOT NULL,
    FechaVencimiento DATETIME NOT NULL,
    ClienteId INT NOT NULL FOREIGN KEY REFERENCES Clientes(Id),
    SucursalId INT NULL FOREIGN KEY REFERENCES Sucursales(Id),
    CilindroInventarioId INT NULL FOREIGN KEY REFERENCES CilindrosInventario(Id)
);

CREATE TABLE HojasDeVida (
    Id INT PRIMARY KEY IDENTITY,
    ExtintorId INT NOT NULL UNIQUE FOREIGN KEY REFERENCES Extintores(Id),
    FechaRegistroInicial DATETIME NOT NULL DEFAULT GETDATE()
);

CREATE TABLE Recargas (
    Id INT PRIMARY KEY IDENTITY,
    HojaDeVidaId INT NOT NULL FOREIGN KEY REFERENCES HojasDeVida(Id),
    Fecha DATETIME NOT NULL DEFAULT GETDATE(),
    AgenteUsado NVARCHAR(50),
    CantidadKg FLOAT NOT NULL,
    InsumoId INT NOT NULL FOREIGN KEY REFERENCES InsumosInventario(Id),
    Tecnico NVARCHAR(100),
    Observaciones NVARCHAR(200)
);

CREATE TABLE Ventas (
    Id INT PRIMARY KEY IDENTITY,
    Fecha DATETIME NOT NULL DEFAULT GETDATE(),
    ClienteId INT NOT NULL FOREIGN KEY REFERENCES Clientes(Id),
    SucursalId INT NULL FOREIGN KEY REFERENCES Sucursales(Id),
    UsuarioId INT NOT NULL FOREIGN KEY REFERENCES Usuarios(Id),
    Descuento FLOAT NOT NULL DEFAULT 0,
    Estado INT NOT NULL DEFAULT 0,
    NumeroComprobante NVARCHAR(20)
);

CREATE TABLE ItemsVenta (
    Id INT PRIMARY KEY IDENTITY,
    VentaId INT NOT NULL FOREIGN KEY REFERENCES Ventas(Id),
    TipoItem NVARCHAR(10) NOT NULL,
    Cantidad INT NOT NULL,
    PrecioUnitario FLOAT NOT NULL,
    -- ExtintorItem
    NroSerie NVARCHAR(50) NULL,
    TipoAgente NVARCHAR(50) NULL,
    Capacidad FLOAT NULL,
    -- RecargaItem
    NroSerieExtintor NVARCHAR(50) NULL,
    InsumoId INT NULL FOREIGN KEY REFERENCES InsumosInventario(Id)
);

INSERT INTO Usuarios (NombreUsuario, Password, NombreCompleto, Rol, Activo)
VALUES 
    ('admin', 'Abc123', 'Administrador Principal', 0, 1),
    ('cajera1', 'Abc123', 'Ana Flores', 1, 1);
GO

ALTER TABLE InsumosInventario ADD EsPresurizante BIT NOT NULL DEFAULT 0;
UPDATE InsumosInventario SET EsPresurizante = 1 WHERE Nombre LIKE '%Nitrógeno%';