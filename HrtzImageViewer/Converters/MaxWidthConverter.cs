using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HrtzImageViewer.Converters
{
    public class MaxWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int) value > SystemParameters.WorkArea.Width ? SystemParameters.WorkArea.Width : (int) value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
