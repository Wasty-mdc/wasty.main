using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace wasty.Utils
{

        public static class PlaceholderService
        {
            public static readonly DependencyProperty PlaceholderProperty =
                DependencyProperty.RegisterAttached(
                    "Placeholder",
                    typeof(string),
                    typeof(PlaceholderService),
                    new PropertyMetadata(string.Empty, OnPlaceholderChanged));

            public static string GetPlaceholder(DependencyObject obj)
            {
                return (string)obj.GetValue(PlaceholderProperty);
            }

            public static void SetPlaceholder(DependencyObject obj, string value)
            {
                obj.SetValue(PlaceholderProperty, value);
            }

            private static void OnPlaceholderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            {
                if (d is not TextBox textBox) return;

                textBox.Loaded += (s, ev) => ShowPlaceholder(textBox);
                textBox.GotFocus += (s, ev) => HidePlaceholder(textBox);
                textBox.LostFocus += (s, ev) => ShowPlaceholder(textBox);
            }

            private static void ShowPlaceholder(TextBox textBox)
            {
                if (string.IsNullOrEmpty(textBox.Text))
                {
                    textBox.Text = GetPlaceholder(textBox);
                    textBox.Foreground = Brushes.Gray;
                }
            }

            private static void HidePlaceholder(TextBox textBox)
            {
                if (textBox.Text == GetPlaceholder(textBox))
                {
                    textBox.Text = string.Empty;
                    textBox.Foreground = Brushes.Black;
                }
            }
        }
    }


