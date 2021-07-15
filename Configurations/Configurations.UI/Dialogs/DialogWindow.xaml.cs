using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace Configurations.UI.Dialogs
{
   public partial class DialogWindow : Window, IDialogWindow
   {
      public DialogWindow()
      {
         InitializeComponent();
      }
   }
}
