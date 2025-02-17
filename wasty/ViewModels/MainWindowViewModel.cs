using System;
using System.ComponentModel;
using System.Windows.Input;
using wasty.Views;

namespace wasty.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

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

        // Comandos para cambiar de vista
        public ICommand ShowLoginViewCommand { get; }
        public ICommand ShowSignupViewCommand { get; }
        public ICommand ShowMainViewCommand { get; }

        public MainWindowViewModel()
        {
            ShowLoginViewCommand = new RelayCommand(_ => CurrentView = new LoginView());
            ShowSignupViewCommand = new RelayCommand(_ => CurrentView = new SignupView());
            ShowMainViewCommand = new RelayCommand(_ => CurrentView = new MainView());

            CurrentView = new LoginView(); // Vista inicial
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}