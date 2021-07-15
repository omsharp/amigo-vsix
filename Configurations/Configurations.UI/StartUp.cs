using System;
using System.Windows;

namespace Configurations.UI
{
   public class StartUp : Application
   {
      [STAThread()]
      public static void Main()
      {
         _ = new StartUp
         {
            StartupUri = new Uri("MainWindow.xaml", UriKind.Relative)
         }.Run();
      }
   }
}
