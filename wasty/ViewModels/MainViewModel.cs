using System.Windows;
using System.Windows.Input;
using wasty.Models;
using wasty.Services;
using wasty.ViewModels;
using wasty.Views;

public class MainViewModel : MainModel
{
    private readonly NavigationService _navigationService;
    private readonly AuthService _authService;


    // Constructor que inicializa el servicio de navegaci�n y los comandos
    public MainViewModel(NavigationService navigationService, AuthService authService)
    {
        _navigationService = navigationService;
        _authService = authService;
        NavigateToLoginCommand = new RelayCommand(_ => _authService.Logout());
        NavigateToClientesCommand = new RelayCommand(_ => _navigationService.NavigateTo<ClientView>());
        NavigateToClientPanelCommand = new RelayCommand(_ => _navigationService.NavigateTo<ClientPanelView>());
        NavigateToResiduosCommand = new RelayCommand(_ => _navigationService.NavigateTo<ResiduosView>());
        NavigateToResiduosPanelCommand = new RelayCommand(_ => _navigationService.NavigateTo<ResiduosPanelView>());
        NavigateToStatisticsPanelCommand = new RelayCommand(_ => _navigationService.NavigateTo<StatisticsPanelView>());
        NavigateToTraeatmentsCommand = new RelayCommand(_ => _navigationService.NavigateTo<TreatmentsView>());
        NavigateToRentingCommand = new RelayCommand(_ => _navigationService.NavigateTo<RentingView>());
        NavigateToTrasladosCommand = new RelayCommand(_ => _navigationService.NavigateTo<TrasladosView>());

        ShowFacturacionCommand = new RelayCommand(ShowFacturacion);
    }

    // Comandos para la navegaci�n
    public ICommand NavigateToLoginCommand { get; }
    public ICommand NavigateToClientesCommand { get; }
    public ICommand NavigateToClientPanelCommand { get; }
    public ICommand NavigateToResiduosCommand { get; }
    public ICommand NavigateToResiduosPanelCommand { get; }
    public ICommand NavigateToStatisticsPanelCommand { get; }
    public ICommand NavigateToTraeatmentsCommand { get; }
    public ICommand NavigateToRentingCommand { get; }
    public ICommand NavigateToTrasladosCommand { get; }


    public ICommand ShowGestionCommand { get; }
    public ICommand ShowFacturacionCommand { get; }

    // M�todos para mostrar mensajes de cuadro de di�logo

    private void ShowFacturacion(object parameter)
    {
        MessageBox.Show("Facturaci�n todav�a est� en desarrollo...");
    }

    // M�todo para navegar a la vista principal
    public void NavigateToMainView()
    {
        _navigationService.NavigateTo<MainView>();
    }
}