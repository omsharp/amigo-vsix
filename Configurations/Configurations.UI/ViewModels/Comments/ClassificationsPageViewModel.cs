using Configurations.Core.Comments;
using Configurations.UI.Dialogs;
using Mvvm.Core;
using Mvvm.Core.Command;
using System.Collections.ObjectModel;

namespace Configurations.UI.ViewModels.Comments
{
   public class ClassificationsPageViewModel : ViewModelBase, IPageViewModel
   {
      public ICommentConfiguration Config { get; }
      public IDialogService DialogService { get; }

      public string Title => "Classifications";

      public ObservableCollection<ClassificationViewModel> Classifications { get; set; }
         = new ObservableCollection<ClassificationViewModel>();

      public RelayCommand AddCommand { get; set; }
      public ClassificationsPageViewModel(ICommentConfiguration config, IDialogService dialogService)
      {
         Config = config;
         DialogService = dialogService;

         foreach (var cls in Config.Classifications)
            Classifications.Add(new ClassificationViewModel(cls, this));

         AddCommand = new RelayCommand(ShowNewClassificationDialog);
      }

      public void ShowNewClassificationDialog()
      {
         var newCls = new Classification();
         var newClsVm = new ClassificationViewModel(newCls, this);
         var dialogVm = new ClassificationDetailsDialogViewModel(newClsVm, "New Classification", DialogType.AddNew);

         var isOk = DialogService.OpenDialog(dialogVm);
         if (isOk)
         {
            Classifications.Add(newClsVm);
            Config.Classifications.Add(newCls);
         }
      }
   }
}
