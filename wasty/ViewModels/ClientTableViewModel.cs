using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows.Input;
using wasty.Models;
using wasty.Services;
using wasty.ViewModels;
using wasty.Views;

public class ClientTableViewModel : INotifyPropertyChanged
{
    private readonly ApiService _apiService;
    private readonly NavigationService _navigationService;
    private ObservableCollection<ClienteModel> _clientes;

    // Comando para volver a la vista principal
    public ICommand NavigateToMainView { get; }

    // Propiedad que representa la colección de clientes
    public ObservableCollection<ClienteModel> Clientes
    {
        get => _clientes;
        set
        {
            _clientes = value;
            OnPropertyChanged();
        }
    }

    // Constructor que inicializa el servicio de navegación y los comandos
    public ClientTableViewModel(ApiService apiService, NavigationService navigationService)
    {
        _apiService = apiService;
        _navigationService = navigationService;
        NavigateToMainView = new RelayCommand(_ => _navigationService.NavigateTo<MainView>());
        // Inicializar la colección de clientes (puedes cargar datos aquí)
        Clientes = new ObservableCollection<ClienteModel>();
        Init().GetAwaiter();
    }

    // Método para inicializar la vista modelo
    private async Task Init()
    {
        Clientes = await GetData();
    }

    // Método para verificar si una columna es visible
    public bool IsColumnVisible(string columnName)
    {
        // Implementar lógica para determinar si una columna es visible
        return true;
    }

    // Método para obtener los datos de clientes
    private async Task<ObservableCollection<ClienteModel>> GetData()
    {
        var result = await _apiService.RequestAsync("GET", "clientes", "");

        try
        {
            ObservableCollection<ClienteModel> clientesList = JsonSerializer.Deserialize<ObservableCollection<ClienteModel>>(result.datos);

            return clientesList;
        }
        catch (Exception ex)
        {
            return new ObservableCollection<ClienteModel>();
        }
    }

    // Evento para notificar cambios en las propiedades
    public event PropertyChangedEventHandler PropertyChanged;

    // Método para invocar el evento PropertyChanged
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}