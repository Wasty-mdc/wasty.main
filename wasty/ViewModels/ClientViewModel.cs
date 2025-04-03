using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using wasty.Services;
using wasty.Views;
using System.Collections.ObjectModel;
using wasty.Models;
using System.Text.Json;

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

        private ObservableCollection<ClienteModel> _clientes;
        public ObservableCollection<ClienteModel> Clientes
        {
            get => _clientes;
            set
            {
                _clientes = value;
                OnPropertyChanged(nameof(Clientes));
            }
        }

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
        public ICommand NavigateToClientPanelCommand { get; }

        private void NavigateToClientPanel(object parameter)
        {
            _navigationService.NavigateTo<ClientPanelView>(parameter);
        }

        private async Task Init()
        {
            Clientes = await GetData();
        }

        private async Task<ObservableCollection<ClienteModel>> GetData()
        {
            JsonElement itemsElement = default;
            string items = "";
            try
            {
                var result = await _apiService.RequestAsync("GET", "clientes", "");

                var itemsList = JsonSerializer.Deserialize<ObservableCollection<ClienteModel>>(result.datos);

                return itemsList;
            }
            catch (Exception ex)
            {
                return new ObservableCollection<ClienteModel>();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
