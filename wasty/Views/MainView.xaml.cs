using System.Windows;
using System.Windows.Controls;
using wasty.Services;
using wasty.Views.TableViews;

namespace wasty.Views
{
    public partial class MainView : UserControl
    {
        private readonly NavigationService _navigationService;

        public MainView()
        {
            InitializeComponent();
            DataContext = ((App)Application.Current).Services.GetService(typeof(MainViewModel));
            _navigationService = ((App)Application.Current).Services.GetService(typeof(NavigationService)) as NavigationService;
        }

        private void Clientes_Click(object sender, RoutedEventArgs e)
        {
            _navigationService?.NavigateTo<ClientTableView>();
        }

        private void Residuos_Click(object sender, RoutedEventArgs e)
        {
            _navigationService?.NavigateTo<RecycTableView>();
        }

        private void Ficheros_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Ficheros seleccionado");
        }

        private void Gestion_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Gestión seleccionada");
        }

        private void Facturacion_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Facturación seleccionada");
        }
    }
}
