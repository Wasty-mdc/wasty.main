using System.Windows;
using System.Windows.Controls;
using wasty.ViewModels;

namespace wasty.Views.TableViews
{
    public partial class ClientTableView : UserControl
    {
        public ClientTableView()
        {
            InitializeComponent();
            // Establecer el DataContext al ClientTableViewModel
            DataContext = ((App)Application.Current).Services.GetService(typeof(ClientTableViewModel));
        }

        // Evento para manejar la generación automática de columnas en el DataGrid
        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var viewModel = DataContext as ClientTableViewModel;
            if (viewModel != null && !viewModel.IsColumnVisible(e.PropertyName))
            {
                e.Column.Visibility = Visibility.Collapsed;
            }
        }
    }
}