using System.ComponentModel;
using wasty.Models;
using wasty.Services;

namespace Wasty.ViewModels
{
    public class ResiduosPanelViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;
        private readonly NavigationService _navigationService;
        private ResiduoModel _residuo;

        public ResiduoModel Residuo
        {
            get => _residuo;
            set
            {
                _residuo = value;
                OnPropertyChanged(nameof(Residuo));
            }
        }

        public ResiduosPanelViewModel(ApiService apiService, NavigationService navigationService)
        {
            _apiService = apiService;
            _navigationService = navigationService;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public async Task Init(ResiduoModel residuo)
        {
            Residuo = residuo;
        }

        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
