using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Input;
using wasty.Services;
using wasty.Views;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace wasty.ViewModels
{
    public class ClientTableViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;
        private readonly NavigationService _navigationService;
        public ICommand ToggleStickyColumnCommand { get; }
        public ICommand ToggleColumnVisibilityCommand { get; }
        public ICommand VolverCommand { get; }

        private Dictionary<string, bool> _stickyColumns;
        private Dictionary<string, bool> _hiddenColumns;
        private int _frozenColumnCount;
        private ObservableCollection<Cliente> _clientes;

        public ObservableCollection<Cliente> Clientes
        {
            get => _clientes;
            set
            {
                _clientes = value;
                OnPropertyChanged();
            }
        }

        public int FrozenColumnCount
        {
            get => _frozenColumnCount;
            set
            {
                _frozenColumnCount = value;
                OnPropertyChanged();
            }
        }

        public ClientTableViewModel(ApiService apiService, NavigationService navigationService)
        {
            _apiService = apiService;
            _navigationService = navigationService;
            _stickyColumns = new Dictionary<string, bool>();
            _hiddenColumns = new Dictionary<string, bool>();
            Clientes = new ObservableCollection<Cliente>();
            ToggleStickyColumnCommand = new RelayCommand<string>(ToggleStickyColumn);
            ToggleColumnVisibilityCommand = new RelayCommand<string>(ToggleColumnVisibility); 
            VolverCommand = new RelayCommand(_ => _navigationService.NavigateTo<MainView>());
            Init().GetAwaiter();
        }

        private async Task Init()
        {
            Clientes = await GetData();
        }

        private void ToggleStickyColumn(string columnName)
        {
            if (string.IsNullOrEmpty(columnName)) return;

            if (_stickyColumns.ContainsKey(columnName))
            {
                _stickyColumns[columnName] = !_stickyColumns[columnName];
            }
            else
            {
                _stickyColumns[columnName] = true;
            }

            UpdateFrozenColumns();
        }

        private void UpdateFrozenColumns()
        {
            FrozenColumnCount = _stickyColumns.Values.Count(v => v);
        }

        private void ToggleColumnVisibility(string columnName)
        {
            if (string.IsNullOrEmpty(columnName)) return;

            if (_hiddenColumns.ContainsKey(columnName))
            {
                _hiddenColumns[columnName] = !_hiddenColumns[columnName];
            }
            else
            {
                _hiddenColumns[columnName] = true;
            }
            OnPropertyChanged("HiddenColumns");
        }

        public bool IsColumnVisible(string columnName)
        {
            return !_hiddenColumns.TryGetValue(columnName, out bool isHidden) || !isHidden;
        }


        private async Task<ObservableCollection<Cliente>> GetData()
        {
            JsonElement tokenElement = default;
            JsonElement clientesElement = default;
            string token = "";
            string clientes = "";
            var login = new
            {
                Email = "Pruebas123@pruebas.com",
                Contrasenia = "Pruebas123."
            };

            var auth = await _apiService.RequestAsync("POST", "auth/login", login);

            if (auth.TryGetProperty("datos", out JsonElement datosElement) && datosElement.TryGetProperty("token", out tokenElement))
                token = tokenElement.GetString();

            var result = await _apiService.RequestAsync("GET", "clientes", "", token);

            if (result.TryGetProperty("datos", out clientesElement))
                clientes = clientesElement.GetRawText();

            try
            {
                ObservableCollection<Cliente> clientesList = JsonSerializer.Deserialize<ObservableCollection<Cliente>>(clientes);

                return clientesList;
            }
            catch (Exception ex)
            {
                return new ObservableCollection<Cliente>();
            }

            //    new Cliente { Codigo = "012", NombreComercial = "Empresa L", NombreFiscal = "Empresa L. Cooperativa",
            //                  NIF = "55667788Q", Fiscalidad = "Cooperativa", Domicilio = "Avenida del Cid 55", CodigoPostal = "46190",
            //                  Provincia = "Valencia", Poblacion = "Riba-roja de Túria", Zona = "07 Riba-roja", Telefono = "962889900",
            //                  Email = "empresaL@example.com", TituloRelacion = "Cliente", Comercial = "Isabel Moreno", Estado = "Activo" }

            //};
        }

        public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    public class Cliente
    {
        [JsonPropertyName("codCliente")]
        public int Codigo { get; set; }
        [JsonPropertyName("nombreComercial")]
        public string NombreComercial { get; set; }
        [JsonPropertyName("nombreFiscal")]
        public string NombreFiscal { get; set; }
        [JsonPropertyName("nif")]
        public string NIF { get; set; }
      //  public string Fiscalidad { get; set; } = "";
      //  public string Domicilio { get; set; } = "";
      //  public string CodigoPostal { get; set; } = "";
      //  public string Provincia { get; set; } = "";
      //  public string Poblacion { get; set; } = "";
      //  public string Zona { get; set; } = "";
        [JsonPropertyName("telefono")]
        public string Telefono { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
       // public string TituloRelacion { get; set; } = "";
       // public string Comercial { get; set; } = "";
        public string Estado { get; set; } = "Activo";
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Predicate<T> _canExecute;

        public RelayCommand(Action<T> execute, Predicate<T> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
