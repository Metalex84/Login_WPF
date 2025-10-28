using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using CustomWPF.Data;
using CustomWPF.Models;

namespace CustomWPF.Services
{
    /// <summary>
    /// Servicio de autenticación y gestión de usuarios
    /// </summary>
    public class AuthService
    {
        /// <summary>
        /// Registra un nuevo usuario en la base de datos
        /// </summary>
        public bool RegistrarUsuario(string nombreUsuario, string email, string password, out string mensaje)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    // Verificar si el usuario ya existe
                    if (context.Usuarios.Any(u => u.NombreUsuario == nombreUsuario))
                    {
                        mensaje = "El nombre de usuario ya está en uso.";
                        return false;
                    }

                    // Verificar si el email ya existe
                    if (context.Usuarios.Any(u => u.Email == email))
                    {
                        mensaje = "El correo electrónico ya está registrado.";
                        return false;
                    }

                    // Crear nuevo usuario
                    var nuevoUsuario = new Usuario
                    {
                        NombreUsuario = nombreUsuario,
                        Email = email,
                        PasswordHash = HashPassword(password),
                        FechaCreacion = DateTime.Now,
                        Activo = true
                    };

                    context.Usuarios.Add(nuevoUsuario);
                    context.SaveChanges();

                    mensaje = "Usuario registrado exitosamente.";
                    return true;
                }
            }
            catch (Exception ex)
            {
                mensaje = $"Error al registrar usuario: {ex.Message}";
                return false;
            }
        }

        /// <summary>
        /// Valida las credenciales del usuario
        /// </summary>
        public bool ValidarCredenciales(string nombreUsuario, string password, out string mensaje)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var usuario = context.Usuarios
                        .FirstOrDefault(u => u.NombreUsuario == nombreUsuario && u.Activo);

                    if (usuario == null)
                    {
                        mensaje = "Usuario no encontrado o inactivo.";
                        return false;
                    }

                    // Verificar contraseña
                    if (VerificarPassword(password, usuario.PasswordHash))
                    {
                        // Actualizar fecha de último acceso
                        usuario.FechaUltimoAcceso = DateTime.Now;
                        context.SaveChanges();

                        mensaje = "Inicio de sesión exitoso.";
                        return true;
                    }
                    else
                    {
                        mensaje = "Contraseña incorrecta.";
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                mensaje = $"Error al validar credenciales: {ex.Message}";
                return false;
            }
        }

        /// <summary>
        /// Obtiene información del usuario
        /// </summary>
        public Usuario ObtenerUsuario(string nombreUsuario)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    return context.Usuarios
                        .FirstOrDefault(u => u.NombreUsuario == nombreUsuario);
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Hash de contraseña usando SHA256
        /// En producción considera usar BCrypt.Net o Argon2
        /// </summary>
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        /// <summary>
        /// Verifica si la contraseña coincide con el hash
        /// </summary>
        private bool VerificarPassword(string password, string hash)
        {
            string passwordHash = HashPassword(password);
            return passwordHash.Equals(hash, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Verifica si la conexión a la base de datos funciona
        /// </summary>
        public bool ProbarConexion(out string mensaje)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    // Intenta acceder a la base de datos
                    var count = context.Usuarios.Count();
                    mensaje = $"Conexión exitosa. Usuarios registrados: {count}";
                    return true;
                }
            }
            catch (Exception ex)
            {
                mensaje = $"Error de conexión: {ex.Message}";
                return false;
            }
        }
    }
}
