using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace VirtualSales.Wpf.Converters
{
    public class FriendlyTimeSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var ts = (TimeSpan)value;
            int years = ts.Days / 365; //no leap year accounting
            int months = (ts.Days % 365) / 30; //naive guess at month size
            int weeks = ((ts.Days % 365) % 30) / 7;
            int days = (((ts.Days % 365) % 30) % 7);

            StringBuilder sb = new StringBuilder();
            if (years > 0)
            {
                sb.Append(years.ToString() + " years, ");
            }
            if (months > 0)
            {
                sb.Append(months.ToString() + " months, ");
            }
            if (weeks > 0)
            {
                sb.Append(weeks.ToString() + " weeks, ");
            }
            if (days > 0)
            {
                sb.Append(days.ToString() + " days.");
            }
            return sb.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
