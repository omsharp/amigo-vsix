using Mvvm.Core;
using Mvvm.Core.Command;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Configurations.UI.ViewModels
{
   public class TreeItemViewModel : ViewModelBase
   {
      public List<TreeItemViewModel> Items { get; set; }
      public string Header { get; set; }
      public ICommand OnClickCommand { get; private set; }

      public TreeItemViewModel(string header)
      {
         Header = header;
      }

      public TreeItemViewModel(string header, Action onClick)
         : this(header)
      {
         OnClickCommand = new RelayCommand(onClick);
      }
   }
}
