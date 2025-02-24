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
    private string _email;
    public string Contrasenia { private get; set; }

    public LoginViewModel(ApiService apiService, NavigationService navigationService)
    {
        _apiService = apiService;
        _navigationService = navigationService;
        LoginCommand = new RelayCommand(async _ => await Login());
        SkipCommand = new RelayCommand(async _ => await SkipLogin());
        RegisterCommand = new RelayCommand(_ => _navigationService.NavigateTo<SignupView>());
        SnackbarMessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(3));
    }

    public string Email
    {
        get => _email;
        set
        {
            _email = value;
            OnPropertyChanged();
        }
    }

    public SnackbarMessageQueue SnackbarMessageQueue { get; }

    public ICommand LoginCommand { get; }
    public ICommand RegisterCommand { get; }
    public ICommand SkipCommand { get; }


    private async Task Login()
    {
        var login = new
        {
            Email,
            Contrasenia
        };

        var result = await _apiService.RequestAsync("POST", "auth/login", login);
        if (result.GetProperty("exito").GetBoolean())
        {
            SnackbarMessageQueue.Enqueue("Inicio de sesión exitoso", "OK", () => { });
            _navigationService.NavigateTo<MainView>();
        }
        else
        {
            SnackbarMessageQueue.Enqueue("Error en el inicio de sesión.", "OK", () => { });
        }
    }
    private async Task SkipLogin()
    {
        // Definir credenciales predeterminadas
        Email = "Pruebas123@pruebas.com";
        Contrasenia = "Pruebas123.";

        // Llamar directamente a la función de Login
        await Login();
    }

    private void NavigateToSignup(object parameter)
    {
        _navigationService.NavigateTo<SignupView>();
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
