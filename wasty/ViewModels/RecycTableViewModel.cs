using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Input;
using wasty.Views;
using wasty.Services;

namespace wasty.ViewModels
{
    public class RecycTableViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;
        private readonly NavigationService _navigationService;
        public ICommand ToggleStickyColumnCommand { get; }
        public ICommand ToggleColumnVisibilityCommand { get; }
        public ICommand VolverCommand { get; }


        private Dictionary<string, bool> _stickyColumns;
        private Dictionary<string, bool> _hiddenColumns;
        private int _frozenColumnCount;
        private ObservableCollection<Residuo> _residuos;

        public ObservableCollection<Residuo> Residuos
        {
            get => _residuos;
            set
            {
                _residuos = value;
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

        public RecycTableViewModel(ApiService apiService, NavigationService navigationService)
        {
            _apiService = apiService;
            _navigationService = navigationService;
            Residuos = new ObservableCollection<Residuo>();
            _stickyColumns = new Dictionary<string, bool>();
            _hiddenColumns = new Dictionary<string, bool>();
            ToggleStickyColumnCommand = new RelayCommand<string>(ToggleStickyColumn);
            ToggleColumnVisibilityCommand = new RelayCommand<string>(ToggleColumnVisibility);
            VolverCommand = new RelayCommand(_ => _navigationService.NavigateTo<MainView>());

            Init().GetAwaiter();
        }

        private async Task Init()
        {
            Residuos = await GetData();
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

        private async Task<ObservableCollection<Residuo>> GetData()
        {
            JsonElement tokenElement = default;
            JsonElement residuosElement = default;
            string token = "";
            string residuos = "";
            var login = new
            {
                Email = "Pruebas123@pruebas.com",
                Contrasenia = "Pruebas123."
            };

            var auth = await _apiService.RequestAsync("POST", "auth/login", login);

            if (auth.TryGetProperty("datos", out JsonElement datosElement) && datosElement.TryGetProperty("token", out tokenElement))
                token = tokenElement.GetString();

            var result = await _apiService.RequestAsync("GET", "residuos", "", token);

            if (result.TryGetProperty("datos", out residuosElement))
                residuos = residuosElement.GetRawText();

            try
            {
                ObservableCollection<Residuo> residuosList = JsonSerializer.Deserialize<ObservableCollection<Residuo>>(residuos);

                return residuosList;
            }
            catch (Exception ex)
            {
                return new ObservableCollection<Residuo>();
            }
            //return new List<Residuo>
            //{
            //    new Residuo { Codigo = "R001", Tipo = "Plástico", Peso = 120, Estado = "Procesado", Ubicacion = "Planta A", FechaRecoleccion = DateTime.Now, EmpresaGestora = "EcoResiduos S.A.", Destino = "Reciclaje" },
            //    new Residuo { Codigo = "R002", Tipo = "Vidrio", Peso = 85, Estado = "Pendiente", Ubicacion = "Planta B", FechaRecoleccion = DateTime.Now, EmpresaGestora = "GreenWaste Ltd.", Destino = "Reciclaje" },
            //    new Residuo { Codigo = "R003", Tipo = "Papel", Peso = 150, Estado = "Procesado", Ubicacion = "Planta C", FechaRecoleccion = DateTime.Now, EmpresaGestora = "RecyclePlus", Destino = "Reutilización" }
            //};
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Residuo
    {
        [JsonPropertyName("codResiduo")]
        public int Codigo { get; set; }
        [JsonPropertyName("nombre")]
        public string Tipo { get; set; }
        public double Peso { get; set; }
        public string Estado { get; set; }
        public string Ubicacion { get; set; }
        public DateTime FechaRecoleccion { get; set; }
        public string EmpresaGestora { get; set; }
        public string Destino { get; set; }
    }
}
