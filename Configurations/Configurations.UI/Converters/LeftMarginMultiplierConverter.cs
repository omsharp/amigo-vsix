using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Configurations.UI.Converters
{
   public class LeftMarginMultiplierConverter : IValueConverter
   {
      public double Length { get; set; }

      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
         if (!(value is TreeViewItem item))
            return new Thickness(0);

         return new Thickness(Length * GetTreeItemDepth(item), 0, 0, 0);
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
         throw new NotImplementedException();
      }

      private static int GetTreeItemDepth(TreeViewItem item)
      {
         while (GetParent(item) is TreeViewItem parent && parent != null)
            return GetTreeItemDepth(parent) + 1;
         
         return 0;
      }

      private static TreeViewItem GetParent(TreeViewItem item)
      {
         var parent = VisualTreeHelper.GetParent(item);

         while (!(parent is TreeViewItem || parent is TreeView))
            parent = VisualTreeHelper.GetParent(parent);

         return parent as TreeViewItem;
      }
   }
}
