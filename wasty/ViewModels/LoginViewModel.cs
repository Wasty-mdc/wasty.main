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

    // Constructor que inicializa el servicio de API, el servicio de navegación y los comandos
    public LoginViewModel(ApiService apiService, NavigationService navigationService, AuthService authService)
    {
        _apiService = apiService;
        _navigationService = navigationService;
        LoginCommand = new RelayCommand(async _ => await Login());
        SkipCommand = new RelayCommand(async _ => await SkipLogin());
        // Skips Personal de cada instancia
        SkipCommandG = new RelayCommand(async _ => await SkipLoginG());
        SkipCommandT = new RelayCommand(async _ => await SkipLoginT());
        SkipCommandS = new RelayCommand(async _ => await SkipLoginS());
        SkipCommandH = new RelayCommand(async _ => await SkipLoginH());
        // Skips Personal de cada instancia
        RegisterCommand = new RelayCommand(_ => _navigationService.NavigateTo<SignupView>());
        SnackbarMessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(3));
        _authService = authService;
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

    // Propiedad para la cola de mensajes de Snackbar
    public SnackbarMessageQueue SnackbarMessageQueue { get; }

    // Comandos para iniciar sesión y registrarse
    public ICommand LoginCommand { get; }
    public ICommand RegisterCommand { get; }
    public ICommand SkipCommand { get; }
    // Skips Personal de cada instancia
    public ICommand SkipCommandG { get; }
    public ICommand SkipCommandT { get; }
    public ICommand SkipCommandS { get; }
    public ICommand SkipCommandH { get; }
    // Skips Personal de cada instancia



    // Método para manejar el inicio de sesión
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
            //Si la API está caida, o presenta problemas, result será null
            if (result is null)
            {
                SnackbarMessageQueue.Enqueue("Error en el inicio de sesión.", "OK", () => { });
            }
            //Si la API responde con éxito, result será un objeto con la propiedad "exito" en true
            else if (result.GetProperty("exito").GetBoolean())
            {
                _authService.IsAuthenticated = true;
                SnackbarMessageQueue.Enqueue("Inicio de sesión exitoso", "OK", () => { });
                _navigationService.NavigateTo<MainView>();
            }
            //Si la API responde con un error de login, result será un objeto con la propiedad "exito" en false
            else
            {
                SnackbarMessageQueue.Enqueue("Error en el inicio de sesión.", "OK", () => { });
            }
        }
        catch(Exception ex)
        {

        }
    }
    private async Task SkipLogin()
    {
        Email = "Pruebas123@pruebas.com";
        Contrasenia = "Pruebas123.";
        await Login();
    }
// Skips Personal de cada instancia
    private async Task SkipLoginG()
    {
        Email = "gonzalo@pruebas.com";
        Contrasenia = "Gonzalo123.";
        await Login();
    }
    private async Task SkipLoginT()
    {
        Email = "toni@pruebas.com";
        Contrasenia = "Toni123.";
        await Login();
    }
    private async Task SkipLoginS()
    {
        Email = "santi@pruebas.com";
        Contrasenia = "Santi123.";
        await Login();
    }
    private async Task SkipLoginH()
    {
        Email = "henry@pruebas.com";
        Contrasenia = "Henry123.";
        await Login();
    }
// Skips Personal de cada instancia


    // Método para navegar a la vista de registro
    private void NavigateToSignup(object parameter)
    {
        _navigationService.NavigateTo<SignupView>();
    }

    // Evento para notificar cambios en las propiedades
    public event PropertyChangedEventHandler PropertyChanged;

    // Método para invocar el evento PropertyChanged
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}