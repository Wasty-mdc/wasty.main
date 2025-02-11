// Definición de la ventana de registro de usuario para Wasty.
// Hereda de Window e inicializa los componentes de la interfaz en el constructor.
// Contiene manejadores de eventos vacíos para interacciones con botones y cambios en los campos de texto.

using System.Windows;

namespace wasty.Views
{
    public partial class SignupView : Window
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
    }
}
