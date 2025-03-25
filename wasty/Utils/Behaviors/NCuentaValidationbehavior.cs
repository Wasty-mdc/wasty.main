using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Xaml.Behaviors;
using System.Windows.Controls;
using System.Windows;

namespace wasty.Utils.Behaviors
{
    public class NCuentaValidationBehavior : Behavior<TextBox>
    {
        private static readonly Regex _ibanRegex = new(@"^ES\d{22}$", RegexOptions.IgnoreCase);

        protected override void OnAttached()
        {
            AssociatedObject.LostFocus += Validate;
        }

        private void Validate(object sender, RoutedEventArgs e)
        {
            string text = AssociatedObject.Text?.Replace(" ", "").ToUpper(); // Limpieza básica
            bool isValid = _ibanRegex.IsMatch(text);

            if (!isValid)
            {
                Validation.MarkInvalid(
                    AssociatedObject.GetBindingExpression(TextBox.TextProperty),
                    new ValidationError(new ExceptionValidationRule(),
                    AssociatedObject.GetBindingExpression(TextBox.TextProperty),
                    "IBAN inválido. Debe empezar por ES y tener 24 caracteres.", null));
            }
            else
            {
                Validation.ClearInvalid(AssociatedObject.GetBindingExpression(TextBox.TextProperty));
            }
        }

        protected override void OnDetaching()
        {
            AssociatedObject.LostFocus -= Validate;
        }
    }
}
