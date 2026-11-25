# GYMFIT

-- 1. Crear y usar la base de datos
CREATE DATABASE GymDB;
GO

USE GymDB;
GO

-- 2. Tabla de Roles (Admin, Entrenador, Recepcionista, Limpiadora)
CREATE TABLE Roles (
    IdRol INT IDENTITY(1,1) PRIMARY KEY,
    NombreRol VARCHAR(50) NOT NULL UNIQUE,
    Descripcion VARCHAR(150) NULL
);
GO

-- 3. Tabla de Usuarios / Empleados del sistema
CREATE TABLE Usuarios (
    IdUsuario INT IDENTITY(1,1) PRIMARY KEY,
    Documento VARCHAR(20) NOT NULL UNIQUE,
    NombreCompleto VARCHAR(100) NOT NULL,
    Telefono VARCHAR(20) NULL,
    NombreUsuario VARCHAR(50) NOT NULL UNIQUE,
    ContrasenaHash VARCHAR(255) NOT NULL, -- Ideal para guardar el hash SHA256 o BCrypt desde C#
    IdRol INT NOT NULL,
    Estado BIT NOT NULL DEFAULT 1, -- 1 = Activo, 0 = Inactivo
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Usuarios_Roles FOREIGN KEY (IdRol) REFERENCES Roles(IdRol)
);
GO

-- 4. Tabla de Planes de Membresía (Días y Precio)
CREATE TABLE Planes (
    IdPlan INT IDENTITY(1,1) PRIMARY KEY,
    NombrePlan VARCHAR(60) NOT NULL,
    DuracionDias INT NOT NULL CHECK (DuracionDias > 0),
    Precio DECIMAL(10, 2) NOT NULL CHECK (Precio >= 0),
    Descripcion VARCHAR(200) NULL,
    Estado BIT NOT NULL DEFAULT 1 -- 1 = Disponible, 0 = Inactivo (para no borrar historial)
);
GO

-- 5. Tabla de Clientes
CREATE TABLE Clientes (
    IdCliente INT IDENTITY(1,1) PRIMARY KEY,
    Documento VARCHAR(20) NOT NULL UNIQUE,
    NombreCompleto VARCHAR(100) NOT NULL,
    Telefono VARCHAR(20) NOT NULL,
    ContactoEmergencia VARCHAR(100) NULL,
    TelefonoEmergencia VARCHAR(20) NULL,
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    Estado BIT NOT NULL DEFAULT 1
);
GO

-- 6. Tabla de Membresías (Relación entre Cliente y Plan)
CREATE TABLE Membresias (
    IdMembresia INT IDENTITY(1,1) PRIMARY KEY,
    IdCliente INT NOT NULL,
    IdPlan INT NOT NULL,
    FechaInicio DATE NOT NULL,
    FechaFin DATE NOT NULL,
    Estado VARCHAR(20) NOT NULL DEFAULT 'Activa', -- 'Activa', 'Vencida', 'Cancelada'
    CONSTRAINT FK_Membresias_Clientes FOREIGN KEY (IdCliente) REFERENCES Clientes(IdCliente),
    CONSTRAINT FK_Membresias_Planes FOREIGN KEY (IdPlan) REFERENCES Planes(IdPlan),
    CONSTRAINT CHK_Fechas_Membresia CHECK (FechaFin >= FechaInicio)
);
GO

-- 7. Tabla de Pagos (Vinculada a la membresía y al empleado que cobró)
CREATE TABLE Pagos (
    IdPago INT IDENTITY(1,1) PRIMARY KEY,
    IdMembresia INT NOT NULL UNIQUE, -- Relación 1 a 1 con cada membresía adquirida
    IdUsuarioCobro INT NOT NULL,
    Monto DECIMAL(10, 2) NOT NULL CHECK (Monto >= 0),
    MetodoPago VARCHAR(30) NOT NULL, -- 'Efectivo', 'Transferencia', 'Tarjeta'
    FechaPago DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Pagos_Membresias FOREIGN KEY (IdMembresia) REFERENCES Membresias(IdMembresia),
    CONSTRAINT FK_Pagos_Usuarios FOREIGN KEY (IdUsuarioCobro) REFERENCES Usuarios(IdUsuario)
);
GO

-- 8. Tabla de Control de Acceso (Registro de entradas de clientes al gym)
CREATE TABLE RegistrosAcceso (
    IdAcceso INT IDENTITY(1,1) PRIMARY KEY,
    IdCliente INT NOT NULL,
    FechaHoraIngreso DATETIME NOT NULL DEFAULT GETDATE(),
    AccesoPermitido BIT NOT NULL, -- 1 = Permitido, 0 = Denegado
    Observacion VARCHAR(150) NULL, -- Ej. 'Plan vigente - 18 días restantes' o 'Plan vencido'
    CONSTRAINT FK_Acceso_Clientes FOREIGN KEY (IdCliente) REFERENCES Clientes(IdCliente)
);
GO

-- 9. Tabla de Rutinas Básicas (Gestionada por el Entrenador)
CREATE TABLE Rutinas (
    IdRutina INT IDENTITY(1,1) PRIMARY KEY,
    IdCliente INT NOT NULL,
    IdEntrenador INT NOT NULL,
    Objetivo VARCHAR(100) NOT NULL, -- Ej. 'Hipertrofia', 'Pérdida de peso', 'Acondicionamiento'
    DescripcionRutina VARCHAR(MAX) NOT NULL,
    ObservacionesFisicas VARCHAR(300) NULL,
    FechaAsignacion DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Rutinas_Clientes FOREIGN KEY (IdCliente) REFERENCES Clientes(IdCliente),
    CONSTRAINT FK_Rutinas_Usuarios FOREIGN KEY (IdEntrenador) REFERENCES Usuarios(IdUsuario)
);
GO

-- 10. Tabla de Turnos y Novedades Operativas (Limpieza, Recepción, Mantenimiento)
CREATE TABLE NovedadesPersonal (
    IdNovedad INT IDENTITY(1,1) PRIMARY KEY,
    IdUsuario INT NOT NULL,
    TipoRegistro VARCHAR(30) NOT NULL, -- 'Inicio Turno', 'Fin Turno', 'Novedad Aseo', 'Mantenimiento'
    Area VARCHAR(60) NULL, -- Ej. 'Zona de pesas', 'Baños', 'Recepción'
    Detalle VARCHAR(300) NOT NULL,
    FechaHora DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Novedades_Usuarios FOREIGN KEY (IdUsuario) REFERENCES Usuarios(IdUsuario)
);
GO


-- Poblar base de datos

USE GymDB;
GO

-- Insertar los 4 roles principales
INSERT INTO Roles (NombreRol, Descripcion) VALUES
('Administrador', 'Control total del sistema, usuarios, planes y reportes'),
('Recepcionista', 'Registro de clientes, venta de planes, cobros y control de acceso'),
('Entrenador', 'Consulta de clientes activos y asignación de rutinas'),
('Limpiadora', 'Registro de turnos y novedades de aseo o mantenimiento');
GO

-- Insertar planes básicos por días y precio (en pesos colombianos)
INSERT INTO Planes (NombrePlan, DuracionDias, Precio, Descripcion) VALUES
('Pase Diario', 1, 8000.00, 'Acceso completo por 1 día'),
('Plan Quincenal', 15, 35000.00, 'Acceso ilimitado durante 15 días calendario'),
('Plan Mensual', 30, 60000.00, 'Acceso ilimitado durante 30 días calendario'),
('Plan Trimestral', 90, 160000.00, 'Acceso ilimitado durante 90 días calendario con descuento');
GO

-- Insertar un usuario Administrador y uno de cada rol para pruebas
-- Nota: En producción la contraseña debe guardarse hasheada desde C#
INSERT INTO Usuarios (Documento, NombreCompleto, Telefono, NombreUsuario, ContrasenaHash, IdRol) VALUES
('10000001', 'Carlos Administrador', '3001112233', 'admin', '1234', 1),
('10000002', 'Laura Recepción', '3002223344', 'recepcion', '1234', 2),
('10000003', 'Andrés Entrenador', '3003334455', 'entrenador', '1234', 3),
('10000004', 'Marta Servicios', '3004445566', 'limpieza', '1234', 4);
GOUSE GymDB;
GO

-- Insertar los 4 roles principales
INSERT INTO Roles (NombreRol, Descripcion) VALUES
('Administrador', 'Control total del sistema, usuarios, planes y reportes'),
('Recepcionista', 'Registro de clientes, venta de planes, cobros y control de acceso'),
('Entrenador', 'Consulta de clientes activos y asignación de rutinas'),
('Limpiadora', 'Registro de turnos y novedades de aseo o mantenimiento');
GO

-- Insertar planes básicos por días y precio (en pesos colombianos)
INSERT INTO Planes (NombrePlan, DuracionDias, Precio, Descripcion) VALUES
('Pase Diario', 1, 8000.00, 'Acceso completo por 1 día'),
('Plan Quincenal', 15, 35000.00, 'Acceso ilimitado durante 15 días calendario'),
('Plan Mensual', 30, 60000.00, 'Acceso ilimitado durante 30 días calendario'),
('Plan Trimestral', 90, 160000.00, 'Acceso ilimitado durante 90 días calendario con descuento');
GO

-- Insertar un usuario Administrador y uno de cada rol para pruebas
-- Nota: En producción la contraseña debe guardarse hasheada desde C#
INSERT INTO Usuarios (Documento, NombreCompleto, Telefono, NombreUsuario, ContrasenaHash, IdRol) VALUES
('10000001', 'Carlos Administrador', '3001112233', 'admin', '1234', 1),
('10000002', 'Laura Recepción', '3002223344', 'recepcion', '1234', 2),
('10000003', 'Andrés Entrenador', '3003334455', 'entrenador', '1234', 3),
('10000004', 'Marta Servicios', '3004445566', 'limpieza', '1234', 4);
GO
