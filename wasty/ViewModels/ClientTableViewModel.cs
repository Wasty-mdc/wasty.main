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
    private ObservableCollection<Cliente> _clientes;

    // Comando para volver a la vista principal
    public ICommand NavigateToMainView { get; }

    // Propiedad que representa la colección de clientes
    public ObservableCollection<Cliente> Clientes
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
        Clientes = new ObservableCollection<Cliente>();
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
    private async Task<ObservableCollection<Cliente>> GetData()
    {
        JsonElement tokenElement = default;
        JsonElement clientesElement = default;
        string token = "";
        string clientes = "";
        var login = new
        {
            Email = "Pruebas123@pruebas.com",
            Contrasenia = "Pruebas123."
        };

        var auth = await _apiService.RequestAsync("POST", "auth/login", login);

        if (auth.TryGetProperty("datos", out JsonElement datosElement) && datosElement.TryGetProperty("token", out tokenElement))
            token = tokenElement.GetString();

        var result = await _apiService.RequestAsync("GET", "clientes", "", token);

        if (result.TryGetProperty("datos", out clientesElement))
            clientes = clientesElement.GetRawText();

        try
        {
            ObservableCollection<Cliente> clientesList = JsonSerializer.Deserialize<ObservableCollection<Cliente>>(clientes);

            return clientesList;
        }
        catch (Exception ex)
        {
            return new ObservableCollection<Cliente>();
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