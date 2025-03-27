using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace wasty.Utils
{
    public static class SoloNumerosHelper
    {
        public static readonly DependencyProperty ActivarSoloNumerosProperty =
            DependencyProperty.RegisterAttached(
                "ActivarSoloNumeros",
                typeof(bool),
                typeof(SoloNumerosHelper),
                new PropertyMetadata(false, OnActivarSoloNumerosChanged));

        public static void SetActivarSoloNumeros(UIElement element, bool value)
        {
            element.SetValue(ActivarSoloNumerosProperty, value);
        }

        public static bool GetActivarSoloNumeros(UIElement element)
        {
            return (bool)element.GetValue(ActivarSoloNumerosProperty);
        }

        private static void OnActivarSoloNumerosChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox textBox)
            {
                if ((bool)e.NewValue)
                {
                    textBox.PreviewTextInput += TextBox_PreviewTextInput;
                    DataObject.AddPastingHandler(textBox, OnPaste);
                }
                else
                {
                    textBox.PreviewTextInput -= TextBox_PreviewTextInput;
                    DataObject.RemovePastingHandler(textBox, OnPaste);
                }
            }
        }

        private static void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^[0-9,]*$");
        }

        private static void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(DataFormats.Text))
            {
                string texto = e.DataObject.GetData(DataFormats.Text) as string;
                if (!Regex.IsMatch(texto, @"^[0-9,]*$"))
                    e.CancelCommand();
            }
            else
            {
                e.CancelCommand();
            }
        }
    }
}
