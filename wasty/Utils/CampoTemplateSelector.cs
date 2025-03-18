using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Controls;
using wasty.ViewModels;
using wasty.Models;

namespace wasty.Utils {
    public class CampoTemplateSelector : DataTemplateSelector
        {
            public DataTemplate TextoTemplate { get; set; }
            public DataTemplate NumeroTemplate { get; set; }
            public DataTemplate FechaTemplate { get; set; }
            public DataTemplate PickerTemplate { get; set; }
            public DataTemplate CheckboxTemplate { get; set; }

            public override DataTemplate SelectTemplate(object item, DependencyObject container)
            {
                if (item is CampoFormulario campo)
                {
                    return campo.Tipo switch
                    {
                        "Texto" => TextoTemplate,
                        "Número" => NumeroTemplate,
                        "Fecha" => FechaTemplate,
                        "Picker" => PickerTemplate,
                        "Checkbox" => CheckboxTemplate,
                        _ => TextoTemplate
                    };
                }
                return base.SelectTemplate(item, container);
            }
        }
    }
