using System.Windows.Input;
using wasty.Services;
using wasty.Views;
using System.ComponentModel;

namespace wasty.ViewModels
{
    public class StatisticsPanelViewModel : INotifyPropertyChanged
    {
        private readonly NavigationService _navigationService;

        public ICommand NavigateToStatisticsCommand { get; }

        public StatisticsPanelViewModel(NavigationService navigationService)
        {
            _navigationService = navigationService;
            NavigateToStatisticsCommand = new RelayCommand(_ => _navigationService.NavigateTo<StatisticsView>());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
