// Definici�n de la ventana de inicio de sesi�n para Wasty. Hereda de Window e inicializa los componentes de la interfaz en el constructor.
// Contiene un manejador de eventos vac�o para el clic del bot�n.

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
