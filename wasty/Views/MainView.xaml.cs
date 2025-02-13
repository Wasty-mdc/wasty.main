// MainView - L�gica de interacci�n para la vista principal de Wasty

using System.Windows;
using System.Windows.Controls;

namespace wasty.Views
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
            DataContext = ((App)Application.Current).Services.GetService(typeof(MainViewModel));
        }

        // Evento de clic para el bot�n Ficheros
        private void Ficheros_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Ficheros seleccionado");
        }

        // Evento de clic para el bot�n Gesti�n
        private void Gestion_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Gesti�n seleccionada");
        }

        // Evento de clic para el bot�n Facturaci�n
        private void Facturacion_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Facturaci�n seleccionada");
        }
    }
}
