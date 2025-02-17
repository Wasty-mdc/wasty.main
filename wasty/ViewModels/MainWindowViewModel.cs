using System;
using System.ComponentModel;
using System.Windows.Input;
using wasty.Views;
using wasty.Services;

namespace wasty.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly NavigationService _navigationService;
        private object _currentView;

        public object CurrentView
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

        public MainWindowViewModel(NavigationService navigationService)
        {
            _navigationService = navigationService;

            ShowLoginViewCommand = new RelayCommand(_ => _navigationService.NavigateTo<LoginView>());
            ShowSignupViewCommand = new RelayCommand(_ => _navigationService.NavigateTo<SignupView>());
            ShowMainViewCommand = new RelayCommand(_ => _navigationService.NavigateTo<MainView>());

            CurrentView = _navigationService.NavigateTo<LoginView>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}