# Guía de Seguridad - Prevención de Inyección SQL

## ✅ Implementaciones de Seguridad

### 1. Clase SecurityHelper
Se ha implementado una clase helper con múltiples métodos de validación:

#### Métodos disponibles:

- **`IsSqlInjectionSafe(string input)`**
  - Detecta patrones comunes de inyección SQL
  - Valida comandos SQL (SELECT, INSERT, DROP, etc.)
  - Detecta comentarios SQL (-- y /* */)
  - Identifica caracteres peligrosos

- **`IsValidUsername(string username)`**
  - Solo permite letras, números, puntos y guiones bajos
  - Longitud entre 3 y 30 caracteres
  - Previene caracteres especiales peligrosos

- **`IsValidEmail(string email)`**
  - Valida formato correcto de email
  - Usa expresiones regulares seguras

- **`IsStrongPassword(string password, int minLength)`**
  - Verifica longitud mínima
  - Requiere al menos una letra y un número
  - Personalizable según necesidades

- **`SanitizeInput(string input)`**
  - Remueve caracteres potencialmente peligrosos
  - Limpia comillas, punto y coma, comentarios SQL

- **`LimitLength(string input, int maxLength)`**
  - Previene ataques de buffer overflow
  - Limita la longitud de entrada

### 2. Validaciones Implementadas en Formularios

#### MainWindow (Login):
- ✅ Validación de formato de usuario
- ✅ Detección de patrones de inyección SQL
- ✅ Sanitización de inputs
- ✅ Mensajes de error específicos

#### RegisterWindow (Registro):
- ✅ Validación de formato de email
- ✅ Validación de formato de usuario
- ✅ Validación de fortaleza de contraseña
- ✅ Detección de inyección SQL en todos los campos
- ✅ Validación de coincidencia de contraseñas

## 🛡️ Mejores Prácticas

### Para cuando conectes a una base de datos:

1. **SIEMPRE usa consultas parametrizadas**
```csharp
// ✅ CORRECTO - Usa parámetros
string query = "SELECT * FROM Users WHERE Username = @username AND Password = @password";
SqlCommand cmd = new SqlCommand(query, connection);
cmd.Parameters.AddWithValue("@username", usuario);
cmd.Parameters.AddWithValue("@password", passwordHash);

// ❌ INCORRECTO - NUNCA concatenes strings
string query = "SELECT * FROM Users WHERE Username = '" + usuario + "'";
```

2. **Usa procedimientos almacenados**
```csharp
SqlCommand cmd = new SqlCommand("sp_ValidateUser", connection);
cmd.CommandType = CommandType.StoredProcedure;
cmd.Parameters.AddWithValue("@username", usuario);
```

3. **Hashea las contraseñas**
```csharp
// Usa BCrypt, PBKDF2 o similar
// NUNCA guardes contraseñas en texto plano
using System.Security.Cryptography;
// Ejemplo con SHA256 (para producción usa BCrypt)
byte[] data = Encoding.UTF8.GetBytes(password);
byte[] hash = SHA256.Create().ComputeHash(data);
```

4. **Limita permisos de la base de datos**
- La cuenta de la aplicación debe tener SOLO los permisos necesarios
- No uses la cuenta 'sa' o 'root'
- Implementa principio de mínimo privilegio

5. **Usa ORM (Entity Framework, Dapper)**
```csharp
// Entity Framework previene inyección SQL automáticamente
using (var context = new AppDbContext())
{
    var user = context.Users
        .FirstOrDefault(u => u.Username == usuario);
}
```

## ⚠️ Patrones Detectados

La clase SecurityHelper detecta estos patrones peligrosos:
- Comandos SQL: `SELECT`, `INSERT`, `UPDATE`, `DELETE`, `DROP`, `ALTER`, etc.
- Comentarios: `--`, `/* */`
- Funciones de conversión: `CAST()`, `CONVERT()`, `CHAR()`
- Operadores lógicos sospechosos: `OR 1=1`, `AND 1=1`
- Variables SQL: `@@version`, `@variable`
- Procedimientos del sistema: `xp_`, `sp_`

## 🔒 Capas de Seguridad Adicionales Recomendadas

1. **Rate Limiting**
   - Limita intentos de login (máximo 5 intentos en 15 minutos)

2. **Logging**
   - Registra todos los intentos fallidos de login
   - Monitorea patrones sospechosos

3. **Encriptación de Contraseñas Guardadas**
   - Actualmente se guardan en texto plano en Settings
   - Considera usar DPAPI para Windows

4. **HTTPS/TLS**
   - Si la aplicación se comunica con un servidor, usa siempre HTTPS

5. **Validación en el Backend**
   - NUNCA confíes solo en validaciones del cliente
   - Valida también en el servidor/API

## 📝 Ejemplos de Uso

### Validar antes de insertar en base de datos:
```csharp
string usuario = txtUsuario.Text.Trim();

// Validar formato
if (!SecurityHelper.IsValidUsername(usuario))
{
    MessageBox.Show("Usuario inválido");
    return;
}

// Validar inyección SQL
if (!SecurityHelper.IsSqlInjectionSafe(usuario))
{
    MessageBox.Show("Caracteres no permitidos");
    return;
}

// Sanitizar (opcional, pero recomendado)
usuario = SecurityHelper.SanitizeInput(usuario);

// Ahora puedes usar usuario de forma segura con consultas parametrizadas
```

## 🚨 IMPORTANTE

Estas validaciones son una **primera línea de defensa**, pero NO son suficientes por sí solas:

1. **Siempre usa consultas parametrizadas** - Es la protección más importante
2. **Valida en cliente Y servidor** - No confíes solo en el cliente
3. **Mantén actualizado** - Actualiza patrones de detección regularmente
4. **Prueba con herramientas** - Usa herramientas como SQLMap para probar tu seguridad

## 📚 Recursos Adicionales

- [OWASP SQL Injection Prevention Cheat Sheet](https://cheatsheetseries.owasp.org/cheatsheets/SQL_Injection_Prevention_Cheat_Sheet.html)
- [Microsoft - SQL Injection](https://docs.microsoft.com/en-us/sql/relational-databases/security/sql-injection)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
