using Configurations.Core.Comments;
using Configurations.UI.Dialogs;
using Mvvm.Core.Command;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace Configurations.UI.ViewModels.Comments
{
   public class ClassificationDetailsDialogViewModel : DialogViewModelBase<bool>, IDataErrorInfo
   {
      readonly ClassificationViewModel classification;
      readonly DialogType dialogType;

      readonly string stringErrorMsg = "* Required field!\n"
                                     + "* Must contain letters or hyphons (-) only.\n"
                                     + "* Words can be separated by a single hyphen.\n"
                                     + "* Must start and end with an alphabet.\n\n"
                                     + "Ex:\n"
                                     + "   word\n"
                                     + "   word-word\n"
                                     + "   word-word-word";

      public string Name
      {
         get => classification.Name;
         set => classification.Name = value;
      }

      public string Token
      {
         get => classification.Token;
         set => classification.Token = value;
      }

      public CustomStyle Style
      {
         get => classification.Style;
         set => classification.Style = value;
      }

      public CapitalizationType Capitalization
      {
         get => classification.Capitalization;
         set => classification.Capitalization = value;
      }

      public StyleApplication Application
      {
         get => classification.Application;
         set => classification.Application = value;
      }

      public bool RaiseWarning
      {
         get => classification.RaiseWarning;
         set => classification.RaiseWarning = value;
      }

      public ObservableCollection<CustomStyle> Styles
         => classification.Owner.Config.Styles;

      public RelayCommand<IDialogWindow> OkCommand { get; }
      public RelayCommand<IDialogWindow> CancelCommand { get; }

      public string Error { get; private set; }

      public string this[string property]
      {
         get
         {
            Error = string.Empty;

            switch (property)
            {
               case nameof(Name):
                  if (!IsNameValid())
                     Error = stringErrorMsg;
                  else if (IsNameTaken())
                     Error = "Classification with the same name already exists!";
                  break;

               case nameof(Token):
                  if (string.IsNullOrEmpty(Token))
                     Error = "Required Field!";
                  else if (IsTokenTaken())
                     Error = "Token is already in use by another classification!";
                  break;

               case nameof(Style):
                  if (Style == null)
                     Error = "Select a style!";
                  break;
            }

            OkCommand.RaiseCanExecuteChanged();
            return Error;
         }
      }

      public ClassificationDetailsDialogViewModel(ClassificationViewModel source, string title, DialogType type)
      {
         classification = source;
         Title = title;
         dialogType = type;

         OkCommand = new RelayCommand<IDialogWindow>(w => CloseDialogWithResult(w, true), CanExecuteOk);
         CancelCommand = new RelayCommand<IDialogWindow>(w => CloseDialogWithResult(w, false));
      }

      private bool IsNameValid()
         => !string.IsNullOrWhiteSpace(Name)
         && Regex.IsMatch(Name, @"^[a-zA-Z-]+$")
         && !Name.EndsWith("-")
         && !Name.StartsWith("-")
         && !Name.Contains("--");

      private bool IsNameTaken()
      {
         var n = dialogType == DialogType.AddNew ? 0 : 1;
         return classification.Owner.Classifications.Count(c => c.Name.ToLower() == Name.ToLower()) > n;
      }

      private bool IsTokenTaken()
      {
         var n = dialogType == DialogType.AddNew ? 0 : 1;
         return classification.Owner.Classifications.Count(c => c.Token.ToLower() == Token.ToLower()) > n;
      }

      private bool CanExecuteOk(IDialogWindow window)
         => IsNameValid()
         && !IsNameTaken()
         && !IsTokenTaken()
         && !string.IsNullOrWhiteSpace(Token)
         && Style != null;
   }
}
