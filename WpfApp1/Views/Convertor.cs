using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace WpfApp1.Views
{
    using System.Windows;

    internal class Convertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is List<int> numbers)
            {
                return string.Join(", ", numbers);
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string numbers))
                return string.Empty;

            try
            {
                return numbers.Split(',').Select(int.Parse).ToList();
            }
            catch
            {
                MessageBox.Show("Недопустимое значение");
            }

            return string.Empty;
        }
    }
}
