using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using wasty.Services;
using wasty.Views;

namespace wasty.ViewModels
{
    public class ClientPanelViewModel : INotifyPropertyChanged
    {

        private readonly ApiService _apiService;
        private readonly NavigationService _navigationService;
        private string _nombreFiscal;
        private string _nombreComercial;
        private string _nif;
        private string _tipoCliente;
        private string _tipoClienteSecundario;
        private string _direccionFiscal;
        private string _direccionRecogida;
        private string _codigoPostal;
        private string _poblacion;
        private string _provincia;
        private string _pais;
        private string _telefono;
        private string _movil;
        private string _email;
        private string _fax;
        private string _personaContacto;
        private string _cargo;
        private string _observaciones;
        private DateTime? _fechaUltimaRetirada;
        private int _diasEntreRetiradas;
        private bool _alarmaActivada;
        private string _contenidoAceite;
        private int _numeroContenedores;
        private string _tamanoVehiculo;

        public event PropertyChangedEventHandler PropertyChanged;

        public string NombreFiscal { get => _nombreFiscal; set => SetProperty(ref _nombreFiscal, value); }
        public string NombreComercial { get => _nombreComercial; set => SetProperty(ref _nombreComercial, value); }
        public string NIF { get => _nif; set => SetProperty(ref _nif, value); }
        public string TipoCliente { get => _tipoCliente; set => SetProperty(ref _tipoCliente, value); }
        public string TipoClienteSecundario { get => _tipoClienteSecundario; set => SetProperty(ref _tipoClienteSecundario, value); }
        public string DireccionFiscal { get => _direccionFiscal; set => SetProperty(ref _direccionFiscal, value); }
        public string DireccionRecogida { get => _direccionRecogida; set => SetProperty(ref _direccionRecogida, value); }
        public string CodigoPostal { get => _codigoPostal; set => SetProperty(ref _codigoPostal, value); }
        public string Poblacion { get => _poblacion; set => SetProperty(ref _poblacion, value); }
        public string Provincia { get => _provincia; set => SetProperty(ref _provincia, value); }
        public string Pais { get => _pais; set => SetProperty(ref _pais, value); }
        public string Telefono { get => _telefono; set => SetProperty(ref _telefono, value); }
        public string Movil { get => _movil; set => SetProperty(ref _movil, value); }
        public string Email { get => _email; set => SetProperty(ref _email, value); }
        public string Fax { get => _fax; set => SetProperty(ref _fax, value); }
        public string PersonaContacto { get => _personaContacto; set => SetProperty(ref _personaContacto, value); }
        public string Cargo { get => _cargo; set => SetProperty(ref _cargo, value); }
        public string Observaciones { get => _observaciones; set => SetProperty(ref _observaciones, value); }
        public DateTime? FechaUltimaRetirada { get => _fechaUltimaRetirada; set => SetProperty(ref _fechaUltimaRetirada, value); }
        public int DiasEntreRetiradas { get => _diasEntreRetiradas; set => SetProperty(ref _diasEntreRetiradas, value); }
        public bool AlarmaActivada { get => _alarmaActivada; set => SetProperty(ref _alarmaActivada, value); }
        public string ContenidoAceite { get => _contenidoAceite; set => SetProperty(ref _contenidoAceite, value); }
        public int NumeroContenedores { get => _numeroContenedores; set => SetProperty(ref _numeroContenedores, value); }
        public string TamanoVehiculo { get => _tamanoVehiculo; set => SetProperty(ref _tamanoVehiculo, value); }

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
        private void LoadClient(object obj) { /* Lógica para cargar un cliente */ }

        protected void SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(storage, value))
            {
                storage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
