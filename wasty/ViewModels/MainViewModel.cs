using System.Windows;
using System.Windows.Input;
using wasty.Models;
using wasty.Services;
using wasty.ViewModels;
using wasty.Views;
using wasty.Views.TableViews;

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
        NavigateToClientesCommand = new RelayCommand(_ => _navigationService.NavigateTo<ClientTableView>());
        NavigateToResiduosCommand = new RelayCommand(_ => _navigationService.NavigateTo<RecycTableView>());
        NavigateToStatisticsCommand = new RelayCommand(_ => _navigationService.NavigateTo<StatisticsView>());
        ShowFicherosCommand = new RelayCommand(ShowFicheros);
        ShowGestionCommand = new RelayCommand(ShowGestion);
        ShowFacturacionCommand = new RelayCommand(ShowFacturacion);
    }

    // Comandos para la navegación
    public ICommand NavigateToLoginCommand { get; }
    public ICommand NavigateToClientesCommand { get; }
    public ICommand NavigateToResiduosCommand { get; }
    public ICommand NavigateToStatisticsCommand { get; }
    public ICommand ShowFicherosCommand { get; }
    public ICommand ShowGestionCommand { get; }
    public ICommand ShowFacturacionCommand { get; }

    // Métodos para mostrar mensajes de cuadro de diálogo
    private void ShowFicheros(object parameter)
    {
        MessageBox.Show("Ficheros seleccionado");
    }

    private void ShowGestion(object parameter)
    {
        MessageBox.Show("Gestión seleccionada");
    }

    private void ShowFacturacion(object parameter)
    {
        MessageBox.Show("Facturación seleccionada");
    }

    // Método para navegar a la vista principal
    public void NavigateToMainView()
    {
        _navigationService.NavigateTo<MainView>();
    }
}