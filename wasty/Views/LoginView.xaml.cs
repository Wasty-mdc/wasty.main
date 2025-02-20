// Definici�n de la ventana de inicio de sesi�n para Wasty. Hereda de Window e inicializa los componentes de la interfaz en el constructor.
// Contiene un manejador de eventos vac�o para el clic del bot�n.

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

            DataContext = ((App)Application.Current).Services.GetService(typeof(LoginViewModel));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RegisterTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((LoginViewModel)this.DataContext).Contrasenia = ((PasswordBox)sender).Password; }
        }

        private void NewRegisterButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}