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

    // Constructor que inicializa el servicio de API, el servicio de navegación y los comandos
    public LoginViewModel(NavigationService navigationService, AuthService authService)
    {
        _navigationService = navigationService;
        LoginCommand = new RelayCommand(async _ => await LoginAsync());
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
    private async Task LoginAsync()
    {
        try
        {
            var result = await _authService.LoginAsync(Email, Contrasenia);
            if (result)
            {
                SnackbarMessageQueue.Enqueue("Inicio de sesión exitoso", "OK", () => { });
                _navigationService.NavigateTo<MainView>();
            } else
                SnackbarMessageQueue.Enqueue("Error en el inicio de sesión.", "OK", () => { });
        }
        catch(Exception ex)
        {

        }
    }
    private async Task SkipLogin()
    {
        Email = "Pruebas123@pruebas.com";
        Contrasenia = "Pruebas123.";
        await LoginAsync();
    }
// Skips Personal de cada instancia
    private async Task SkipLoginG()
    {
        Email = "gonzalo@pruebas.com";
        Contrasenia = "Gonzalo123.";
        await LoginAsync();
    }
    private async Task SkipLoginT()
    {
        Email = "toni@pruebas.com";
        Contrasenia = "Toni123.";
        await LoginAsync();
    }
    private async Task SkipLoginS()
    {
        Email = "santi@pruebas.com";
        Contrasenia = "Santi123.";
        await LoginAsync();
    }
    private async Task SkipLoginH()
    {
        Email = "henry@pruebas.com";
        Contrasenia = "Henry123.";
        await LoginAsync();
    }
// Skips Personal de cada instancia


    // Método para navegar a la vista de registro
    private void NavigateToSignup(object parameter)
    {
        _navigationService.NavigateTo<SignupView>();
    }

   
}