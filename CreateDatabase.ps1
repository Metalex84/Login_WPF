# Script PowerShell para crear la base de datos LoginWPF_DB
# Ejecutar en PowerShell con permisos de administrador

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Creando Base de Datos LoginWPF_DB" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Cadena de conexión
$serverName = "(localdb)\MSSQLLocalDB"
$masterConnectionString = "Server=$serverName;Database=master;Integrated Security=True;Connect Timeout=30;"

try {
    # Cargar ensamblado de SQL Client
    Add-Type -AssemblyName "System.Data"
    
    Write-Host "Conectando a SQL Server LocalDB..." -ForegroundColor Yellow
    
    # Crear conexión a la base de datos master
    $connection = New-Object System.Data.SqlClient.SqlConnection($masterConnectionString)
    $connection.Open()
    
    Write-Host "✓ Conectado exitosamente" -ForegroundColor Green
    Write-Host ""
    
    # Verificar si la base de datos existe
    Write-Host "Verificando si la base de datos existe..." -ForegroundColor Yellow
    $checkDbQuery = "SELECT database_id FROM sys.databases WHERE name = 'LoginWPF_DB'"
    $checkCommand = New-Object System.Data.SqlClient.SqlCommand($checkDbQuery, $connection)
    $dbExists = $checkCommand.ExecuteScalar()
    
    if ($null -eq $dbExists) {
        # Crear base de datos
        Write-Host "Creando base de datos LoginWPF_DB..." -ForegroundColor Yellow
        $createDbQuery = "CREATE DATABASE LoginWPF_DB"
        $createDbCommand = New-Object System.Data.SqlClient.SqlCommand($createDbQuery, $connection)
        $createDbCommand.ExecuteNonQuery() | Out-Null
        Write-Host "✓ Base de datos LoginWPF_DB creada exitosamente" -ForegroundColor Green
    } else {
        Write-Host "✓ La base de datos LoginWPF_DB ya existe" -ForegroundColor Green
    }
    
    $connection.Close()
    Write-Host ""
    
    # Conectar a la base de datos LoginWPF_DB para crear la tabla
    Write-Host "Conectando a LoginWPF_DB..." -ForegroundColor Yellow
    $dbConnectionString = "Server=$serverName;Database=LoginWPF_DB;Integrated Security=True;Connect Timeout=30;"
    $dbConnection = New-Object System.Data.SqlClient.SqlConnection($dbConnectionString)
    $dbConnection.Open()
    
    Write-Host "✓ Conectado a LoginWPF_DB" -ForegroundColor Green
    Write-Host ""
    
    # Crear tabla Usuarios
    Write-Host "Creando tabla Usuarios..." -ForegroundColor Yellow
    
    $createTableQuery = @"
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
END
"@
    
    $createTableCommand = New-Object System.Data.SqlClient.SqlCommand($createTableQuery, $dbConnection)
    $createTableCommand.ExecuteNonQuery() | Out-Null
    Write-Host "✓ Tabla Usuarios creada exitosamente" -ForegroundColor Green
    Write-Host ""
    
    # Crear índice único para NombreUsuario
    Write-Host "Creando índice para NombreUsuario..." -ForegroundColor Yellow
    
    $createIndexUserQuery = @"
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = N'IX_Usuarios_NombreUsuario' AND object_id = OBJECT_ID(N'[dbo].[Usuarios]'))
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX [IX_Usuarios_NombreUsuario]
    ON [dbo].[Usuarios] ([NombreUsuario] ASC);
END
"@
    
    $createIndexUserCommand = New-Object System.Data.SqlClient.SqlCommand($createIndexUserQuery, $dbConnection)
    $createIndexUserCommand.ExecuteNonQuery() | Out-Null
    Write-Host "✓ Índice IX_Usuarios_NombreUsuario creado" -ForegroundColor Green
    
    # Crear índice único para Email
    Write-Host "Creando índice para Email..." -ForegroundColor Yellow
    
    $createIndexEmailQuery = @"
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = N'IX_Usuarios_Email' AND object_id = OBJECT_ID(N'[dbo].[Usuarios]'))
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX [IX_Usuarios_Email]
    ON [dbo].[Usuarios] ([Email] ASC);
END
"@
    
    $createIndexEmailCommand = New-Object System.Data.SqlClient.SqlCommand($createIndexEmailQuery, $dbConnection)
    $createIndexEmailCommand.ExecuteNonQuery() | Out-Null
    Write-Host "✓ Índice IX_Usuarios_Email creado" -ForegroundColor Green
    Write-Host ""
    
    # Verificar estructura
    Write-Host "Verificando estructura de la tabla..." -ForegroundColor Yellow
    $verifyQuery = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Usuarios'"
    $verifyCommand = New-Object System.Data.SqlClient.SqlCommand($verifyQuery, $dbConnection)
    $columnCount = $verifyCommand.ExecuteScalar()
    Write-Host "✓ Tabla Usuarios tiene $columnCount columnas" -ForegroundColor Green
    
    $dbConnection.Close()
    
    Write-Host ""
    Write-Host "========================================" -ForegroundColor Green
    Write-Host "¡Base de datos creada exitosamente!" -ForegroundColor Green
    Write-Host "========================================" -ForegroundColor Green
    Write-Host ""
    Write-Host "Detalles:" -ForegroundColor Cyan
    Write-Host "  Servidor: $serverName" -ForegroundColor White
    Write-Host "  Base de datos: LoginWPF_DB" -ForegroundColor White
    Write-Host "  Tabla: Usuarios" -ForegroundColor White
    Write-Host "  Columnas: $columnCount" -ForegroundColor White
    Write-Host ""
    Write-Host "Cadena de conexión:" -ForegroundColor Cyan
    Write-Host "  $dbConnectionString" -ForegroundColor White
    Write-Host ""
    
} catch {
    Write-Host ""
    Write-Host "========================================" -ForegroundColor Red
    Write-Host "ERROR al crear la base de datos" -ForegroundColor Red
    Write-Host "========================================" -ForegroundColor Red
    Write-Host ""
    Write-Host "Mensaje de error:" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Detalles:" -ForegroundColor Red
    Write-Host $_.Exception -ForegroundColor Yellow
    Write-Host ""
    
    # Sugerencias
    Write-Host "Posibles soluciones:" -ForegroundColor Cyan
    Write-Host "1. Verifica que SQL Server LocalDB esté instalado" -ForegroundColor White
    Write-Host "2. Intenta iniciar LocalDB manualmente: sqllocaldb start MSSQLLocalDB" -ForegroundColor White
    Write-Host "3. Verifica que tengas permisos de administrador" -ForegroundColor White
    Write-Host ""
    
    exit 1
}

Write-Host "Presiona cualquier tecla para continuar..." -ForegroundColor Gray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
