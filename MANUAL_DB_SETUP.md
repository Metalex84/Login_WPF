# Configuración Manual de la Base de Datos

## ⚠️ LocalDB no está disponible

El script detectó que SQL Server LocalDB no está instalado o no está en ejecución. Tienes varias opciones:

## 📋 Opción 1: Instalar SQL Server LocalDB (Recomendado)

### Pasos:
1. Descarga SQL Server Express desde: https://www.microsoft.com/es-es/sql-server/sql-server-downloads
2. Selecciona "Express" y descarga
3. Durante la instalación, asegúrate de marcar "LocalDB"
4. Una vez instalado, ejecuta: `sqllocaldb start MSSQLLocalDB`
5. Luego ejecuta el script: `.\Setup-Database.ps1`

## 📋 Opción 2: Usar SQL Server Express

Si tienes SQL Server Express instalado:

### 1. Actualiza el archivo `App.config`
Abre `CustomWPF\App.config` y descomenta/modifica la cadena de conexión:

```xml
<connectionStrings>
  <add name="LoginDbConnection" 
       connectionString="Data Source=.\SQLEXPRESS;Initial Catalog=LoginWPF_DB;Integrated Security=True" 
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

### 2. Ejecuta este script SQL en SQL Server Management Studio

Abre SQL Server Management Studio (SSMS) y ejecuta:

```sql
USE master;
GO

-- Crear base de datos
CREATE DATABASE LoginWPF_DB;
GO

USE LoginWPF_DB;
GO

-- Crear tabla Usuarios
CREATE TABLE Usuarios (
    IdUsuario INT IDENTITY(1,1) PRIMARY KEY,
    NombreUsuario NVARCHAR(30) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),
    FechaUltimoAcceso DATETIME NULL,
    Activo BIT NOT NULL DEFAULT 1
);
GO

-- Crear índices únicos
CREATE UNIQUE INDEX IX_Usuarios_NombreUsuario ON Usuarios(NombreUsuario);
CREATE UNIQUE INDEX IX_Usuarios_Email ON Usuarios(Email);
GO

-- Verificar
SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Usuarios';
GO
```

## 📋 Opción 3: Usar Entity Framework Migrations

Si tienes Visual Studio abierto:

### 1. Abre Package Manager Console
En Visual Studio: **Tools > NuGet Package Manager > Package Manager Console**

### 2. Ejecuta estos comandos:

```powershell
# Habilitar migraciones
Enable-Migrations

# Crear migración inicial
Add-Migration InitialCreate

# Aplicar migración (crea la BD automáticamente)
Update-Database
```

Esto creará la base de datos automáticamente usando Code First.

## 📋 Opción 4: Dejar que Entity Framework cree la BD automáticamente

### 1. Agrega a `App.xaml.cs`:

```csharp
using System.Data.Entity;
using CustomWPF.Data;

protected override void OnStartup(StartupEventArgs e)
{
    base.OnStartup(e);
    
    // Crear BD si no existe
    Database.SetInitializer(new CreateDatabaseIfNotExists<AppDbContext>());
    
    using (var context = new AppDbContext())
    {
        context.Database.Initialize(force: false);
    }
}
```

### 2. Ejecuta la aplicación
La base de datos se creará automáticamente la primera vez que ejecutes la aplicación.

## 📋 Opción 5: Usar SQLite (Alternativa sin SQL Server)

Si no quieres instalar SQL Server, puedes usar SQLite:

### 1. Instala el paquete NuGet:
```powershell
Install-Package System.Data.SQLite
Install-Package System.Data.SQLite.EF6
```

### 2. Actualiza `App.config`:
```xml
<connectionStrings>
  <add name="LoginDbConnection" 
       connectionString="Data Source=LoginWPF.db;Version=3;" 
       providerName="System.Data.SQLite.EF6" />
</connectionStrings>
```

## 🔍 Verificar instalación de SQL Server

Para verificar qué tienes instalado, ejecuta en PowerShell:

```powershell
# Verificar servicios de SQL Server
Get-Service | Where-Object {$_.Name -like "*SQL*"}

# Verificar instancias de LocalDB
sqllocaldb info

# Ver instancias de SQL Server
Get-ItemProperty -Path 'HKLM:\SOFTWARE\Microsoft\Microsoft SQL Server' -ErrorAction SilentlyContinue
```

## ✅ Verificar que la BD fue creada

Después de usar cualquiera de las opciones anteriores, verifica con este código C#:

### Agregar temporalmente en `MainWindow.xaml.cs`:

```csharp
using CustomWPF.Services;

public MainWindow()
{
    InitializeComponent();
    CargarCredencialesGuardadas();
    
    // Probar conexión
    var authService = new AuthService();
    if (authService.ProbarConexion(out string mensaje))
    {
        MessageBox.Show(mensaje, "Conexión OK", MessageBoxButton.OK, MessageBoxImage.Information);
    }
    else
    {
        MessageBox.Show(mensaje, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}
```

## 📝 Archivos Disponibles

- `CreateDatabase.sql` - Script SQL completo
- `Setup-Database.ps1` - Script PowerShell simplificado
- `CreateDatabase.ps1` - Script PowerShell detallado

## 🆘 ¿Necesitas ayuda?

Si ninguna de estas opciones funciona:

1. Verifica que tienes alguna versión de SQL Server instalada
2. Considera usar SQLite como alternativa más simple
3. Usa migraciones de Entity Framework (Opción 3)
4. La base de datos se creará automáticamente al ejecutar la app (Opción 4)

## 💡 Recomendación

La **Opción 3** (Entity Framework Migrations) es la más recomendada si tienes Visual Studio, ya que:
- No requiere SQL Server instalado previamente
- Crea la BD automáticamente
- Maneja cambios en el esquema fácilmente
- Es la práctica estándar con Entity Framework
