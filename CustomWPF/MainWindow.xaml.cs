using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CustomWPF
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CargarCredencialesGuardadas();
        }

        private void CargarCredencialesGuardadas()
        {
            // Cargar credenciales guardadas si existen
            if (Properties.Settings.Default.RecordarContrasena)
            {
                txtUsuario.Text = Properties.Settings.Default.Usuario;
                txtContrasena.Password = Properties.Settings.Default.Contrasena;
                chkRecordarContrasena.IsChecked = true;
            }
        }

        private void btnEntrar_Click(object sender, RoutedEventArgs e)
        {
            // Validar que los campos no estén vacíos
            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                MessageBox.Show("Por favor, ingrese un usuario.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtUsuario.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtContrasena.Password))
            {
                MessageBox.Show("Por favor, ingrese una contraseña.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtContrasena.Focus();
                return;
            }

            // Validaciones de seguridad
            string usuario = txtUsuario.Text.Trim();
            string contrasena = txtContrasena.Password;

            // Validar formato de usuario
            if (!SecurityHelper.IsValidUsername(usuario))
            {
                MessageBox.Show("El nombre de usuario contiene caracteres no válidos.\nSolo se permiten letras, números, puntos y guiones bajos (3-30 caracteres).", 
                    "Error de Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtUsuario.Focus();
                return;
            }

            // Validar inyección SQL
            if (!SecurityHelper.IsSqlInjectionSafe(usuario) || !SecurityHelper.IsSqlInjectionSafe(contrasena))
            {
                MessageBox.Show("Se han detectado caracteres o patrones no permitidos en los campos.", 
                    "Error de Seguridad", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Aquí iría la lógica de autenticación
            // Los valores ya están validados y sanitizados arriba

            // Guardar o eliminar credenciales según el checkbox
            if (chkRecordarContrasena.IsChecked == true)
            {
                Properties.Settings.Default.Usuario = usuario;
                Properties.Settings.Default.Contrasena = contrasena;
                Properties.Settings.Default.RecordarContrasena = true;
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.Usuario = string.Empty;
                Properties.Settings.Default.Contrasena = string.Empty;
                Properties.Settings.Default.RecordarContrasena = false;
                Properties.Settings.Default.Save();
            }

            MessageBox.Show($"Bienvenido, {usuario}!", "Login Exitoso", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnRegistrarse_Click(object sender, RoutedEventArgs e)
        {
            // Abrir ventana de registro
            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.ShowDialog();
        }

        // Manejadores de eventos para los botones de ventana
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

    }
}
