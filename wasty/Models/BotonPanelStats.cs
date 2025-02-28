using System.ComponentModel;
using System.Text.Json.Serialization;

namespace wasty.Models
{
    public class BotonPanelStats : INotifyPropertyChanged
    {
        private string _tabla;
        private string _nombre;
        private string _icono;
        private string _comando;
        private string _colorFondo;


        [JsonPropertyName("tabla")]
        public string Tabla
        {
            get => _tabla;
            set
            {
                _tabla = value;
                OnPropertyChanged(nameof(Tabla));
            }
        }

        [JsonPropertyName("nombre")]
        public string Nombre
        {
            get => _nombre;
            set
            {
                _nombre = value;
                OnPropertyChanged(nameof(Nombre));
            }
        }

        [JsonPropertyName("icono")]
        public string Icono
        {
            get => _icono;
            set
            {
                _icono = value;
                OnPropertyChanged(nameof(Icono));
            }
        }

        public string Comando
        {
            get => _comando;
            set
            {
                _comando = value;
                OnPropertyChanged(nameof(Comando));
            }
        }

        [JsonPropertyName("colorFondo")]
        public string ColorFondo
        {
            get => _colorFondo;
            set
            {
                _colorFondo = value;
                OnPropertyChanged(nameof(ColorFondo));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
