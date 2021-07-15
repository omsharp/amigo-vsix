using Configurations.UI.ViewModels;
using System;
using System.Windows;
using XB = Microsoft.Xaml.Behaviors.Core;

namespace Configurations.UI
{
   public partial class MainWindow : Window
   {
      public static MainWindow CurrentInstance { get; private set; }

      public MainWindow()
      {
         if (CurrentInstance != null)
            throw new InvalidOperationException($"Only one instance of {nameof(MainWindow)} is allowed.");

         //! This is a work around to force load Microsoft.Xaml.Behaviors assembly.
         //! Removing this causes an exception when constructed in another assembly.
         _ = new XB.ConditionCollection();

         InitializeComponent();
         CurrentInstance = this;
      }

      protected override void OnClosed(EventArgs e)
      {
         // Clean CurrentInstance after window is closed.
         CurrentInstance = null;
         base.OnClosed(e);
      }

      //! SelectedItemChanged event handler.
      //! Get the bound TreeItemViewModel and then execute it's command.
      private void NavigationTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
         => (NavigationTree.SelectedItem as TreeItemViewModel)?.OnClickCommand?.Execute(this);

   }
}
