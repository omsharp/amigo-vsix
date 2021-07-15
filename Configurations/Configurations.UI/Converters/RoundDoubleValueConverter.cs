using System;
using System.Globalization;
using System.Windows.Data;

namespace Configurations.UI.Converters
{
   public class RoundDoubleValueConverter : IValueConverter
   {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
         => RoundValue(value, parameter);

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
         => RoundValue(value, parameter);
      
      private double RoundValue(object value, object parameter)
      {
         if (value is double d && int.TryParse(parameter.ToString(), out var i))
            return Math.Round(d, i);

         throw new Exception($"Invalid data type : {value}");
      }
   }
}
