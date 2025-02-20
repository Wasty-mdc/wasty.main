using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using wasty.ViewModels;

namespace wasty.Views.TableViews
{
    /// <summary>
    /// Lógica de interacción para RecycTableView.xaml
    /// </summary>
    public partial class RecycTableView : UserControl
    {
        public RecycTableView()
        {
            InitializeComponent();
            DataContext = ((App)Application.Current).Services.GetService(typeof(RecycTableViewModel));

        }

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
