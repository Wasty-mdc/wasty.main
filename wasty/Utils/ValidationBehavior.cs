using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Text.RegularExpressions;
using Microsoft.Xaml.Behaviors;
using wasty.Models;

namespace wasty.Utils
{
    public class ValidationBehavior : Behavior<TextBox>
    {
        public static readonly DependencyProperty CampoFormularioProperty =
            DependencyProperty.Register(nameof(CampoFormulario), typeof(CampoFormulario), typeof(ValidationBehavior), new PropertyMetadata(null));

        public CampoFormulario CampoFormulario
        {
            get => (CampoFormulario)GetValue(CampoFormularioProperty);
            set => SetValue(CampoFormularioProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.LostFocus += ValidarCampo;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.LostFocus -= ValidarCampo;
        }

        private void ValidarCampo(object sender, RoutedEventArgs e)
        {
            if (CampoFormulario == null) return;

            var valor = AssociatedObject.Text;
            var tipo = CampoFormulario.TipoValidacion;

            bool esValido = tipo switch
            {
                "Vacio" => !string.IsNullOrWhiteSpace(valor),
                "DNI" or "NIF" => Regex.IsMatch(valor, @"^([A-Za-z]\d{8}|\d{8}[A-Za-z])$"),
                "NIMA" => Regex.IsMatch(valor, @"^\d{14}$"),
                "CNAE" => Regex.IsMatch(valor, @"^\d{4}$"),
                "INE" => Regex.IsMatch(valor, @"^\d{1,5}$"),
                "Telefono" => Regex.IsMatch(valor, @"^\d{9}$"),
                "Email" => Regex.IsMatch(valor, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"),
                _ => true
            };

            AssociatedObject.BorderBrush = esValido ? Brushes.Green : Brushes.Red;
        }
    }
}
