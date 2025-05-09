using System;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace wasty.Models
{
    public class BloqueFormulario
    {
        public string Nombre { get; set; } //Campo Nombre
        public ObservableCollection<CampoFormulario> Campos { get; set; } //Campo pra introducir varios campos (Texto, Número, Picker, Fecha, Checkbox)
        public bool IsFirst { get; set; } //Booleano necesario para formulario clientes

        private SolidColorBrush _colorBase;
        public int NumeroPagina { get; set; }


        public SolidColorBrush ColorBase //Campo para seleccionar el color que se utilizara
        {
            get => _colorBase;
            set
            {
                _colorBase = value;
                SetColorsFromBase(value);
            }
        }

        public SolidColorBrush ColorFondo { get; private set; } = new SolidColorBrush(Colors.White); //Color mas clarito para el fondo
        public SolidColorBrush ColorBorde { get; private set; } = new SolidColorBrush(Colors.Gray); //Color mas oscuro para los bordes

        //Función para seleccionar colores a partir del color (Introduces "Green" sale "LightGreen" y "DarkGreen")
        private void SetColorsFromBase(SolidColorBrush colorName)
        {
            try
            {
                Color baseColor = colorName.Color;
                ColorFondo = new SolidColorBrush(LightenColor(baseColor, 0.5));
                ColorBorde = new SolidColorBrush(DarkenColor(baseColor, 0.7));
            }
            catch
            {
                ColorFondo = new SolidColorBrush(Colors.White);
                ColorBorde = new SolidColorBrush(Colors.Gray);
            }
        }

        //Función que se utiliza para sacar el color mas clarito ("Green" a "LightGreen")
        private Color LightenColor(Color color, double factor)
        {
            return Color.FromRgb(
                (byte)(color.R + (255 - color.R) * factor),
                (byte)(color.G + (255 - color.G) * factor),
                (byte)(color.B + (255 - color.B) * factor));
        }

        //Función que se utiliza para sacar el color mas oscura ("Green" a "DarkGreen")
        private Color DarkenColor(Color color, double factor)
        {
            return Color.FromRgb(
                (byte)(color.R * factor),
                (byte)(color.G * factor),
                (byte)(color.B * factor));
        }
    }
}
