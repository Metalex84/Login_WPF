# Script para crear base de datos LoginWPF_DB
Add-Type -AssemblyName "System.Data"

$serverName = "(localdb)\MSSQLLocalDB"
$masterConn = "Server=$serverName;Database=master;Integrated Security=True;Connect Timeout=30;"

Write-Host "Creando base de datos LoginWPF_DB..." -ForegroundColor Cyan

try {
    $conn = New-Object System.Data.SqlClient.SqlConnection($masterConn)
    $conn.Open()
    
    $checkDb = "SELECT database_id FROM sys.databases WHERE name = 'LoginWPF_DB'"
    $cmd = New-Object System.Data.SqlClient.SqlCommand($checkDb, $conn)
    $exists = $cmd.ExecuteScalar()
    
    if ($null -eq $exists) {
        $createDb = "CREATE DATABASE LoginWPF_DB"
        $cmd = New-Object System.Data.SqlClient.SqlCommand($createDb, $conn)
        $cmd.ExecuteNonQuery() | Out-Null
        Write-Host "Base de datos creada" -ForegroundColor Green
    } else {
        Write-Host "Base de datos ya existe" -ForegroundColor Yellow
    }
    
    $conn.Close()
    
    $dbConn = "Server=$serverName;Database=LoginWPF_DB;Integrated Security=True;Connect Timeout=30;"
    $conn2 = New-Object System.Data.SqlClient.SqlConnection($dbConn)
    $conn2.Open()
    
    $createTable = "IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'Usuarios')) BEGIN CREATE TABLE Usuarios (IdUsuario INT IDENTITY(1,1) PRIMARY KEY, NombreUsuario NVARCHAR(30) NOT NULL, Email NVARCHAR(100) NOT NULL, PasswordHash NVARCHAR(255) NOT NULL, FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(), FechaUltimoAcceso DATETIME NULL, Activo BIT NOT NULL DEFAULT 1) END"
    
    $cmd2 = New-Object System.Data.SqlClient.SqlCommand($createTable, $conn2)
    $cmd2.ExecuteNonQuery() | Out-Null
    Write-Host "Tabla Usuarios creada" -ForegroundColor Green
    
    $createIdx1 = "IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = N'IX_Usuarios_NombreUsuario') BEGIN CREATE UNIQUE INDEX IX_Usuarios_NombreUsuario ON Usuarios(NombreUsuario) END"
    $cmd3 = New-Object System.Data.SqlClient.SqlCommand($createIdx1, $conn2)
    $cmd3.ExecuteNonQuery() | Out-Null
    
    $createIdx2 = "IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = N'IX_Usuarios_Email') BEGIN CREATE UNIQUE INDEX IX_Usuarios_Email ON Usuarios(Email) END"
    $cmd4 = New-Object System.Data.SqlClient.SqlCommand($createIdx2, $conn2)
    $cmd4.ExecuteNonQuery() | Out-Null
    Write-Host "Indices creados" -ForegroundColor Green
    
    $conn2.Close()
    
    Write-Host ""
    Write-Host "Base de datos configurada exitosamente!" -ForegroundColor Green
    Write-Host "Servidor: $serverName" -ForegroundColor White
    Write-Host "Base de datos: LoginWPF_DB" -ForegroundColor White
    
} catch {
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
}
