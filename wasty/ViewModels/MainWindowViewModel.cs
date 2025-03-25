using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using wasty.Models;
using wasty.Services;
using wasty.Views;

namespace wasty.ViewModels
{
    public class MainWindowViewModel : MainWindowModel
    {
        private readonly NavigationService _navigationService;
        private readonly AuthService _authService;
        private readonly AuthModel _authModel;
        // Evento para notificar cambios en las propiedades
        public event PropertyChangedEventHandler PropertyChanged;

        // Comandos para cambiar de vista
        public ICommand ShowLoginViewCommand { get; }
        public ICommand ShowSignupViewCommand { get; }
        public ICommand ShowMainViewCommand { get; }


        public MainWindowViewModel(NavigationService navigationService, AuthService authService, AuthModel authModel)
        {
            _navigationService = navigationService;
            _authService = authService;
            _authModel = authModel;
           
            _authModel.OnAuthenticationChanged += (s, e) => IsAuthenticated = _authModel.IsAuthenticated;
            // Establecer la vista inicial
            //IsAuthenticated = true;
            CurrentView = new LoginView();
            _authService = authService;
        }

        // Método para invocar el evento PropertyChanged
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
