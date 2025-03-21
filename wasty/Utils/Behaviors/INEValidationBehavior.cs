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
    public class IneValidationBehavior : Behavior<TextBox>
    {
        private static readonly Regex _ineRegex = new(@"^\d{1,5}$");

        protected override void OnAttached()
        {
            AssociatedObject.LostFocus += Validate;
        }

        private void Validate(object sender, RoutedEventArgs e)
        {
            string text = AssociatedObject.Text;
            bool isValid = _ineRegex.IsMatch(text);

            if (!isValid)
            {
                Validation.MarkInvalid(
                    AssociatedObject.GetBindingExpression(TextBox.TextProperty),
                    new ValidationError(new ExceptionValidationRule(),
                    AssociatedObject.GetBindingExpression(TextBox.TextProperty),
                    "El código INE debe tener entre 1 y 5 dígitos.", null));
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
