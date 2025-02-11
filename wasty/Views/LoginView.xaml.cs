// Definición de la ventana de inicio de sesión para Wasty. Hereda de Window e inicializa los componentes de la interfaz en el constructor.
// Contiene un manejador de eventos vacío para el clic del botón.

using System.Windows;

namespace wasty.Views
{
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
