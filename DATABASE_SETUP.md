# Configuración de Entity Framework y SQL Server

## 📦 Instalación de Entity Framework

### Opción 1: Usar NuGet Package Manager en Visual Studio

1. Abre tu solución en Visual Studio
2. Click derecho en el proyecto `CustomWPF`
3. Selecciona **"Manage NuGet Packages..."**
4. Ve a la pestaña **"Browse"**
5. Busca **"EntityFramework"**
6. Instala la versión **6.4.4** o superior
7. Acepta las licencias y dependencias

### Opción 2: Usar Package Manager Console

1. En Visual Studio, ve a **Tools > NuGet Package Manager > Package Manager Console**
2. Ejecuta el siguiente comando:

```powershell
Install-Package EntityFramework -Version 6.4.4
```

### Opción 3: Agregar manualmente al .csproj

Si las opciones anteriores no funcionan, agrega estas líneas al archivo `CustomWPF.csproj`:

```xml
<ItemGroup>
  <Reference Include="EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089, processorArchitecture=MSIL">
    <HintPath>..\packages\EntityFramework.6.4.4\lib\net45\EntityFramework.dll</HintPath>
  </Reference>
  <Reference Include="EntityFramework.SqlServer, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089, processorArchitecture=MSIL">
    <HintPath>..\packages\EntityFramework.6.4.4\lib\net45\EntityFramework.SqlServer.dll</HintPath>
  </Reference>
  <Reference Include="System.ComponentModel.DataAnnotations" />
</ItemGroup>
```

## 🗄️ Configuración de la Base de Datos

### LocalDB (Recomendado para desarrollo)

LocalDB viene instalado con Visual Studio. La cadena de conexión ya está configurada en `App.config`:

```xml
<add name="LoginDbConnection" 
     connectionString="Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=LoginWPF_DB;Integrated Security=True..." 
     providerName="System.Data.SqlClient" />
```

### SQL Server Express (Alternativa)

Si prefieres usar SQL Server Express, descomenta esta sección en `App.config`:

```xml
<add name="LoginDbConnection" 
     connectionString="Data Source=.\SQLEXPRESS;Initial Catalog=LoginWPF_DB;Integrated Security=True" 
     providerName="System.Data.SqlClient" />
```

## 🚀 Crear la Base de Datos

### Opción 1: Usando Migraciones de Entity Framework (Recomendado)

1. Abre **Package Manager Console** en Visual Studio
2. Ejecuta los siguientes comandos:

```powershell
# Habilitar migraciones
Enable-Migrations

# Crear migración inicial
Add-Migration InitialCreate

# Aplicar la migración (crea la BD)
Update-Database
```

### Opción 2: Code First con inicialización automática

La base de datos se creará automáticamente la primera vez que ejecutes la aplicación y uses una función que acceda a la BD.

### Opción 3: Crear manualmente con SQL

Ejecuta este script en SQL Server Management Studio o en Visual Studio:

```sql
CREATE DATABASE LoginWPF_DB;
GO

USE LoginWPF_DB;
GO

CREATE TABLE Usuarios (
    IdUsuario INT IDENTITY(1,1) PRIMARY KEY,
    NombreUsuario NVARCHAR(30) NOT NULL UNIQUE,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),
    FechaUltimoAcceso DATETIME NULL,
    Activo BIT NOT NULL DEFAULT 1
);
GO

-- Índices
CREATE UNIQUE INDEX IX_NombreUsuario ON Usuarios(NombreUsuario);
CREATE UNIQUE INDEX IX_Email ON Usuarios(Email);
GO
```

## 📁 Estructura del Proyecto

```
CustomWPF/
├── Models/
│   └── Usuario.cs              # Entidad de usuario
├── Data/
│   └── AppDbContext.cs         # Contexto de Entity Framework
├── Services/
│   └── AuthService.cs          # Lógica de autenticación
├── App.config                   # Configuración y cadena de conexión
└── MainWindow.xaml / RegisterWindow.xaml
```

## 🔧 Actualizar archivos del proyecto

Agrega estos archivos al `CustomWPF.csproj`:

```xml
<Compile Include="Models\Usuario.cs" />
<Compile Include="Data\AppDbContext.cs" />
<Compile Include="Services\AuthService.cs" />
```

O hazlo desde Visual Studio:
1. Click derecho en el proyecto > Add > Existing Item
2. Selecciona cada archivo .cs creado

## ✅ Probar la Conexión

Después de compilar el proyecto, puedes probar la conexión agregando este código temporal en el constructor de `MainWindow`:

```csharp
// En MainWindow.xaml.cs
using CustomWPF.Services;

public MainWindow()
{
    InitializeComponent();
    CargarCredencialesGuardadas();
    
    // Probar conexión (remover después de verificar)
    var authService = new AuthService();
    if (authService.ProbarConexion(out string mensaje))
    {
        MessageBox.Show(mensaje, "Conexión OK");
    }
    else
    {
        MessageBox.Show(mensaje, "Error de Conexión");
    }
}
```

## 🔐 Usar el Servicio de Autenticación

### En el Login (MainWindow.xaml.cs):

```csharp
private void btnEntrar_Click(object sender, RoutedEventArgs e)
{
    // ... validaciones existentes ...
    
    var authService = new AuthService();
    if (authService.ValidarCredenciales(usuario, contrasena, out string mensaje))
    {
        // Login exitoso
        MessageBox.Show($"Bienvenido, {usuario}!", "Login Exitoso");
        
        // Guardar credenciales si está marcado
        if (chkRecordarContrasena.IsChecked == true)
        {
            Properties.Settings.Default.Usuario = usuario;
            Properties.Settings.Default.Contrasena = contrasena;
            Properties.Settings.Default.RecordarContrasena = true;
            Properties.Settings.Default.Save();
        }
    }
    else
    {
        MessageBox.Show(mensaje, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}
```

### En el Registro (RegisterWindow.xaml.cs):

```csharp
private void btnRegistrarse_Click(object sender, RoutedEventArgs e)
{
    // ... validaciones existentes ...
    
    var authService = new AuthService();
    if (authService.RegistrarUsuario(usuario, email, contrasena, out string mensaje))
    {
        MessageBox.Show(mensaje, "Registro Exitoso");
        this.Close(); // Volver al login
    }
    else
    {
        MessageBox.Show(mensaje, "Error");
    }
}
```

## 🛠️ Solución de Problemas

### Error: "Could not load file or assembly 'EntityFramework'"

- Asegúrate de que EntityFramework está instalado correctamente
- Verifica que las referencias estén en el archivo .csproj
- Reconstruye la solución (Build > Rebuild Solution)

### Error: "Cannot attach the file as database"

- Verifica que SQL Server LocalDB esté instalado
- Cambia la cadena de conexión a SQL Server Express si es necesario

### Error: "Login failed for user"

- Verifica que la cadena de conexión use `Integrated Security=True`
- Asegúrate de tener permisos en SQL Server

### La base de datos no se crea automáticamente

- Ejecuta manualmente `Update-Database` en Package Manager Console
- O ejecuta el script SQL manualmente

## 📚 Recursos Adicionales

- [Entity Framework 6 Documentation](https://docs.microsoft.com/en-us/ef/ef6/)
- [Code First Migrations](https://docs.microsoft.com/en-us/ef/ef6/modeling/code-first/migrations/)
- [SQL Server LocalDB](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb)
