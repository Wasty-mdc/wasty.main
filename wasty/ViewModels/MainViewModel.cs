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


    // Constructor que inicializa el servicio de navegación y los comandos
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

        ShowGestionCommand = new RelayCommand(ShowGestion);
        ShowFacturacionCommand = new RelayCommand(ShowFacturacion);
    }

    // Comandos para la navegación
    public ICommand NavigateToLoginCommand { get; }
    public ICommand NavigateToClientesCommand { get; }
    public ICommand NavigateToClientPanelCommand { get; }
    public ICommand NavigateToResiduosCommand { get; }
    public ICommand NavigateToResiduosPanelCommand { get; }
    public ICommand NavigateToStatisticsPanelCommand { get; }
    public ICommand NavigateToTraeatmentsCommand { get; }
    public ICommand NavigateToRentingCommand { get; }

    public ICommand ShowGestionCommand { get; }
    public ICommand ShowFacturacionCommand { get; }

    // Métodos para mostrar mensajes de cuadro de diálogo

    private void ShowGestion(object parameter)
    {
        MessageBox.Show("Logística todavía está en desarrollo...");
    }

    private void ShowFacturacion(object parameter)
    {
        MessageBox.Show("Facturación todavía está en desarrollo...");
    }

    // Método para navegar a la vista principal
    public void NavigateToMainView()
    {
        _navigationService.NavigateTo<MainView>();
    }
}