using Configurations.UI.Dialogs;
using Mvvm.Core.Command;

namespace Configurations.UI.ViewModels.Dialogs
{
   public enum NotificationType
   {
      Information,
      Explanation
   }

   public class NotificationDialogViewModel : DialogViewModelBase<bool>
   {
      public NotificationType Type { get; }

      public RelayCommand<IDialogWindow> OkCommand { get; }

      public NotificationDialogViewModel(string title, string message, NotificationType type)
      {
         Title = title;
         Message = message;
         Type = type;

         OkCommand = new RelayCommand<IDialogWindow>(w => CloseDialogWithResult(w, true));
      }
   }
}
