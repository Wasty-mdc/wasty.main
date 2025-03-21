using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows;
using Microsoft.Xaml.Behaviors;

namespace wasty.Utils.Behaviors
{
        public class NimaValidationBehavior : Behavior<TextBox>
        {
            private static readonly Regex _nimaRegex = new(@"^\d{14}$");

            protected override void OnAttached()
            {
                AssociatedObject.LostFocus += Validate;
            }

            private void Validate(object sender, RoutedEventArgs e)
            {
                string text = AssociatedObject.Text;
                bool isValid = _nimaRegex.IsMatch(text);

                if (!isValid)
                {
                    Validation.MarkInvalid(
                        AssociatedObject.GetBindingExpression(TextBox.TextProperty),
                        new ValidationError(new ExceptionValidationRule(),
                        AssociatedObject.GetBindingExpression(TextBox.TextProperty),
                        "El NIMA debe tener 14 dígitos numéricos.", null));
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
