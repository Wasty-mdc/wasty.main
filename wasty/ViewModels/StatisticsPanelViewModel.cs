using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using wasty.Services;
using wasty.Views;
using wasty.Models;
using System.Text.Json;

namespace wasty.ViewModels
{
    public class StatisticsPanelViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;
        private readonly NavigationService _navigationService;
        public ICommand NavigateToStatisticsCommand { get; }

        private ObservableCollection<BotonPanelStats> _botones;
        public ObservableCollection<BotonPanelStats> Botones
        {
            get => _botones;
            set
            {
                _botones = value;
                OnPropertyChanged(nameof(Botones));
            }
        }

        public StatisticsPanelViewModel(ApiService apiService, NavigationService navigationService)
        {
            _apiService = apiService;
            _navigationService = navigationService;
            NavigateToStatisticsCommand = new RelayCommand(_ => _navigationService.NavigateTo<StatisticsView>());
            Botones = new ObservableCollection<BotonPanelStats>();
            Init().GetAwaiter();
            //Botones = new ObservableCollection<BotonPanelStats>
            //{
            //    new BotonPanelStats { Nombre = "CLIENTES", Icono = "AccountMultiple", Comando = "ClientesCommand", ColorFondo = "#2271b1" },
            //    new BotonPanelStats { Nombre = "RESIDUOS", Icono = "Recycle", Comando = "ResiduosCommand", ColorFondo = "#A3BE8C" },
            //    new BotonPanelStats { Nombre = "ENVIOS", Icono = "Truck", Comando = "EnviosCommand", ColorFondo = "#F3CC5C" },
            //    new BotonPanelStats { Nombre = "RECOGIDAS", Icono = "Cube", Comando = "RecogidasCommand", ColorFondo = "#E57373" },
            //    new BotonPanelStats { Nombre = "STOCK", Icono = "Warehouse", Comando = "StockCommand", ColorFondo = "#A67C52" },
            //    new BotonPanelStats { Nombre = "COBROS", Icono = "Calculator", Comando = "CobrosCommand", ColorFondo = "#4A90E2" },
            //    new BotonPanelStats { Nombre = "PAGOS", Icono = "CashMultiple", Comando = "PagosCommand", ColorFondo = "#F39C12" },
            //    new BotonPanelStats { Nombre = "MATERIAL", Icono = "Archive", Comando = "MaterialCommand", ColorFondo = "#2D2D2D" },
            //    new BotonPanelStats { Nombre = "VENTAS", Icono = "Store", Comando = "VentasCommand", ColorFondo = "#2E7D32" },
            //    new BotonPanelStats { Nombre = "COBROS", Icono = "Cart", Comando = "CobrosCommand2", ColorFondo = "#B71C1C" }
            //};
        }

        private async Task Init()
        {
            Botones = await GetData();
        }

        private async Task<ObservableCollection<BotonPanelStats>> GetData()
        {
            JsonElement itemsElement = default;
            string items = "";
            try
            {
                JsonElement tokenElement = default;
                ObservableCollection<BotonPanelStats> fieldsList = new ObservableCollection<BotonPanelStats>();
                string token = "";
                var login = new
                {
                    Email = "Pruebas123@pruebas.com",
                    Contrasenia = "Pruebas123."
                };

                var auth = await _apiService.RequestAsync("POST", "auth/login", login);

                if (auth.TryGetProperty("datos", out JsonElement datosElement) && datosElement.TryGetProperty("token", out tokenElement))
                    token = tokenElement.GetString();

                var result = await _apiService.RequestAsync("GET", "estadistica/paneles", "", token);

                if (result.TryGetProperty("datos", out itemsElement))
                    items = itemsElement.GetRawText();

                var itemsList = JsonSerializer.Deserialize<ObservableCollection<BotonPanelStats>>(items);

                return itemsList;
            }
            catch (Exception ex)
            {
                return new ObservableCollection<BotonPanelStats>();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
