using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using wasty.Utils;
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
        private void Importar_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ImportDialog
            {
                Owner = Window.GetWindow(this)
            };
            dialog.ShowDialog();
        }

        private void ToggleFiltros_Click(object sender, RoutedEventArgs e)
        {
            if (PanelFiltros.Visibility != Visibility.Visible)
            {
                PanelFiltros.Visibility = Visibility.Visible;
                var sb = (Storyboard)Resources["MostrarPanelAnimado"];
                sb.Begin(PanelFiltros);
            }
            else
            {
                var sb = (Storyboard)Resources["OcultarPanelAnimado"];
                sb.Completed += (s, _) => PanelFiltros.Visibility = Visibility.Collapsed;
                sb.Begin(PanelFiltros);
            }
        }
    }
}
