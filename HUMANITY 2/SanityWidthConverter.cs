using System;
using System.Globalization;
using System.Windows.Data;

namespace HumanityWPF
{
    public class SanityWidthConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 &&
                values[0] is double percentage &&
                values[1] is double totalWidth)
            {
                // Oblicz szerokość paska na podstawie procentu sanity
                return totalWidth * percentage;
            }
            return 0.0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}