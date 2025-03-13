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

    // Constructor que inicializa el servicio de API, el servicio de navegación y los comandos
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

    // Propiedad para el correo electrónico
    public string Email
    {
        get => _email;
        set
        {
            _email = value;
            OnPropertyChanged();
        }
    }

    // Comandos para crear usuario y volver al inicio de sesión
    public ICommand CrearUsuarioCommand { get; }
    public ICommand LoginCommand { get; }

    // Método para manejar la creación de usuario
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
        //Si la API está caida, o presenta problemas, result será null
        if (result is null)
        {
            //Espacio para manejar el mensaje de error cuando la API está fuera de servicio.
        }
        else if (result.exito)
        {
            // Usuario creado exitosamente
            _navigationService.NavigateTo<LoginView>();
        }
        else
        {
            // Error al crear usuario
            // Aquí puedes agregar lógica para manejar el error, como mostrar un mensaje al usuario
        }
    }

    // Método para volver a la vista de inicio de sesión
    private void VolverInicioSesion(object parameter)
    {
        _navigationService.NavigateTo<LoginView>();
    }

    // Evento para notificar cambios en las propiedades
    public event PropertyChangedEventHandler PropertyChanged;

    // Método para invocar el evento PropertyChanged
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}