using Configurations.UI.Dialogs;
using Mvvm.Core.Command;

namespace Configurations.UI.ViewModels.Dialogs
{
   public class ConfirmationDialogViewModel : DialogViewModelBase<bool>
   {
      public RelayCommand<IDialogWindow> YesCommand { get; set; }
      public RelayCommand<IDialogWindow> NoCommand { get; set; }

      public ConfirmationDialogViewModel(string title = "", string message = "")
      {
         Title = title;
         Message = message;

         YesCommand = new RelayCommand<IDialogWindow>((w) => CloseDialogWithResult(w, true));
         NoCommand = new RelayCommand<IDialogWindow>((w) => CloseDialogWithResult(w, false));
      }
   }

}
