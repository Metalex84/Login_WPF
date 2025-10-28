using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CustomWPF.Data;

namespace CustomWPF
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                // Configurar Entity Framework para crear la base de datos si no existe
                Database.SetInitializer(new CreateDatabaseIfNotExists<AppDbContext>());

                // Inicializar la base de datos
                using (var context = new AppDbContext())
                {
                    // Esto forzará la creación de la BD si no existe
                    context.Database.Initialize(force: false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al inicializar la base de datos:\n\n{ex.Message}\n\nRevisa MANUAL_DB_SETUP.md para configuración manual.",
                    "Error de Base de Datos",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }
    }
}
