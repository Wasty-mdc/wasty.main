using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Input;
using wasty.Models;
using wasty.Services;
using wasty.ViewModels;
using wasty.Views;

public class RecycTableViewModel : INotifyPropertyChanged
{
    private readonly ApiService _apiService;
    private readonly NavigationService _navigationService;
    public ICommand ToggleStickyColumnCommand { get; }
    public ICommand ToggleColumnVisibilityCommand { get; }
    public ICommand VolverCommand { get; }

    private Dictionary<string, bool> _stickyColumns;
    private Dictionary<string, bool> _hiddenColumns;
    private int _frozenColumnCount;
    private ObservableCollection<Residuo> _residuos;

    // Propiedad que representa la colección de residuos
    public ObservableCollection<Residuo> Residuos
    {
        get => _residuos;
        set
        {
            _residuos = value;
            OnPropertyChanged();
        }
    }

    // Propiedad que representa el número de columnas congeladas
    public int FrozenColumnCount
    {
        get => _frozenColumnCount;
        set
        {
            _frozenColumnCount = value;
            OnPropertyChanged();
        }
    }

    // Constructor que inicializa el servicio de API, el servicio de navegación y los comandos
    public RecycTableViewModel(ApiService apiService, NavigationService navigationService)
    {
        _apiService = apiService;
        _navigationService = navigationService;
        Residuos = new ObservableCollection<Residuo>();
        _stickyColumns = new Dictionary<string, bool>();
        _hiddenColumns = new Dictionary<string, bool>();
        ToggleStickyColumnCommand = new RelayCommand<string>(ToggleStickyColumn);
        ToggleColumnVisibilityCommand = new RelayCommand<string>(ToggleColumnVisibility);
        VolverCommand = new RelayCommand(_ => _navigationService.NavigateTo<MainView>());

        Init().GetAwaiter();
    }

    // Método para inicializar la vista modelo
    private async Task Init()
    {
        Residuos = await GetData();
    }

    // Método para alternar la columna fija
    private void ToggleStickyColumn(string columnName)
    {
        if (string.IsNullOrEmpty(columnName)) return;

        if (_stickyColumns.ContainsKey(columnName))
        {
            _stickyColumns[columnName] = !_stickyColumns[columnName];
        }
        else
        {
            _stickyColumns[columnName] = true;
        }

        UpdateFrozenColumns();
    }

    // Método para actualizar las columnas congeladas
    private void UpdateFrozenColumns()
    {
        FrozenColumnCount = _stickyColumns.Values.Count(v => v);
    }

    // Método para alternar la visibilidad de la columna
    private void ToggleColumnVisibility(string columnName)
    {
        if (string.IsNullOrEmpty(columnName)) return;

        if (_hiddenColumns.ContainsKey(columnName))
        {
            _hiddenColumns[columnName] = !_hiddenColumns[columnName];
        }
        else
        {
            _hiddenColumns[columnName] = true;
        }
        OnPropertyChanged("HiddenColumns");
    }

    // Método para verificar si una columna es visible
    public bool IsColumnVisible(string columnName)
    {
        return !_hiddenColumns.TryGetValue(columnName, out bool isHidden) || !isHidden;
    }

    // Método para obtener los datos de residuos
    private async Task<ObservableCollection<Residuo>> GetData()
    {
        JsonElement tokenElement = default;
        JsonElement residuosElement = default;
        string token = "";
        string residuos = "";
        var login = new
        {
            Email = "Pruebas123@pruebas.com",
            Contrasenia = "Pruebas123."
        };

        var auth = await _apiService.RequestAsync("POST", "auth/login", login);

        if (auth.TryGetProperty("datos", out JsonElement datosElement) && datosElement.TryGetProperty("token", out tokenElement))
            token = tokenElement.GetString();

        var result = await _apiService.RequestAsync("GET", "residuos", "", token);

        if (result.TryGetProperty("datos", out residuosElement))
            residuos = residuosElement.GetRawText();

        try
        {
            ObservableCollection<Residuo> residuosList = JsonSerializer.Deserialize<ObservableCollection<Residuo>>(residuos);

            return residuosList;
        }
        catch (Exception ex)
        {
            return new ObservableCollection<Residuo>();
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