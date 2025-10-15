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

            if (!chkTerminos.IsChecked == true)
            {
                MessageBox.Show("Debe aceptar los términos y condiciones.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Aquí iría la lógica de autenticación
            string usuario = txtUsuario.Text;
            string contrasena = txtContrasena.Password;

            MessageBox.Show($"Bienvenido, {usuario}!", "Login Exitoso", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnRegistrarse_Click(object sender, RoutedEventArgs e)
        {
            // Aquí iría la lógica para abrir el formulario de registro
            MessageBox.Show("Funcionalidad de registro no implementada aún.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void chkTerminos_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
