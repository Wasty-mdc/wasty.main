using MaterialDesignThemes.Wpf;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using wasty.Models;
using wasty.Services;
using wasty.ViewModels;
using wasty.Views;

class LoginViewModel : LoginModel
{
    private readonly NavigationService _navigationService;
    private readonly AuthService _authService;
    public string Contrasenia { private get; set; }
    // Propiedad para la cola de mensajes de Snackbar
    public SnackbarMessageQueue SnackbarMessageQueue { get; }

    // Constructor que inicializa el servicio de API, el servicio de navegaci�n y los comandos
    public LoginViewModel(NavigationService navigationService, AuthService authService)
    {
        _navigationService = navigationService;
        LoginCommand = new RelayCommand(async _ => await LoginAsync());
        SkipCommand = new RelayCommand(async _ => await SkipLogin());
        SkipCommandG = new RelayCommand(async _ => await SkipLoginG());
        SkipCommandS = new RelayCommand(async _ => await SkipLoginS());
        SkipCommandT = new RelayCommand(async _ => await SkipLoginT());
        SkipCommandH = new RelayCommand(async _ => await SkipLoginH());

        RegisterCommand = new RelayCommand(_ => _navigationService.NavigateTo<SignupView>());
        SnackbarMessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(3));
        _authService = authService;
    }

    // Comandos para iniciar sesi�n y registrarse
    public ICommand LoginCommand { get; }
    public ICommand SkipCommandG { get; }
    public ICommand SkipCommandS { get; }
    public ICommand SkipCommandT { get; }
    public ICommand SkipCommandH { get; }
    public ICommand RegisterCommand { get; }
    public ICommand SkipCommand { get; }


    // M�todo para manejar el inicio de sesi�n
    private async Task LoginAsync()
    {
        try
        {
            var result = await _authService.LoginAsync(Email, Contrasenia);
            if (result)
            {
                SnackbarMessageQueue.Enqueue("Inicio de sesi�n exitoso", "OK", () => { });
                _navigationService.NavigateTo<MainView>();
            }
            else
                SnackbarMessageQueue.Enqueue("Error en el inicio de sesi�n.", "OK", () => { });
        }
        catch (Exception ex)
        {

        }
    }
    private async Task SkipLogin()
    {
        // Definir credenciales predeterminadas
        Email = "Pruebas123@pruebas.com";
        Contrasenia = "Pruebas123.";

        // Llamar directamente a la funci�n de Login
        await LoginAsync();
    }

    private async Task SkipLoginG()
    {
        // Definir credenciales predeterminadas
        Email = "Gonzalo@pruebas.com";
        Contrasenia = "Gonzalo123.";

        // Llamar directamente a la funci�n de Login
        await LoginAsync();
    }

    private async Task SkipLoginS()
    {
        // Definir credenciales predeterminadas
        Email = "Santi@pruebas.com";
        Contrasenia = "Santi123.";

        // Llamar directamente a la funci�n de Login
        await LoginAsync();
    }

    private async Task SkipLoginT()
    {
        // Definir credenciales predeterminadas
        Email = "Toni@pruebas.com";
        Contrasenia = "Toni123.";

        // Llamar directamente a la funci�n de Login
        await LoginAsync();
    }
    private async Task SkipLoginH()
    {
        // Definir credenciales predeterminadas
        Email = "Henry@pruebas.com";
        Contrasenia = "Henry123.";

        // Llamar directamente a la funci�n de Login
        await LoginAsync();
    }

    // M�todo para navegar a la vista de registro
    private void NavigateToSignup(object parameter)
    {
        _navigationService.NavigateTo<SignupView>();
    }


}