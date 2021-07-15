namespace Configurations.UI.Dialogs
{
   public interface IDialogService
   {
      T OpenDialog<T>(DialogViewModelBase<T> viewModel);
   }

}
