using Microsoft.Xaml.Behaviors;
using System.Windows.Controls;
using System.Windows;

namespace wasty.Utils.Behaviors
{
    public class VacioValidationBehavior : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            AssociatedObject.LostFocus += Validate;
        }

        private void Validate(object sender, RoutedEventArgs e)
        {
            string text = AssociatedObject.Text;
            bool isValid = !string.IsNullOrWhiteSpace(text);

            if (!isValid)
            {
                Validation.MarkInvalid(
                    AssociatedObject.GetBindingExpression(TextBox.TextProperty),
                    new ValidationError(new ExceptionValidationRule(),
                    AssociatedObject.GetBindingExpression(TextBox.TextProperty),
                    "Campo obligatorio", null));
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
