using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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

        public event PropertyChangedEventHandler PropertyChanged;

        // Comandos de navegación
        public ICommand ShowLoginViewCommand { get; }
        public ICommand ShowSignupViewCommand { get; }
        public ICommand ShowMainViewCommand { get; }

        // Comando para mostrar/ocultar notificaciones
        public ICommand ToggleNotificationPanelCommand { get; }

        // Propiedad para visibilidad del panel de notificaciones
        private bool _isNotificationPanelOpen;
        public bool IsNotificationPanelOpen
        {
            get => _isNotificationPanelOpen;
            set
            {
                _isNotificationPanelOpen = value;
                OnPropertyChanged();
            }
        }

        // Lista de notificaciones
        public ObservableCollection<NotificationModel> Notifications { get; set; }

        public MainWindowViewModel(NavigationService navigationService, AuthService authService, AuthModel authModel)
        {
            _navigationService = navigationService;
            _authService = authService;
            _authModel = authModel;

            // Inicializar comandos de navegación
            ShowLoginViewCommand = new RelayCommand(_ =>
            {
                _authService.Logout();
            });

            ShowSignupViewCommand = new RelayCommand(_ =>
            {
                IsAuthenticated = false;
                CurrentView = new SignupView();
            });

            ShowMainViewCommand = new RelayCommand(_ =>
            {
                _navigationService.NavigateTo<MainView>();
            });

            // Inicializar comando de notificaciones
            ToggleNotificationPanelCommand = new RelayCommand(_ => ToggleNotificationPanel());

            // Simulación de notificaciones
            Notifications = new ObservableCollection<NotificationModel>
            {
                new NotificationModel { Mensaje = "Nuevo contrato generado." },
                new NotificationModel { Mensaje = "Recogida pendiente mañana." },
                new NotificationModel { Mensaje = "Factura emitida para Cliente A." }
            };

            _authModel.OnAuthenticationChanged += (s, e) => IsAuthenticated = _authModel.IsAuthenticated;

            // Establecer la vista inicial
            CurrentView = new LoginView();
        }

        private void ToggleNotificationPanel()
        {
            IsNotificationPanelOpen = !IsNotificationPanelOpen;
        }

        // Notificar cambios de propiedad
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
