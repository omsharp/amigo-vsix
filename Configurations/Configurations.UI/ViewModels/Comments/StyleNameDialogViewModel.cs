using Configurations.UI.Dialogs;
using Mvvm.Core.Command;
using System.ComponentModel;
using System.Linq;

namespace Configurations.UI.ViewModels.Comments
{
   public class StyleNameDialogViewModel : DialogViewModelBase<bool>, IDataErrorInfo
   {
      private readonly StyleViewModel style;
      private readonly DialogType dialogType;

      public string Name
      {
         get => style.Name;
         set => style.Name = value;
      }

      public string Error { get; private set; }

      public string this[string property]
      {
         get
         {
            Error = string.Empty;

            if (string.IsNullOrEmpty(Name))
               Error = "Required field.";
            else if (IsNameTaken())
               Error = "Style with the same name already exists.";

            OkCommand.RaiseCanExecuteChanged();

            return Error;
         }
      }

      public RelayCommand<IDialogWindow> OkCommand { get; private set; }
      public RelayCommand<IDialogWindow> CancelCommand { get; private set; }

      public StyleNameDialogViewModel(StyleViewModel source, string title, DialogType type)
      {
         style = source;
         Title = title;
         dialogType = type;

         OkCommand = new RelayCommand<IDialogWindow>(w => CloseDialogWithResult(w, true), CanExecute);
         CancelCommand = new RelayCommand<IDialogWindow>(w => CloseDialogWithResult(w, false));
      }

      private bool IsNameTaken()
      {
         var n = dialogType == DialogType.AddNew ? 0 : 1;
         return style.Owner.Styles.Count(s => s.Name == style.Name) > n;
      }

      private bool CanExecute(IDialogWindow window)
         => string.IsNullOrEmpty(Error);
   }
}
