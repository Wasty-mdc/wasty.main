using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using wasty.Services;
using wasty.ViewModels;
using wasty.Views;

public class SignupViewModel : INotifyPropertyChanged
{
    private readonly ApiService _apiService;
    private readonly NavigationService _navigationService;

    private string _nombre;
    private string _usuario;
    private string _email;
    public string Contrasenia { private get; set; }
    public string ConfirmarContrasenia { private get; set; }

    // Constructor que inicializa el servicio de API, el servicio de navegaci�n y los comandos
    public SignupViewModel(ApiService apiService, NavigationService navigationService)
    {
        _apiService = apiService;
        _navigationService = navigationService;
        CrearUsuarioCommand = new RelayCommand(async _ => await CrearUsuario());
        LoginCommand = new RelayCommand(VolverInicioSesion);
    }

    // Propiedad para el nombre
    public string Nombre
    {
        get => _nombre;
        set
        {
            _nombre = value;
            OnPropertyChanged();
        }
    }

    // Propiedad para el usuario
    public string Usuario
    {
        get => _usuario;
        set
        {
            _usuario = value;
            OnPropertyChanged();
        }
    }

    // Propiedad para el correo electr�nico
    public string Email
    {
        get => _email;
        set
        {
            _email = value;
            OnPropertyChanged();
        }
    }

    // Comandos para crear usuario y volver al inicio de sesi�n
    public ICommand CrearUsuarioCommand { get; }
    public ICommand LoginCommand { get; }

    // M�todo para manejar la creaci�n de usuario
    private async Task CrearUsuario()
    {
        var usuario = new
        {
            Nombre,
            Usuario,
            Email,
            Contrasenia
        };

        var result = await _apiService.RequestAsync("POST", "usuarios", usuario);
        //Si la API est� caida, o presenta problemas, result ser� null
        if (result is null)
        {
            //Espacio para manejar el mensaje de error cuando la API est� fuera de servicio.
        }
        else if (result.exito)
        {
            // Usuario creado exitosamente
            _navigationService.NavigateTo<LoginView>();
        }
        else
        {
            // Error al crear usuario
            // Aqu� puedes agregar l�gica para manejar el error, como mostrar un mensaje al usuario
        }
    }

    // M�todo para volver a la vista de inicio de sesi�n
    private void VolverInicioSesion(object parameter)
    {
        _navigationService.NavigateTo<LoginView>();
    }

    // Evento para notificar cambios en las propiedades
    public event PropertyChangedEventHandler PropertyChanged;

    // M�todo para invocar el evento PropertyChanged
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}