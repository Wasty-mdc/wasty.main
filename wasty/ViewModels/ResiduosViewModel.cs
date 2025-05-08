using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using wasty.Models;
using wasty.Services;
using wasty.Utils;
using wasty.Views;

namespace wasty.ViewModels
{
    public class ResiduosViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;
        private readonly NavigationService _navigationService;

        public Paginador<ResiduoModel> PaginadorResiduos { get; private set; }

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
            var residuos = await GetData();
            //PaginadorResiduos = new Paginador<ResiduoModel>(residuos, 23); // Puedes ajustar items por página
            OnPropertyChanged(nameof(PaginadorResiduos));
            OnPropertyChanged(nameof(PaginadorResiduos.ItemsPaginados));
        }

        private async Task<ObservableCollection<ResiduoModel>> GetData()
        {
            try
            {
                var result = await _apiService.RequestAsync("GET", "residuos", "");
                var itemsList = JsonSerializer.Deserialize<ObservableCollection<ResiduoModel>>(result.datos);
                PaginadorResiduos = new Paginador<ResiduoModel>(_apiService, "residuos", itemsList, result.pagination.pageSize, result.pagination.pageNumber, result.pagination.totalPages);
                return itemsList ?? new ObservableCollection<ResiduoModel>();
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
