using System.ComponentModel;
using System.Runtime.CompilerServices;
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
        RegisterCommand = new RelayCommand(NavigateToSignup);
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

    public ICommand LoginCommand { get; }
    public ICommand RegisterCommand { get; }

    private async Task Login()
    {
        var login = new
        {
            Email,
            Contrasenia
        };

        var result = await _apiService.PostAsync("auth/login", login);
        if (result)
        {
            _navigationService.NavigateTo<MainView>();
        }
        else
        {
            // L�gica para manejar error en el inicio de sesi�n
        }
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
