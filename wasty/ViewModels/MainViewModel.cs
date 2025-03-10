using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using wasty.Services;
using wasty.ViewModels;
using wasty.Views;
using wasty.Views.TableViews;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly NavigationService _navigationService;
    private object _currentView;

    // Propiedad que representa la vista actual
    public object CurrentView
    {
        get => _currentView;
        set
        {
            _currentView = value;
            OnPropertyChanged();
        }
    }

    // Constructor que inicializa el servicio de navegación y los comandos
    public MainViewModel(NavigationService navigationService)
    {
        _navigationService = navigationService;
        NavigateToLoginCommand = new RelayCommand(_ => _navigationService.NavigateTo<LoginView>());
        NavigateToClientesCommand = new RelayCommand(_ => _navigationService.NavigateTo<ClientTableView>());
        NavigateToResiduosCommand = new RelayCommand(_ => _navigationService.NavigateTo<RecycTableView>());
        NavigateToStatisticsCommand = new RelayCommand(_ => _navigationService.NavigateTo<StatisticsPanelView>());
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

    // Evento para notificar cambios en las propiedades
    public event PropertyChangedEventHandler PropertyChanged;

    // Método para invocar el evento PropertyChanged
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}