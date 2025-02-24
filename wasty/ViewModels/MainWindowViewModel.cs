using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using wasty.Views;

namespace wasty.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        // Evento para notificar cambios en las propiedades
        public event PropertyChangedEventHandler PropertyChanged;

        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        // Comandos para cambiar de vista
        public ICommand ShowLoginViewCommand { get; }
        public ICommand ShowSignupViewCommand { get; }
        public ICommand ShowMainViewCommand { get; }

        // Comandos para la interacción con la ventana
        public ICommand MinimizeCommand { get; }
        public ICommand MaximizeCommand { get; }
        public ICommand CloseCommand { get; }
        public ICommand DragMoveCommand { get; }

        public MainWindowViewModel()
        {
            // Inicializar comandos de navegación
            ShowLoginViewCommand = new RelayCommand(_ => CurrentView = new LoginView());
            ShowSignupViewCommand = new RelayCommand(_ => CurrentView = new SignupView());
            ShowMainViewCommand = new RelayCommand(_ => CurrentView = new MainView());

            // Inicializar comandos de interacción con la ventana
            MinimizeCommand = new RelayCommand(_ => Application.Current.MainWindow.WindowState = WindowState.Minimized);
            MaximizeCommand = new RelayCommand(_ => ToggleMaximize());
            CloseCommand = new RelayCommand(_ => Application.Current.Shutdown());
            DragMoveCommand = new RelayCommand(_ => Application.Current.MainWindow.DragMove());

            // Establecer la vista inicial
            CurrentView = new LoginView();
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