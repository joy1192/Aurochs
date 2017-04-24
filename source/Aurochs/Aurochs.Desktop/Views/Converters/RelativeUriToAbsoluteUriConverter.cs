using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Aurochs.Desktop.Views.Converters
{
    public class RelativeUriToAbsoluteUriConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string uri)
            {
                if (Uri.IsWellFormedUriString(uri, UriKind.Absolute))
                    return uri;

                if (Uri.TryCreate(uri, UriKind.Relative, out Uri result))
                {
                    Uri fullUri = new Uri(new Uri("https://friends.nico/"), result);
                    return fullUri.ToString();
                }
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
