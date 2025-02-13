using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using wasty.Services;
using wasty.ViewModels;
using wasty.Views;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly NavigationService _navigationService;

    public MainViewModel(NavigationService navigationService)
    {
        _navigationService = navigationService;
        SalirCommand = new RelayCommand(NavigateToLogin);
    }

    public ICommand SalirCommand { get; }

    private void NavigateToLogin(object parameter)
    {
        _navigationService.NavigateTo<LoginView>();
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
