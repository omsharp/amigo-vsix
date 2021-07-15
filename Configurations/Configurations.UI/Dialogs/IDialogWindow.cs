using System.Windows;

namespace Configurations.UI.Dialogs
{
   public interface IDialogWindow
   {
      bool? DialogResult { get; set; }
      object DataContext { get; set; }

      Window Owner { get; set; }

      bool? ShowDialog();
   }

}
