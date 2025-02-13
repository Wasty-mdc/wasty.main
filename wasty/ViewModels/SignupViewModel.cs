using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security;
using System.Windows.Input;
using wasty.Services;

namespace wasty.ViewModels
{
    public class SignupViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;

        private string _nombre;
        private string _usuario;
        private string _email;
        public string Contrasenia { private get; set; }
        public string ConfirmarContrasenia { private get; set; }

        public SignupViewModel(ApiService apiService)
        {
            _apiService = apiService;
            CrearUsuarioCommand = new RelayCommand(async _ => await CrearUsuario());
            VolverInicioSesionCommand = new RelayCommand(VolverInicioSesion);
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
        public ICommand VolverInicioSesionCommand { get; }

        private async Task CrearUsuario()
        {
            var usuario = new
            {
                Nombre,
                Usuario,
                Email,
                Contrasenia
            };

            var result = await _apiService.PostAsync("usuarios", usuario);
            if (result)
            {
                // Usuario creado exitosamente
            }
            else
            {
                // Error al crear usuario
            }
        }

        private void VolverInicioSesion(object parameter)
        {
            // Lógica para volver al inicio de sesión
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}