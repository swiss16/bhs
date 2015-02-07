using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DA_Buchhaltung.common.converter
{
    public class RabattConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var rabatt = (decimal)values[0];//1. Wert = Rabett (decimal-Wert)
            var typ = (bool)values[1];//2. Wert = RabattInProzent

            if (typ)
            {
                return string.Format("{0:n2} %", rabatt);
            }
            else
            {
                return string.Format("Fr. {0:n2}.-", rabatt);
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
