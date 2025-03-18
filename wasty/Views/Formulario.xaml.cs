using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using wasty.ViewModels;

namespace wasty.Views
{
    public partial class Formulario : UserControl
    {
        // Crear una instancia estática del convertidor
        public static readonly IValueConverter BooleanToVisibility = new BooleanToVisibilityConverter();

        public Formulario()
        {
            InitializeComponent();
            this.DataContext = new FormularioViewModel();
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
