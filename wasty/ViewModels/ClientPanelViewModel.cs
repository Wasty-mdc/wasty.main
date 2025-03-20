using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using wasty.Services;
using wasty.Views;
using wasty.Models;

namespace wasty.ViewModels
{
    public class ClientPanelViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;
        private readonly NavigationService _navigationService;

        private string _searchText;
        private ObservableCollection<ClienteModel> _clientes;
        private ObservableCollection<ClienteModel> _clientesFiltrados = new ObservableCollection<ClienteModel>();

        public event PropertyChangedEventHandler PropertyChanged;

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value)) // Ahora SetProperty devuelve un bool
                {
                    FiltrarClientes();
                }
            }
        }

        public ObservableCollection<ClienteModel> Clientes
        {
            get => _clientes;
            set
            {
                if (SetProperty(ref _clientes, value))
                {
                    FiltrarClientes(); // Se actualiza la lista filtrada cuando cambian los clientes
                }
            }
        }

        public ObservableCollection<ClienteModel> ClientesFiltrados
        {
            get => _clientesFiltrados;
            set => SetProperty(ref _clientesFiltrados, value);
        }

        public ICommand CreateCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DisableCommand { get; }
        public ICommand LoadClientCommand { get; }

        public ClientPanelViewModel(ApiService apiService, NavigationService navigationService)
        {
            _apiService = apiService;
            _navigationService = navigationService;

            CreateCommand = new RelayCommand(CreateClient);
            UpdateCommand = new RelayCommand(UpdateClient);
            DisableCommand = new RelayCommand(DisableClient);
            LoadClientCommand = new RelayCommand(LoadClient);
        }

        private void CreateClient(object obj) { /* Lógica para crear un cliente */ }
        private void UpdateClient(object obj) { /* Lógica para actualizar un cliente */ }
        private void DisableClient(object obj) { /* Lógica para deshabilitar un cliente */ }
        private void LoadClient(object obj)
        {
            // Simulación de carga de datos
            Clientes = new ObservableCollection<ClienteModel>
            {
                new ClienteModel { NombreFiscal = "Empresa ABC", NombreComercial = "ABC S.A." },
                new ClienteModel { NombreFiscal = "Comercial XYZ", NombreComercial = "XYZ Ltda." }
            };

            FiltrarClientes();
        }

        private void FiltrarClientes()
        {
            if (Clientes == null) return;

            ClientesFiltrados.Clear();

            if (string.IsNullOrWhiteSpace(SearchText))
            {
                foreach (var cliente in Clientes)
                {
                    ClientesFiltrados.Add(cliente);
                }
            }
            else
            {
                string searchLower = SearchText.ToLower();
                var filtrados = Clientes.Where(c =>
                    (!string.IsNullOrEmpty(c.NombreFiscal) && c.NombreFiscal.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(c.NombreComercial) && c.NombreComercial.ToLower().Contains(searchLower))
                );

                foreach (var cliente in filtrados)
                {
                    ClientesFiltrados.Add(cliente);
                }
            }
        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(storage, value))
            {
                storage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true; // Indica que el valor cambió
            }
            return false; // Indica que el valor no cambió
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
