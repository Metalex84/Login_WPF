using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CustomWPF
{
    /// <summary>
    /// Lógica de interacción para RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void btnRegistrarse_Click(object sender, RoutedEventArgs e)
        {
            // Validar que los campos no estén vacíos
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Por favor, ingrese su correo electrónico.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtEmail.Focus();
                return;
            }

            // Validar formato de email
            if (!SecurityHelper.IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Por favor, ingrese un correo electrónico válido.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtEmail.Focus();
                return;
            }

            // Validar inyección SQL en email
            if (!SecurityHelper.IsSqlInjectionSafe(txtEmail.Text))
            {
                MessageBox.Show("El correo electrónico contiene caracteres no permitidos.", "Error de Seguridad", MessageBoxButton.OK, MessageBoxImage.Error);
                txtEmail.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                MessageBox.Show("Por favor, ingrese un nombre de usuario.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtUsuario.Focus();
                return;
            }

            // Validar formato de usuario
            if (!SecurityHelper.IsValidUsername(txtUsuario.Text))
            {
                MessageBox.Show("El nombre de usuario contiene caracteres no válidos.\nSolo se permiten letras, números, puntos y guiones bajos (3-30 caracteres).", 
                    "Error de Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtUsuario.Focus();
                return;
            }

            // Validar inyección SQL en usuario
            if (!SecurityHelper.IsSqlInjectionSafe(txtUsuario.Text))
            {
                MessageBox.Show("El nombre de usuario contiene caracteres o patrones no permitidos.", "Error de Seguridad", MessageBoxButton.OK, MessageBoxImage.Error);
                txtUsuario.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtContrasena.Password))
            {
                MessageBox.Show("Por favor, ingrese una contraseña.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtContrasena.Focus();
                return;
            }

            // Validar fortaleza de contraseña
            if (!SecurityHelper.IsStrongPassword(txtContrasena.Password, 6))
            {
                MessageBox.Show("La contraseña debe tener al menos 6 caracteres y contener letras y números.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtContrasena.Focus();
                return;
            }

            // Validar inyección SQL en contraseña
            if (!SecurityHelper.IsSqlInjectionSafe(txtContrasena.Password))
            {
                MessageBox.Show("La contraseña contiene caracteres o patrones no permitidos.", "Error de Seguridad", MessageBoxButton.OK, MessageBoxImage.Error);
                txtContrasena.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtConfirmarContrasena.Password))
            {
                MessageBox.Show("Por favor, confirme su contraseña.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtConfirmarContrasena.Focus();
                return;
            }

            // Validar que las contraseñas coincidan
            if (txtContrasena.Password != txtConfirmarContrasena.Password)
            {
                MessageBox.Show("Las contraseñas no coinciden.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtConfirmarContrasena.Focus();
                return;
            }

            if (!chkTerminos.IsChecked == true)
            {
                MessageBox.Show("Debe aceptar los términos y condiciones.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Aquí iría la lógica de registro
            string email = txtEmail.Text;
            string usuario = txtUsuario.Text;
            string contrasena = txtContrasena.Password;

            MessageBox.Show($"Cuenta creada exitosamente!\n\nBienvenido, {usuario}!", "Registro Exitoso", MessageBoxButton.OK, MessageBoxImage.Information);
            
            // Opcional: Cerrar ventana de registro y volver al login
            this.Close();
        }

        private void btnVolverLogin_Click(object sender, RoutedEventArgs e)
        {
            // Volver a la ventana de login
            this.Close();
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
