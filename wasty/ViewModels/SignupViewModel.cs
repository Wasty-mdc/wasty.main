using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using wasty.Services;
using wasty.Views;

namespace wasty.ViewModels
{
    public class SignupViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;
        private readonly NavigationService _navigationService;

        private string _nombre;
        private string _usuario;
        private string _email;
        public string Contrasenia { private get; set; }
        public string ConfirmarContrasenia { private get; set; }

        public SignupViewModel(ApiService apiService, NavigationService navigationService)
        {
            _apiService = apiService;
            _navigationService = navigationService;
            CrearUsuarioCommand = new RelayCommand(async _ => await CrearUsuario());
            LoginCommand = new RelayCommand(VolverInicioSesion);
        }

        public string Nombre
        {
            get => _nombre;
            set
            {
                _nombre = value;
                OnPropertyChanged();
            }
        }

        public string Usuario
        {
            get => _usuario;
            set
            {
                _usuario = value;
                OnPropertyChanged();
            }
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

        public ICommand CrearUsuarioCommand { get; }
        public ICommand LoginCommand { get; }

        private async Task CrearUsuario()
        {
            var usuario = new
            {
                Nombre,
                Usuario,
                Email,
                Contrasenia
            };

            var result = await _apiService.RequestAsync("POST", "usuarios", usuario);
            if (result.GetProperty("Exito").GetBoolean())
            {
                // Usuario creado exitosamente
                _navigationService.NavigateTo<LoginView>();
            }
            else
            {
                // Error al crear usuario
            }
        }

        private void VolverInicioSesion(object parameter)
        {
            _navigationService.NavigateTo<LoginView>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}