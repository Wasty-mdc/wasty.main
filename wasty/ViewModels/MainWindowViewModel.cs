using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using wasty.Views;
using wasty.Services;

namespace wasty.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly NavigationService _navigationService;
        private UserControl _currentView;

        public UserControl CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public ICommand ShowLoginViewCommand { get; }
        public ICommand ShowSignupViewCommand { get; }
        public ICommand ShowMainViewCommand { get; }

        //  Se añade el constructor para recibir NavigationService
        public MainWindowViewModel(NavigationService navigationService)
        {
            _navigationService = navigationService;

            ShowLoginViewCommand = new RelayCommand(_ => _navigationService.NavigateTo<LoginView>());
            ShowSignupViewCommand = new RelayCommand(_ => _navigationService.NavigateTo<SignupView>());
            ShowMainViewCommand = new RelayCommand(_ => _navigationService.NavigateTo<MainView>());

            // Se asegura de que `LoginView` sea la vista inicial
            _navigationService.NavigateTo<LoginView>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
