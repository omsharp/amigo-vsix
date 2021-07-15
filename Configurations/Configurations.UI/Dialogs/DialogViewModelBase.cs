using Mvvm.Core;

namespace Configurations.UI.Dialogs
{
   public abstract class DialogViewModelBase<T> : ViewModelBase
   {
      public string Title { get; set; }
      public string Message { get; set; }
      public T DialogResult { get; set; }

      public void CloseDialogWithResult(IDialogWindow dialog, T result)
      {
         DialogResult = result;

         if (dialog != null)
            dialog.DialogResult = true;
      }
   }

}
