using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using wasty.ViewModels;

namespace wasty.Views
{
    /// <summary>
    /// Lógica de interacción para ClientView.xaml
    /// </summary>
    public partial class ClientView : UserControl
    {
        public ClientView()
        {
            InitializeComponent();
            DataContext = ((App)Application.Current).Services.GetService(typeof(ClientViewModel));
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is ClientViewModel vm)
            {
                var selectedItem = vm.ClienteSeleccionado;
                if (selectedItem != null && vm.NavigateToClientPanelCommand.CanExecute(selectedItem))
                {
                    vm.NavigateToClientPanelCommand.Execute(selectedItem);
                }
            }
        }

        private void ClientView_Loaded(object sender, RoutedEventArgs e)
        {
            AjustarFilasDataGrid();
        }


        private void AjustarFilasDataGrid()
        {
            if (DataContext is ClientViewModel vm)
            {
                double alturaDisponible = ActualHeight - 250; // Ajusta según tu layout
                double altoFila = 35;
                int filasCalculadas = Math.Max(1, (int)(alturaDisponible / altoFila));

                if (vm.PaginadorClientes != null)
                    vm.PaginadorClientes.ItemsPorPagina = filasCalculadas;
            }
        }
        private void ClientView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            AjustarFilasDataGrid();
        }


        private void Buscador_LostFocus(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as ClientViewModel;
            if (vm == null) return;

            if (string.IsNullOrWhiteSpace(vm.TextoBusqueda))
            {
                vm.TextoBusqueda = string.Empty; // Por si hay espacios, los borra
                vm.ResetClientes();
            }
        }


        public ClientView()
        {
            InitializeComponent();
            SizeChanged += ClientView_SizeChanged;
            DataContext = ((App)Application.Current).Services.GetService(typeof(ClientViewModel));
        }
    }
}
