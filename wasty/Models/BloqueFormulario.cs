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

        private int _opacidad = 100;
        public int Opacidad
        {
            get => _opacidad;
            set
            {
                if (_opacidad != value)
                {
                    _opacidad = Math.Max(0, Math.Min(100, value)); // Limita entre 0 y 100
                    UpdateColors();
                }
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
        private void UpdateColors()
        {
            try
            {
                var baseColor = ColorBase?.Color ?? Colors.Transparent;
                byte alpha = (byte)(Opacidad * 2.55); // Convierte de 0-100 a 0-255

                ColorFondo = new SolidColorBrush(Color.FromArgb(alpha, LightenColor(baseColor, 0.5).R, LightenColor(baseColor, 0.5).G, LightenColor(baseColor, 0.5).B));
                ColorBorde = new SolidColorBrush(Color.FromArgb(alpha, DarkenColor(baseColor, 0.7).R, DarkenColor(baseColor, 0.7).G, DarkenColor(baseColor, 0.7).B));
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
