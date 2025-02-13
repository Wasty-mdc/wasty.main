// MainView - Lógica de interacción para la vista principal de Wasty

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

        // Evento de clic para el botón Ficheros
        private void Ficheros_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Ficheros seleccionado");
        }

        // Evento de clic para el botón Gestión
        private void Gestion_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Gestión seleccionada");
        }

        // Evento de clic para el botón Facturación
        private void Facturacion_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Facturación seleccionada");
        }
    }
}
