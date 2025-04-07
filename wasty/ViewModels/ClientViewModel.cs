using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using wasty.Models;
using wasty.Services;
using wasty.Utils;
using wasty.Views;

namespace wasty.ViewModels
{
    public class ClientViewModel : INotifyPropertyChanged
    {
        private readonly NavigationService _navigationService;
        private readonly ApiService _apiService;

        private string _clienteNombre = "METALLS DEL CAMP. SLU.";
        public string ClienteNombre
        {
            get => _clienteNombre;
            set
            {
                _clienteNombre = value;
                OnPropertyChanged();
            }
        }

        public Paginador<ClienteModel> PaginadorClientes { get; private set; }

        public ICommand NavigateToClientPanelCommand { get; }

        private ClienteModel _clienteSeleccionado;
        public ClienteModel ClienteSeleccionado
        {
            get => _clienteSeleccionado;
            set
            {
                _clienteSeleccionado = value;
                OnPropertyChanged(nameof(ClienteSeleccionado));
            }
        }

        public ClientViewModel(NavigationService navigationService, ApiService apiService)
        {
            _navigationService = navigationService;
            _apiService = apiService;

            NavigateToClientPanelCommand = new RelayCommand<object>(NavigateToClientPanel);
            Init().GetAwaiter();
        }

        private void NavigateToClientPanel(object parameter)
        {
            _navigationService.NavigateTo<ClientPanelView>(parameter);
        }

        private async Task Init()
        {
            var clientes = await GetData();
            PaginadorClientes = new Paginador<ClienteModel>(clientes, 25); // 10 por página
            OnPropertyChanged(nameof(PaginadorClientes));
        }

        private async Task<ObservableCollection<ClienteModel>> GetData()
        {
            try
            {
                var result = await _apiService.RequestAsync("GET", "clientes", "");
                var itemsList = JsonSerializer.Deserialize<ObservableCollection<ClienteModel>>(result.datos);
                return itemsList ?? new ObservableCollection<ClienteModel>();
            }
            catch (Exception)
            {
                return new ObservableCollection<ClienteModel>();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
