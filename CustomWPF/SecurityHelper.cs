using System;
using System.Text.RegularExpressions;

namespace CustomWPF
{
    /// <summary>
    /// Clase helper para validaciones de seguridad
    /// </summary>
    public static class SecurityHelper
    {
        // Patrones peligrosos comunes en inyección SQL
        private static readonly string[] SqlInjectionPatterns = new string[]
        {
            @"(\s|^)(ALTER|CREATE|DELETE|DROP|EXEC(UTE)?|INSERT(\s+INTO)?|MERGE|SELECT|UPDATE|UNION(\s+ALL)?)\s",
            @"--",
            @";",
            @"/\*",
            @"\*/",
            @"xp_",
            @"sp_",
            @"0x[0-9a-fA-F]+",
            @"CHAR\(",
            @"NCHAR\(",
            @"VARCHAR\(",
            @"NVARCHAR\(",
            @"CAST\(",
            @"CONVERT\(",
            @"@@",
            @"@\w+",
            @"'(\s|$)",
            @"('')+",
            @"OR\s+[\d\w]+\s*=\s*[\d\w]+",
            @"AND\s+[\d\w]+\s*=\s*[\d\w]+"
        };

        /// <summary>
        /// Valida si el input contiene patrones de inyección SQL
        /// </summary>
        /// <param name="input">Texto a validar</param>
        /// <returns>True si el input es seguro, False si contiene patrones peligrosos</returns>
        public static bool IsSqlInjectionSafe(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return true;

            foreach (var pattern in SqlInjectionPatterns)
            {
                if (Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Valida formato de usuario (solo letras, números y guiones bajos)
        /// </summary>
        /// <param name="username">Nombre de usuario</param>
        /// <returns>True si el formato es válido</returns>
        public static bool IsValidUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return false;

            // Solo permite letras, números, guiones bajos y puntos
            // Longitud entre 3 y 30 caracteres
            var pattern = @"^[a-zA-Z0-9_.]{3,30}$";
            return Regex.IsMatch(username, pattern);
        }

        /// <summary>
        /// Valida formato de email
        /// </summary>
        /// <param name="email">Correo electrónico</param>
        /// <returns>True si el formato es válido</returns>
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                return Regex.IsMatch(email, pattern);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Valida la fortaleza de la contraseña
        /// </summary>
        /// <param name="password">Contraseña a validar</param>
        /// <param name="minLength">Longitud mínima (por defecto 6)</param>
        /// <returns>True si la contraseña cumple los requisitos mínimos</returns>
        public static bool IsStrongPassword(string password, int minLength = 6)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            if (password.Length < minLength)
                return false;

            // Opcional: Requiere al menos una letra y un número
            bool hasLetter = Regex.IsMatch(password, @"[a-zA-Z]");
            bool hasDigit = Regex.IsMatch(password, @"\d");

            return hasLetter && hasDigit;
        }

        /// <summary>
        /// Sanitiza el input removiendo caracteres peligrosos
        /// </summary>
        /// <param name="input">Texto a sanitizar</param>
        /// <returns>Texto sanitizado</returns>
        public static string SanitizeInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            // Remover caracteres potencialmente peligrosos
            input = input.Replace("'", "");
            input = input.Replace("\"", "");
            input = input.Replace(";", "");
            input = input.Replace("--", "");
            input = input.Replace("/*", "");
            input = input.Replace("*/", "");
            input = input.Replace("xp_", "");
            input = input.Replace("sp_", "");

            return input.Trim();
        }

        /// <summary>
        /// Limita la longitud del input
        /// </summary>
        /// <param name="input">Texto a limitar</param>
        /// <param name="maxLength">Longitud máxima</param>
        /// <returns>Texto limitado</returns>
        public static string LimitLength(string input, int maxLength)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return input.Length > maxLength ? input.Substring(0, maxLength) : input;
        }
    }
}
