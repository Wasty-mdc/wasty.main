﻿using System;
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

        // Comandos para la interacción con la ventana
        public ICommand MinimizeCommand { get; }
        public ICommand MaximizeCommand { get; }
        public ICommand CloseCommand { get; }
        public ICommand DragMoveCommand { get; }

        public MainWindowViewModel(NavigationService navigationService, AuthService authService, AuthModel authModel)
        {
            _navigationService = navigationService;
            _authService = authService;
            _authModel = authModel;
            // Inicializar comandos de navegación
            ShowLoginViewCommand = new RelayCommand(_ =>
            {
                IsAuthenticated = false;
                //CurrentView = new LoginView();
                _navigationService.NavigateTo<LoginView>();
            });

            ShowSignupViewCommand = new RelayCommand(_ =>
            {
                IsAuthenticated = false;
                CurrentView = new SignupView();
            });

            ShowMainViewCommand = new RelayCommand(_ =>
            {
                IsAuthenticated = true;
                //CurrentView = new MainView();
                _navigationService.NavigateTo<MainView>();
            });

            // Inicializar comandos de interacción con la ventana
            MinimizeCommand = new RelayCommand(_ => Application.Current.MainWindow.WindowState = WindowState.Minimized);
            MaximizeCommand = new RelayCommand(_ => ToggleMaximize());
            CloseCommand = new RelayCommand(_ => Application.Current.Shutdown());
            DragMoveCommand = new RelayCommand(_ => Application.Current.MainWindow.DragMove());
            _authModel.OnAuthenticationChanged += (s, e) => IsAuthenticated = _authModel.IsAuthenticated;
            // Establecer la vista inicial
            //IsAuthenticated = true;
            CurrentView = new LoginView();
            _authService = authService;
        }

        // Método para alternar entre maximizar y restaurar la ventana
        private void ToggleMaximize()
        {
            if (Application.Current.MainWindow.WindowState == WindowState.Maximized)
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
            else
            {
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            }
        }

        // Método para invocar el evento PropertyChanged
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
