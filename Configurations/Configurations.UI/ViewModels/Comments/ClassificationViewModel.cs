using Configurations.Core.Comments;
using Configurations.UI.Dialogs;
using Configurations.UI.ViewModels.Dialogs;
using Mvvm.Core;
using Mvvm.Core.Command;
using System.Linq;

namespace Configurations.UI.ViewModels.Comments
{
   public class ClassificationViewModel : ViewModelBase
   {
      readonly Classification classification;

      public ClassificationsPageViewModel Owner { get; }

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

      public RelayCommand DeleteCommand { get; }
      public RelayCommand EditCommand { get; }

      public ClassificationViewModel(Classification source, ClassificationsPageViewModel owner)
      {
         classification = source;
         Owner = owner;
         DeleteCommand = new RelayCommand(Remove);
         EditCommand = new RelayCommand(Edit);
      }

      private void Remove()
      {
         var message = $"The ({Name}) classification will be permanently deleted.";
         var dialogVm = new ConfirmationDialogViewModel("Delete", message);
         var isOk = Owner.DialogService.OpenDialog(dialogVm);
         if (isOk)
         {
            Owner.Classifications.Remove(this);
            Owner.Config.Classifications.Remove(
               Owner.Config.Classifications.Single(c => c.Name == Name));
         }
      }

      private void Edit()
      {
         var oldName = Name;
         var oldToken = Token;
         var oldStyle = Style;
         var oldCap = Capitalization;
         var oldApp = Application;

         var dialogVm = new ClassificationDetailsDialogViewModel(this, $"Edit Classification [{Name}]", DialogType.Edit);

         var isOk = Owner.DialogService.OpenDialog(dialogVm);
         if (!isOk)
         {
            Name = oldName;
            Token = oldToken;
            Style = oldStyle;
            Capitalization = oldCap;
            Application = oldApp;
         }
      }
   }
}
