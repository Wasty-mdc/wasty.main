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

namespace wasty.Views
{
    /// <summary>
    /// Lógica de interacción para ClientView.xaml
    /// </summary>
    public partial class ClientView : UserControl
    {
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
            DataContext = ((App)Application.Current).Services.GetService(typeof(ClientViewModel));
        }
    }
}
