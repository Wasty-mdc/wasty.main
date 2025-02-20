using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using wasty.Services;
using wasty.ViewModels;
using wasty.Views;
using wasty.Views.TableViews;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly NavigationService _navigationService;

    public MainViewModel(NavigationService navigationService)
    {
        _navigationService = navigationService;
        SalirCommand = new RelayCommand(NavigateToLogin);
        clientesCommand = new RelayCommand(NavigateToClientes);
        residuosCommand = new RelayCommand(NavigateToResiduos);

    }

    public ICommand SalirCommand { get; }
    public ICommand clientesCommand { get; }
    public ICommand residuosCommand { get; }


    private void NavigateToLogin(object parameter)
    {
        _navigationService.NavigateTo<LoginView>();
    }

    private void NavigateToClientes(object parameter)
    {
        _navigationService.NavigateTo<ClientTableView>();
    }

    private void NavigateToResiduos(object parameter)
    {
        _navigationService.NavigateTo<RecycTableView>();
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
