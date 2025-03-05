using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace wasty.Converters
{
    public class FilterableValuesMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            List<string> lista = new List<string>();
            if (values[0] is ObservableCollection<Dictionary<string, List<string>>>)
            {
                ObservableCollection<Dictionary<string, List<string>>> dict = (ObservableCollection<Dictionary<string, List<string>>>)values[0];
                lista = dict.Where(dict => dict.ContainsKey((string)values[1]))
                                        .SelectMany(dict => dict[(string)values[1]])
                                        .ToList();
            }
            return lista;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
