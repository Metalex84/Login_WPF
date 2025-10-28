-- Script para crear la base de datos LoginWPF_DB
-- Ejecutar en SQL Server Management Studio o en Visual Studio

USE master;
GO

-- Crear la base de datos si no existe
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'LoginWPF_DB')
BEGIN
    CREATE DATABASE LoginWPF_DB;
    PRINT 'Base de datos LoginWPF_DB creada exitosamente.';
END
ELSE
BEGIN
    PRINT 'La base de datos LoginWPF_DB ya existe.';
END
GO

-- Usar la base de datos
USE LoginWPF_DB;
GO

-- Crear la tabla Usuarios si no existe
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Usuarios]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Usuarios] (
        [IdUsuario] INT IDENTITY(1,1) NOT NULL,
        [NombreUsuario] NVARCHAR(30) NOT NULL,
        [Email] NVARCHAR(100) NOT NULL,
        [PasswordHash] NVARCHAR(255) NOT NULL,
        [FechaCreacion] DATETIME NOT NULL DEFAULT GETDATE(),
        [FechaUltimoAcceso] DATETIME NULL,
        [Activo] BIT NOT NULL DEFAULT 1,
        CONSTRAINT [PK_Usuarios] PRIMARY KEY CLUSTERED ([IdUsuario] ASC)
    );
    PRINT 'Tabla Usuarios creada exitosamente.';
END
ELSE
BEGIN
    PRINT 'La tabla Usuarios ya existe.';
END
GO

-- Crear índice único para NombreUsuario
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = N'IX_Usuarios_NombreUsuario' AND object_id = OBJECT_ID(N'[dbo].[Usuarios]'))
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX [IX_Usuarios_NombreUsuario]
    ON [dbo].[Usuarios] ([NombreUsuario] ASC);
    PRINT 'Índice IX_Usuarios_NombreUsuario creado exitosamente.';
END
ELSE
BEGIN
    PRINT 'El índice IX_Usuarios_NombreUsuario ya existe.';
END
GO

-- Crear índice único para Email
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = N'IX_Usuarios_Email' AND object_id = OBJECT_ID(N'[dbo].[Usuarios]'))
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX [IX_Usuarios_Email]
    ON [dbo].[Usuarios] ([Email] ASC);
    PRINT 'Índice IX_Usuarios_Email creado exitosamente.';
END
ELSE
BEGIN
    PRINT 'El índice IX_Usuarios_Email ya existe.';
END
GO

-- Verificar la estructura de la tabla
PRINT '';
PRINT '========================================';
PRINT 'Estructura de la tabla Usuarios:';
PRINT '========================================';
EXEC sp_help 'dbo.Usuarios';
GO

-- Mostrar información de la base de datos
PRINT '';
PRINT '========================================';
PRINT 'Base de datos creada exitosamente!';
PRINT 'Nombre: LoginWPF_DB';
PRINT 'Tabla: Usuarios';
PRINT '========================================';
GO
