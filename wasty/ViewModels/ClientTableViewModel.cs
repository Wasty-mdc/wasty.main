using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using wasty.Services;
using wasty.Views;

namespace wasty.ViewModels
{
    public class ClientTableViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;
        private readonly NavigationService _navigationService;
        public ObservableCollection<Cliente> Clientes { get; set; }
        public ICommand ToggleStickyColumnCommand { get; }
        public ICommand ToggleColumnVisibilityCommand { get; }
        public ICommand VolverCommand { get; }

        private Dictionary<string, bool> _stickyColumns;
        private Dictionary<string, bool> _hiddenColumns;
        private int _frozenColumnCount;

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
            _navigationService = navigationService;
            Clientes = new ObservableCollection<Cliente>(GetData().Result);
            _stickyColumns = new Dictionary<string, bool>();
            _hiddenColumns = new Dictionary<string, bool>();
            ToggleStickyColumnCommand = new RelayCommand<string>(ToggleStickyColumn);
            ToggleColumnVisibilityCommand = new RelayCommand<string>(ToggleColumnVisibility); 
            VolverCommand = new RelayCommand(_ => _navigationService.NavigateTo<MainView>());
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


        private async Task<List<Cliente>> GetData()
        {
            var login = new
            {
                Email = "Pruebas123@pruebas.com",
                Contrasenia = "Pruebas123."
            };

            var result = await _apiService.PostAsync<object, dynamic>("auth/login", login);

            return new List<Cliente>
                    {
                        new Cliente { Codigo = "001", NombreComercial = "Empresa A", NombreFiscal = "Empresa A. S.L.",
                                      NIF = "12345678X", Fiscalidad = "S.L.", Domicilio = "Calle Falsa 123", CodigoPostal = "28001",
                                      Provincia = "Valencia", Poblacion = "Aldaia", Zona = "04 Aldaia Alaquas", Telefono = "912345678",
                                      Email = "empresaA@example.com", TituloRelacion = "Proveedor", Comercial = "Gonzalo Aguirre", Estado = "Activo" },

                        new Cliente { Codigo = "002", NombreComercial = "Empresa B", NombreFiscal = "Empresa B. S.L.",
                                      NIF = "87654321Y", Fiscalidad = "Autónomo", Domicilio = "Avenida Principal 456", CodigoPostal = "46910",
                                      Provincia = "Valencia", Poblacion = "Alaquàs", Zona = "04 Aldaia Alaquas", Telefono = "934567890",
                                      Email = "empresaB@example.com", TituloRelacion = "Cliente", Comercial = "María López", Estado = "Inactivo" },

                        new Cliente { Codigo = "003", NombreComercial = "Empresa C", NombreFiscal = "Empresa C. S.A.",
                                      NIF = "11223344Z", Fiscalidad = "S.A.", Domicilio = "Calle Mayor 8", CodigoPostal = "46920",
                                      Provincia = "Valencia", Poblacion = "Mislata", Zona = "05 Mislata Quart", Telefono = "955678901",
                                      Email = "empresaC@example.com", TituloRelacion = "Distribuidor", Comercial = "Luis Fernández", Estado = "Activo" },

                        new Cliente { Codigo = "004", NombreComercial = "Empresa D", NombreFiscal = "Empresa D. Cooperativa",
                                      NIF = "55667788W", Fiscalidad = "Cooperativa", Domicilio = "Calle San Vicente 101", CodigoPostal = "46930",
                                      Provincia = "Valencia", Poblacion = "Quart de Poblet", Zona = "05 Mislata Quart", Telefono = "966789012",
                                      Email = "empresaD@example.com", TituloRelacion = "Socio", Comercial = "Elena Torres", Estado = "Activo" },

                        new Cliente { Codigo = "005", NombreComercial = "Empresa E", NombreFiscal = "Empresa E. S.L.U.",
                                      NIF = "99887766V", Fiscalidad = "S.L.U.", Domicilio = "Plaza del Ayuntamiento 202", CodigoPostal = "46002",
                                      Provincia = "Valencia", Poblacion = "Valencia", Zona = "01 Valencia Centro", Telefono = "963445566",
                                      Email = "empresaE@example.com", TituloRelacion = "Proveedor", Comercial = "Carlos García", Estado = "Inactivo" },

                        new Cliente { Codigo = "006", NombreComercial = "Empresa F", NombreFiscal = "Empresa F. S.L.",
                                      NIF = "44556677K", Fiscalidad = "S.L.", Domicilio = "Calle de las Flores 15", CodigoPostal = "46100",
                                      Provincia = "Valencia", Poblacion = "Burjassot", Zona = "02 Burjassot Paterna", Telefono = "961223344",
                                      Email = "empresaF@example.com", TituloRelacion = "Cliente", Comercial = "Andrea Martínez", Estado = "Activo" },

                        new Cliente { Codigo = "007", NombreComercial = "Empresa G", NombreFiscal = "Empresa G. S.A.",
                                      NIF = "66778899L", Fiscalidad = "S.A.", Domicilio = "Calle Mayor 9", CodigoPostal = "46110",
                                      Provincia = "Valencia", Poblacion = "Godella", Zona = "02 Burjassot Paterna", Telefono = "961334455",
                                      Email = "empresaG@example.com", TituloRelacion = "Distribuidor", Comercial = "Pedro Sánchez", Estado = "Inactivo" },

                        new Cliente { Codigo = "008", NombreComercial = "Empresa H", NombreFiscal = "Empresa H. Autónomo",
                                      NIF = "11224455M", Fiscalidad = "Autónomo", Domicilio = "Avenida del Oeste 30", CodigoPostal = "46130",
                                      Provincia = "Valencia", Poblacion = "Massamagrell", Zona = "03 Massamagrell", Telefono = "961556677",
                                      Email = "empresaH@example.com", TituloRelacion = "Proveedor", Comercial = "Lucía Gómez", Estado = "Activo" },

                        new Cliente { Codigo = "009", NombreComercial = "Empresa I", NombreFiscal = "Empresa I. S.L.U.",
                                      NIF = "22334455N", Fiscalidad = "S.L.U.", Domicilio = "Calle la Paz 22", CodigoPostal = "46980",
                                      Provincia = "Valencia", Poblacion = "Paterna", Zona = "02 Burjassot Paterna", Telefono = "961667788",
                                      Email = "empresaI@example.com", TituloRelacion = "Cliente", Comercial = "Manuel Ruiz", Estado = "Inactivo" },

                        new Cliente { Codigo = "010", NombreComercial = "Empresa J", NombreFiscal = "Empresa J. S.L.",
                                      NIF = "33445566O", Fiscalidad = "S.L.", Domicilio = "Plaza del Ayuntamiento 10", CodigoPostal = "46002",
                                      Provincia = "Valencia", Poblacion = "Valencia", Zona = "01 Valencia Centro", Telefono = "963334455",
                                      Email = "empresaJ@example.com", TituloRelacion = "Distribuidor", Comercial = "Ana Fernández", Estado = "Activo" },

                        new Cliente { Codigo = "011", NombreComercial = "Empresa K", NombreFiscal = "Empresa K. S.A.",
                                      NIF = "44556677P", Fiscalidad = "S.A.", Domicilio = "Calle del Mar 101", CodigoPostal = "46120",
                                      Provincia = "Valencia", Poblacion = "Alboraya", Zona = "06 Alboraya Alboraia", Telefono = "962556677",
                                      Email = "empresaK@example.com", TituloRelacion = "Proveedor", Comercial = "José Herrera", Estado = "Inactivo" },

                        new Cliente { Codigo = "012", NombreComercial = "Empresa L", NombreFiscal = "Empresa L. Cooperativa",
                                      NIF = "55667788Q", Fiscalidad = "Cooperativa", Domicilio = "Avenida del Cid 55", CodigoPostal = "46190",
                                      Provincia = "Valencia", Poblacion = "Riba-roja de Túria", Zona = "07 Riba-roja", Telefono = "962889900",
                                      Email = "empresaL@example.com", TituloRelacion = "Cliente", Comercial = "Isabel Moreno", Estado = "Activo" }

                    };
                        }

        public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public class Cliente
        {
            public string Codigo { get; set; }
            public string NombreComercial { get; set; }
            public string NombreFiscal { get; set; }
            public string NIF { get; set; }
            public string Fiscalidad { get; set; }
            public string Domicilio { get; set; }
            public string CodigoPostal { get; set; }
            public string Provincia { get; set; }
            public string Poblacion { get; set; }
            public string Zona { get; set; }
            public string Telefono { get; set; }
            public string Email { get; set; }
            public string TituloRelacion { get; set; }
            public string Comercial { get; set; }
            public string Estado { get; set; }
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
