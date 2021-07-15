using System;
using System.Globalization;
using System.Windows.Data;

namespace Configurations.UI.Converters
{
   public class ReverseBooleanValueConverter : IValueConverter
   {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
         if(value is bool b)
            return !b;

         throw new Exception($"Invalid data type : {value}");
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
         throw new NotImplementedException();
      }
   }
}
