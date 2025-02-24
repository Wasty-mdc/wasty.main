using System.Windows;
using System.Windows.Controls;
using wasty.ViewModels;

namespace wasty.Views.TableViews
{
    public partial class RecycTableView : UserControl
    {
        public RecycTableView()
        {
            InitializeComponent();
            // Establecer el DataContext al RecycTableViewModel
            DataContext = ((App)Application.Current).Services.GetService(typeof(RecycTableViewModel));
        }

        // Evento para manejar la generación automática de columnas en el DataGrid
        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var viewModel = DataContext as RecycTableViewModel;
            if (viewModel != null && !viewModel.IsColumnVisible(e.PropertyName))
            {
                e.Column.Visibility = Visibility.Collapsed;
            }
        }
    }
}