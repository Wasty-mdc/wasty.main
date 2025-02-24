using System.Windows;
using System.Windows.Controls;
using wasty.ViewModels;

namespace wasty.Views
{
    public partial class SignupView : UserControl
    {
        public SignupView()
        {
            InitializeComponent();
            // Establecer el DataContext al SignupViewModel
            DataContext = ((App)Application.Current).Services.GetService(typeof(SignupViewModel));
        }

        // Evento para manejar el cambio de contraseña en el PasswordBox
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                // Actualizar la propiedad Contrasenia en el ViewModel
                ((SignupViewModel)this.DataContext).Contrasenia = ((PasswordBox)sender).Password;
            }
        }

        // Evento para manejar el cambio de confirmación de contraseña en el PasswordBox
        private void PasswordBox_ConfirmPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                // Actualizar la propiedad ConfirmarContrasenia en el ViewModel
                ((SignupViewModel)this.DataContext).ConfirmarContrasenia = ((PasswordBox)sender).Password;
            }
        }
    }
}