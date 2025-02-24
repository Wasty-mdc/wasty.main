using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using wasty.Models;
using wasty.Services;
using wasty.ViewModels;
using wasty.Views;

public class ClientTableViewModel : INotifyPropertyChanged
{
    private readonly NavigationService _navigationService;
    private ObservableCollection<Cliente> _clientes;

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
    public ClientTableViewModel(NavigationService navigationService)
    {
        _navigationService = navigationService;
        VolverCommand = new RelayCommand(_ => _navigationService.NavigateTo<MainView>());
        // Inicializar la colección de clientes (puedes cargar datos aquí)
        Clientes = new ObservableCollection<Cliente>();
    }

    // Comando para volver a la vista principal
    public ICommand VolverCommand { get; }

    // Método para verificar si una columna es visible
    public bool IsColumnVisible(string columnName)
    {
        // Implementar lógica para determinar si una columna es visible
        return true;
    }

    // Evento para notificar cambios en las propiedades
    public event PropertyChangedEventHandler PropertyChanged;

    // Método para invocar el evento PropertyChanged
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}