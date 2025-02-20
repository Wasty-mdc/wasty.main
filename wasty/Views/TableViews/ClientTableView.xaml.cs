using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using wasty.ViewModels;

namespace wasty.Views.TableViews
{
    public partial class ClientTableView : UserControl
    {
        public ClientTableView()
        {
            InitializeComponent();
            DataContext = ((App)Application.Current).Services.GetService(typeof(ClientTableViewModel));
        }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var viewModel = DataContext as ClientTableViewModel;
            if (viewModel != null && !viewModel.IsColumnVisible(e.PropertyName))
            {
                e.Column.Visibility = Visibility.Collapsed;
            }
        }
        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow.DataContext is MainViewModel mainViewModel)
            {
                mainViewModel.NavigateToMainView();
            }
        }


    }
}
