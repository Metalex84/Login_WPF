using System.Data.Entity;
using CustomWPF.Models;

namespace CustomWPF.Data
{
    /// <summary>
    /// Contexto de base de datos usando Entity Framework
    /// </summary>
    public class AppDbContext : DbContext
    {
        // Constructor que usa la conexión definida en App.config
        public AppDbContext() : base("name=LoginDbConnection")
        {
        }

        // DbSet para la tabla Usuarios
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuraciones adicionales del modelo si es necesario
            modelBuilder.Entity<Usuario>()
                .Property(u => u.NombreUsuario)
                .IsRequired()
                .HasMaxLength(30);

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Usuario>()
                .Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(255);

            // Índices únicos
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.NombreUsuario)
                .IsUnique();

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}
