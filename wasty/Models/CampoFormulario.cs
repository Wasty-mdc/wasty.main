using System;
using System.Windows.Media;
namespace wasty.Models
{
    public class CampoFormulario
    {
        public string Nombre { get; set; } //Nombre del campo
        public string Tipo { get; set; } //Si es texto, numero, etc...

        private string _valor = ""; //  Ahora el campo empieza vacío

        public string Valor
        {
            get => _valor;
            set
            {
                _valor = value; // Permitir valores vacíos sin forzar "0"
            }
        }
        public List<String> Opciones { get; set; } //Opciones para el Picker
        public bool EstaSeleccionado { get; set; } //Booleano para CheckBox
        public string TipoValidacion { get; set; } //String para saber que tipo de validacion hay que hacer (DNI, Telefono, Correo, etc..)
    }
}