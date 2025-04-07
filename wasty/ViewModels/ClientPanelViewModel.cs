using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using wasty.Services;
using wasty.Views;
using wasty.Models;
using System.DirectoryServices;
using System.Net.Http;
using System.Text.Json;
using System.Windows.Controls;
using System.Windows;

namespace wasty.ViewModels
{
    public class ClientPanelViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;
        private readonly NavigationService _navigationService;

        private string _searchText;
        private ClienteModel _cliente;
        private ObservableCollection<ClienteModel> _clientes;
        private ObservableCollection<ClienteModel> _clientesFiltrados = new ObservableCollection<ClienteModel>();

        public event PropertyChangedEventHandler PropertyChanged;

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                    FiltrarClientes();
            }
        }

        private bool _isDropdownVisible;
        public bool IsDropdownVisible
        {
            get { return _isDropdownVisible; }
            set { _isDropdownVisible = value; OnPropertyChanged(); }
        }

        private string _searchAddress;
        public string SearchAddress
        {
            get { return _searchAddress; }
            set
            {
                _searchAddress = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<LocationModel> _searchAddressResults = new ObservableCollection<LocationModel>();
        public ObservableCollection<LocationModel> SearchAddressResults
        {
            get { return _searchAddressResults; }
            set
            {
                _searchAddressResults = value;
                OnPropertyChanged();
            }
        }

        private LocationModel _selectedAddressResult;
        public LocationModel SelectedAddressResult
        {
            get { return _selectedAddressResult; }
            set
            {
                if (value is not null)
                {
                    _selectedAddressResult = value;
                    SearchAddress = _selectedAddressResult.Address;
                    IsDropdownVisible = false;
                    OnPropertyChanged();
                }
            }
        }

        public ClienteModel Cliente
        {
            get => _cliente;
            set
            {
                if (SetProperty(ref _cliente, value))
                    FiltrarClientes();
            }
        }

        public ObservableCollection<ClienteModel> Clientes
        {
            get => _clientes;
            set
            {
                if (SetProperty(ref _clientes, value))
                    FiltrarClientes();
            }
        }

        public ObservableCollection<ClienteModel> ClientesFiltrados
        {
            get => _clientesFiltrados;
            set => SetProperty(ref _clientesFiltrados, value);
        }

        // --- VISIBILIDAD DINÁMICA PARA GROUPBOXES ---

        private Dictionary<string, bool> _groupBoxVisibility = new();
        public Dictionary<string, bool> GroupBoxVisibility
        {
            get => _groupBoxVisibility;
            set
            {
                _groupBoxVisibility = value;
                OnPropertyChanged();
            }
        }

        public ICommand ToggleGroupBoxVisibilityCommand { get; }
        public ICommand ShowAllGroupBoxesCommand { get; }

        // --------------------------------------------

        public ICommand CreateCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DisableCommand { get; }
        public ICommand LoadClientCommand { get; }

        public ICommand StartSearchCommand { get; }

        public ClientPanelViewModel(ApiService apiService, NavigationService navigationService)
        {
            _apiService = apiService;
            _navigationService = navigationService;

            CreateCommand = new RelayCommand(CreateClient);
            UpdateCommand = new RelayCommand(UpdateClient);
            DisableCommand = new RelayCommand(DisableClient);
            LoadClientCommand = new RelayCommand(LoadClient);

            StartSearchCommand = new RelayCommand(HandleSearchInputWithDelay);

            ToggleGroupBoxVisibilityCommand = new RelayCommand<string>(ToggleGroupBoxVisibility);
            ShowAllGroupBoxesCommand = new RelayCommand<string>(ShowAllGroupBoxes);

            InicializarVisibilidad();
        }

        public async Task Init(ClienteModel cliente)
        {
            Cliente = cliente;
        }

        private void InicializarVisibilidad()
        {
            var keys = new[]
            {
                "DatosClientes", "Online", "DatosGenerales", "Horario", "Ubicacion",
                "Contacto", "Comercial", "Observaciones", "DatosPagos", "DatosPrecios",
                "NIMA", "DatosNIMA", "MedioAmbiente", "Legalidad", "Recogidas",
                "DatosRecogidas", "NoPeligrosas", "Peligrosas", "Aceite"
            };

            foreach (var key in keys)
                GroupBoxVisibility[key] = true;
        }

        private void ToggleGroupBoxVisibility(string key)
        {
            if (GroupBoxVisibility.ContainsKey(key))
            {
                GroupBoxVisibility[key] = !GroupBoxVisibility[key];
                OnPropertyChanged(nameof(GroupBoxVisibility));
            }
        }

        private void ShowAllGroupBoxes(string show)
        {
            var keys = GroupBoxVisibility.Keys.ToList();
            foreach (var key in keys)
                GroupBoxVisibility[key] = true;

            OnPropertyChanged(nameof(GroupBoxVisibility));
        }

        private void CreateClient(object obj) { /* lógica crear */ }
        private void UpdateClient(object obj) { /* lógica actualizar */ }
        private void DisableClient(object obj) { /* lógica desactivar */ }

        private void LoadClient(object obj)
        {
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
                    ClientesFiltrados.Add(cliente);
            }
            else
            {
                string searchLower = SearchText.ToLower();
                var filtrados = Clientes.Where(c =>
                    (!string.IsNullOrEmpty(c.NombreFiscal) && c.NombreFiscal.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(c.NombreComercial) && c.NombreComercial.ToLower().Contains(searchLower))
                );

                foreach (var cliente in filtrados)
                    ClientesFiltrados.Add(cliente);
            }
        }

        private async void HandleSearchInputWithDelay(object obj)
        {
            SearchAddressResults.Clear();
            try
            {
                if (!string.IsNullOrWhiteSpace(SearchAddress))
                {
                    var result = await _apiService.RequestAsync("GET", $"geo/location/{SearchAddress}", "");
                    SearchAddressResults = JsonSerializer.Deserialize<ObservableCollection<LocationModel>>(result.datos);
                }
            }
            catch (Exception ex)
            {
            }

            if (SearchAddressResults.Count > 0)
                IsDropdownVisible = true;
            //if (Keyboard.Modifiers != ModifierKeys.None)
            //{
            //    return;
            //}

            //_lastKeyPressTime = DateTime.Now;
            //await Task.Delay(SearchDelayMilliseconds);

            //if ((DateTime.Now - _lastKeyPressTime).TotalMilliseconds >= SearchDelayMilliseconds)
            //{
            //    if (!string.IsNullOrWhiteSpace(SearchText) && SearchText.Length >= 2)
            //    {
            //        await FetchSearchResults(SearchText);
            //    }
            //    else
            //    {
            //        SearchResults.Clear();
            //        IsDropdownVisible = false;
            //    }
            //}
        }


        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(storage, value))
            {
                storage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }
            return false;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
