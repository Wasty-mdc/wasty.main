using MaterialDesignThemes.Wpf;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using wasty.Services;
using wasty.ViewModels;
using wasty.Views;

public class LoginViewModel : INotifyPropertyChanged
{
    private readonly ApiService _apiService;
    private readonly NavigationService _navigationService;
    private readonly AuthService _authService;
    private string _email;
    public string Contrasenia { private get; set; }

    // Constructor que inicializa el servicio de API, el servicio de navegaci�n y los comandos
    public LoginViewModel(ApiService apiService, NavigationService navigationService, AuthService authService)
    {
        _apiService = apiService;
        _navigationService = navigationService;
        LoginCommand = new RelayCommand(async _ => await Login());
        SkipCommand = new RelayCommand(async _ => await SkipLogin());
        RegisterCommand = new RelayCommand(_ => _navigationService.NavigateTo<SignupView>());
        SnackbarMessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(3));
        _authService = authService;
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

    // Propiedad para la cola de mensajes de Snackbar
    public SnackbarMessageQueue SnackbarMessageQueue { get; }

    // Comandos para iniciar sesi�n y registrarse
    public ICommand LoginCommand { get; }
    public ICommand RegisterCommand { get; }
    public ICommand SkipCommand { get; }


    // M�todo para manejar el inicio de sesi�n
    private async Task Login()
    {
        var login = new
        {
            Email,
            Contrasenia
        };

        try
        {
            var result = await _apiService.RequestAsync("POST", "auth/login", login);
            //Si la API est� caida, o presenta problemas, result ser� null
            if (result is null)
            {
                SnackbarMessageQueue.Enqueue("Error en el inicio de sesi�n.", "OK", () => { });
            }
            //Si la API responde con �xito, result ser� un objeto con la propiedad "exito" en true
            else if (result.GetProperty("exito").GetBoolean())
            {
                _authService.IsAuthenticated = true;
                SnackbarMessageQueue.Enqueue("Inicio de sesi�n exitoso", "OK", () => { });
                _navigationService.NavigateTo<MainView>();
            }
            //Si la API responde con un error de login, result ser� un objeto con la propiedad "exito" en false
            else
            {
                SnackbarMessageQueue.Enqueue("Error en el inicio de sesi�n.", "OK", () => { });
            }
        }
        catch(Exception ex)
        {

        }
    }
    private async Task SkipLogin()
    {
        // Definir credenciales predeterminadas
        Email = "Pruebas123@pruebas.com";
        Contrasenia = "Pruebas123.";

        // Llamar directamente a la funci�n de Login
        await Login();
    }

    // M�todo para navegar a la vista de registro
    private void NavigateToSignup(object parameter)
    {
        _navigationService.NavigateTo<SignupView>();
    }

    // Evento para notificar cambios en las propiedades
    public event PropertyChangedEventHandler PropertyChanged;

    // M�todo para invocar el evento PropertyChanged
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}