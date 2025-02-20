using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using wasty.ViewModels;

namespace wasty.Views.TableViews

{
    public partial class TableView : UserControl
    {
        public TableView()
        {
            InitializeComponent();
            DataContext = new ClientTableViewModel();
        }

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
