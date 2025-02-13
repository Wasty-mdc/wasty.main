using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using wasty.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace wasty.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;
        private string _email;
        public string Contrasenia { private get; set; }

        public LoginViewModel(ApiService apiService)
        {
            _apiService = apiService;
            LoginCommand = new RelayCommand(async _ => await Login());
            NavigateToSignupCommand = new RelayCommand(NavigateToSignup);
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
        public ICommand NavigateToSignupCommand { get; }

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
                // Lógica para manejar inicio de sesión exitoso
            }
            else
            {
                // Lógica para manejar error en el inicio de sesión
            }
        }

        private void NavigateToSignup(object parameter)
        {
            // Lógica para navegar a la vista de registro
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}