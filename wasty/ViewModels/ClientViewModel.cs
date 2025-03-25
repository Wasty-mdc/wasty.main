using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using wasty.Services;
using wasty.Views;

namespace wasty.ViewModels
{
    public class ClientViewModel : INotifyPropertyChanged
    {
        private readonly NavigationService _navigationService;

        private string _clienteNombre = "METALLS DEL CAMP. SLU.";
        public string ClienteNombre
        {
            get => _clienteNombre;
            set
            {
                _clienteNombre = value;
                OnPropertyChanged();
            }
        }

        public ClientViewModel(NavigationService navigationService)
        {
            _navigationService = navigationService;

            NavigateToClientPanelCommand = new RelayCommand(_ => new ClientPanelView().ShowDialog());

        }
        public ICommand NavigateToClientPanelCommand { get; }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
