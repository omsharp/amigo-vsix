using Configurations.Core;
using Mvvm.Core;
using Mvvm.Core.Command;
using System.Collections.Generic;

namespace Configurations.UI.ViewModels
{
   public class MainViewModel : ViewModelBase
   {
      readonly ViewModelLocator vmLocator = ViewModelLocator.Instance;
      readonly IConfigurationCollection configs;

      public IPageViewModel CurrentPageViewModel { get; set; }
      public TreeItemViewModel SelectedItem { get; set; }
      public List<TreeItemViewModel> NavigationTreeItems { get; }

      public RelayCommand<MainWindow> CancelCommand
         => new RelayCommand<MainWindow>(ResetAndClose, (w) => true);

      public RelayCommand<MainWindow> SaveCommand
         => new RelayCommand<MainWindow>(SaveAndClose, (w) => true);

      public MainViewModel(IConfigurationCollection configCollection)
      {
         configs = configCollection;
         NavigationTreeItems = BuildNavigationTree();
         SelectedItem = NavigationTreeItems[0].Items[0];
      }

      private void SaveAndClose(MainWindow window)
      {
         configs.SaveAllChanges();
         window.Close();
         ViewModelLocator.Cleanup();
      }

      private void ResetAndClose(MainWindow window)
      {
         configs.LoadAll();
         window.Close();
         ViewModelLocator.Cleanup();
      }

      private List<TreeItemViewModel> BuildNavigationTree()
      {
         return new List<TreeItemViewModel> //! ROOT
         {
            new TreeItemViewModel("Comments")
            {
               Items = new List<TreeItemViewModel>
                     {
                        new TreeItemViewModel("Defaults",() => CurrentPageViewModel = vmLocator.Defaults) ,
                        new TreeItemViewModel("Custom Styles", () => CurrentPageViewModel = vmLocator.Styles),
                        new TreeItemViewModel("Classifications", () => CurrentPageViewModel = vmLocator.Classifications)
                     },
            },

            //new TreeItemViewModel("Line Height")
            //{
            //   Items = new List<TreeItemViewModel>
            //         {
            //            new TreeItemViewModel("Empty Lines"),
            //            new TreeItemViewModel("Regx"),
            //         },
            //},
         };
      }
   }
}
