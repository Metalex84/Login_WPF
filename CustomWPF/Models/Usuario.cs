using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomWPF.Models
{
    /// <summary>
    /// Entidad Usuario para la base de datos
    /// </summary>
    [Table("Usuarios")]
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUsuario { get; set; }

        [Required]
        [StringLength(30)]
        [Index(IsUnique = true)]
        public string NombreUsuario { get; set; }

        [Required]
        [StringLength(100)]
        [Index(IsUnique = true)]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime? FechaUltimoAcceso { get; set; }

        public bool Activo { get; set; }

        public Usuario()
        {
            FechaCreacion = DateTime.Now;
            Activo = true;
        }
    }
}
