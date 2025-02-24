using System.Windows;
using System.Windows.Controls;
using wasty.ViewModels;

namespace wasty.Views
{
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
            // Establecer el DataContext al LoginViewModel
            DataContext = ((App)Application.Current).Services.GetService(typeof(LoginViewModel));
        }

        // Evento para manejar el cambio de contraseña en el PasswordBox
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                // Actualizar la propiedad Contrasenia en el ViewModel
                ((LoginViewModel)this.DataContext).Contrasenia = ((PasswordBox)sender).Password;
            }
        }
    }
}