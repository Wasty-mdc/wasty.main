using System;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using wasty.ViewModels;

namespace wasty.Views
{
    public partial class Formulario : INotifyPropertyChanged
    {
        // Crear una instancia estática del convertidor
        public static readonly IValueConverter BooleanToVisibility = new BooleanToVisibilityConverter();

        public Formulario()
        {
            InitializeComponent();
            this.DataContext = new FormularioViewModel();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        // Permitir solo números y un único separador decimal
        private void OnlyNumbers_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;

            string currentText = textBox.Text.Insert(textBox.SelectionStart, e.Text);
            string decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

            //  Permite vacío o número válido
            if (string.IsNullOrWhiteSpace(currentText))
            {
                e.Handled = false; //  Permitir valores vacíos sin bloquear el campo
                return;
            }

            //  Permitir solo números enteros o decimales bien formateados
            Regex regex = new Regex(@"^\d*(" + Regex.Escape(decimalSeparator) + @"\d*)?$");

            e.Handled = !regex.IsMatch(currentText);
        }

        // Validar y corregir formato al salir del campo
        private void ValidateDecimalFormat(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            string decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                return; // Permitir valores vacíos sin ocultar el campo
            }

            if (double.TryParse(textBox.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double result))
            {
                // Asegurar formato correcto
                textBox.Text = result.ToString("G", CultureInfo.CurrentCulture);
            }
            else
            {
                // Si el valor no es válido, limpiar el campo pero no ocultarlo
                textBox.Text = "";
            }
        }


        // Convertidor Booleano a Visibilidad
        public class BooleanToVisibilityConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is bool booleanValue)
                    return booleanValue ? Visibility.Visible : Visibility.Collapsed;
                return Visibility.Collapsed;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return (value is Visibility visibility) && visibility == Visibility.Visible;
            }
        }
    }
}
