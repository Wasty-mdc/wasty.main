using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json;
using System.Windows.Input;
using wasty.Models;
using wasty.Services;
using wasty.Views;

namespace wasty.ViewModels
{
    public class ResiduosViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;
        private readonly NavigationService _navigationService;

        private ResiduoModel _residuoSeleccionado;
        public ResiduoModel ResiduoSeleccionado
        {
            get => _residuoSeleccionado;
            set
            {
                _residuoSeleccionado = value;
                OnPropertyChanged(nameof(ResiduoSeleccionado));
            }
        }

        private ObservableCollection<ResiduoModel> _residuos;
        public ObservableCollection<ResiduoModel> Residuos
        {
            get => _residuos;
            set
            {
                _residuos = value;
                OnPropertyChanged(nameof(Residuos));
            }
        }

        public ICommand NavigateToResiduosPanelCommand { get; }
        public ICommand VolverAlMenuCommand { get; }

        public ResiduosViewModel(ApiService apiService, NavigationService navigationService)
        {
            _apiService = apiService;
            _navigationService = navigationService;

            NavigateToResiduosPanelCommand = new RelayCommand<object>(NavigateToResiduosPanel);
            VolverAlMenuCommand = new RelayCommand(_ => VolverAlMenu());
            Init().GetAwaiter();
        }

        private void NavigateToResiduosPanel(object parameter)
        {
            _navigationService.NavigateTo<ResiduosPanelView>(parameter);
        }

        private async Task Init()
        {
            Residuos = await GetData();
        }

        private async Task<ObservableCollection<ResiduoModel>> GetData()
        {
            JsonElement itemsElement = default;
            string items = "";
            try
            {
                var result = await _apiService.RequestAsync("GET", "residuos", "");

                var itemsList = JsonSerializer.Deserialize<ObservableCollection<ResiduoModel>>(result.datos);

                return itemsList;
            }
            catch (Exception ex)
            {
                return new ObservableCollection<ResiduoModel>();
            }
        }
        private void VolverAlMenu()
        {
            _navigationService.NavigateTo<MainView>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
