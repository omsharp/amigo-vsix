using System.Windows;
using System.Windows.Media.Effects;

namespace Configurations.UI.Dialogs
{
   public class DialogService : IDialogService
   {
      readonly Window owner;

      public DialogService(Window owner)
      {
         this.owner = owner;
      }

      public T OpenDialog<T>(DialogViewModelBase<T> viewModel)
      {
         owner.Opacity = 0.6;
         owner.Effect = new BlurEffect { Radius = 4 };

         new DialogWindow
         {
            DataContext = viewModel,
            Owner = owner

         }.ShowDialog();

         owner.Opacity = 1;
         owner.Effect = null;

         return viewModel.DialogResult;
      }
   }

}
