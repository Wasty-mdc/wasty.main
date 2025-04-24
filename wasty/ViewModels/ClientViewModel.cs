using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows.Input;
using wasty.Models;
using wasty.Services;
using wasty.Utils;
using wasty.ViewModels;
using wasty.Views;

public class ClientViewModel : INotifyPropertyChanged
{
    private readonly NavigationService _navigationService;
    private readonly ApiService _apiService;

    private string _clienteNombre = "METALLS DEL CAMP. SLU.";
    public string ClienteNombre
    {
        get => _clienteNombre;
        set
        {
            _clienteNombre = value;
            OnPropertyChanged();
        }
    }

    private string _textoBusqueda;
    public string TextoBusqueda
    {
        get => _textoBusqueda;
        set
        {
            if (_textoBusqueda != value)
            {
                _textoBusqueda = value;
                OnPropertyChanged();

                FiltrarClientes(); // 🔍 Filtrar cuando el texto cambie
            }
        }
    }

    public Paginador<ClienteModel> PaginadorClientes { get; private set; }

    public ICommand NavigateToClientPanelCommand { get; }

    private ClienteModel _clienteSeleccionado;
    public ClienteModel ClienteSeleccionado
    {
        get => _clienteSeleccionado;
        set
        {
            _clienteSeleccionado = value;
            OnPropertyChanged();
        }
    }

    private bool _mostrarFiltros;
    public bool MostrarFiltros
    {
        get => _mostrarFiltros;
        set
        {
            _mostrarFiltros = value;
            OnPropertyChanged();
        }
    }

    private bool _soloConNombreComercial;
    public bool SoloConNombreComercial
    {
        get => _soloConNombreComercial;
        set
        {
            _soloConNombreComercial = value;
            OnPropertyChanged();
            FiltrarClientes(); 
        }
    }

    public ICommand ToggleFiltrosCommand { get; }

    private bool _isLoadingClientes;
    public bool IsLoadingClientes
    {
        get => _isLoadingClientes;
        set
        {
            _isLoadingClientes = value;
            OnPropertyChanged();
        }
    }


    public ClientViewModel(NavigationService navigationService, ApiService apiService)
    {
        _navigationService = navigationService;
        _apiService = apiService;

        NavigateToClientPanelCommand = new RelayCommand<object>(NavigateToClientPanel);
        ToggleFiltrosCommand = new RelayCommand(_ => MostrarFiltros = !MostrarFiltros);
        Init().GetAwaiter();
    }

    private void NavigateToClientPanel(object parameter)
    {
        var selected = (ClienteModel)parameter;
        _navigationService.NavigateTo<ClientPanelView>(selected.Codigo);
    }

    private async Task Init()
    {
        IsLoadingClientes = true;

        var clientes = await GetData();
        PaginadorClientes = new Paginador<ClienteModel>(clientes, 23);

        OnPropertyChanged(nameof(PaginadorClientes));
        OnPropertyChanged(nameof(PaginadorClientes.ItemsPaginados));

        IsLoadingClientes = false;
    }


    private void FiltrarClientes()
    {
        if (PaginadorClientes == null || PaginadorClientes.TodosLosItemsOriginales == null)
            return;

        // Obtener el texto de búsqueda
        var texto = TextoBusqueda?.Trim();

        // Empezamos con todos los clientes
        var filtrados = PaginadorClientes.TodosLosItemsOriginales.AsEnumerable();

        // Si hay texto de búsqueda, filtramos por eso
        if (!string.IsNullOrEmpty(texto))
        {
            texto = texto.ToLower();
            filtrados = filtrados.Where(c =>
                (!string.IsNullOrEmpty(c.NombreComercial) && c.NombreComercial.ToLower().Contains(texto)) ||
                (!string.IsNullOrEmpty(c.NombreFiscal) && c.NombreFiscal.ToLower().Contains(texto)) ||
                (!string.IsNullOrEmpty(c.NIF) && c.NIF.ToLower().Contains(texto))
            );
        }

        // Si el filtro "Solo con Nombre Comercial" está activado, solo mostrar aquellos clientes con nombre comercial
        if (SoloConNombreComercial)
        {
            filtrados = filtrados.Where(c => !string.IsNullOrEmpty(c.NombreComercial));
        }

        // Si no hay texto ni filtros aplicados, restauramos la lista completa de clientes
        if (string.IsNullOrEmpty(texto) && !SoloConNombreComercial)
        {
            ResetClientes(); // Restaurar todos los clientes si no hay texto ni filtro
            return;
        }

        // Aplicar los filtros al paginador
        PaginadorClientes.RefrescarCon(filtrados);

        OnPropertyChanged(nameof(PaginadorClientes));
        OnPropertyChanged(nameof(PaginadorClientes.ItemsPaginados));
    }


    public void ResetClientes()
    {
        PaginadorClientes.Reset();
        OnPropertyChanged(nameof(PaginadorClientes));
        OnPropertyChanged(nameof(PaginadorClientes.ItemsPaginados));
    }

    private async Task<ObservableCollection<ClienteModel>> GetData()
    {
        try
        {
            var result = await _apiService.RequestAsync("GET", "clientes", "");
            var itemsList = JsonSerializer.Deserialize<ObservableCollection<ClienteModel>>(result.datos);
            return itemsList ?? new ObservableCollection<ClienteModel>();
        }
        catch (Exception)
        {
            return new ObservableCollection<ClienteModel>();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
