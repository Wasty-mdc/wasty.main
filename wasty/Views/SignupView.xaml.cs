// Definicion de la ventana de registro de usuario para Wasty.
// Hereda de Window e inicializa los componentes de la interfaz en el constructor.
// Contiene manejadores de eventos vacios para interacciones con botones y cambios en los campos de texto.

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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void SignupNameTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((SignupViewModel)this.DataContext).Contrasenia = ((PasswordBox)sender).Password; }
        }

        private void PasswordBox_ConfirmPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((SignupViewModel)this.DataContext).ConfirmarContrasenia = ((PasswordBox)sender).Password; }
        }
    }
}