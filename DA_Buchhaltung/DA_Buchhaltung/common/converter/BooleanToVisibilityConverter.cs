/*
 * Klasse: BooleanToVisibilityConverter.cs
 * Author: Martin Osterwalder >> Hilfe von Projektwoche Projekt (BioBS) (Hauptersteller Imanuel Näf, Mitstudent HFU) 
 * Konvertiert ein bool Property zu Visibility Value (True = Visible , False = Collapsed)
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace DA_Buchhaltung.common.converter
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (value is bool && (bool)value) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
