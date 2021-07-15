using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;

namespace VSIX.Package.Commands
{
   internal sealed class SettingsCommand
   {
      private readonly AsyncPackage package;

      public const int COMMAND_ID = 0x0200;

      private SettingsCommand(AsyncPackage package, OleMenuCommandService commandService)
      {
         this.package = package ?? throw new ArgumentNullException(nameof(package));
         commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

         var menuCommandID = new CommandID(Guids.CommandSet, COMMAND_ID);
         var menuItem = new MenuCommand(Execute, menuCommandID);
         commandService.AddCommand(menuItem);
      }

      public static SettingsCommand Instance { get; private set; }

      public static async Task InitializeAsync(AsyncPackage package)
      {
         await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

         var commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
         Instance = new SettingsCommand(package, commandService);
      }

      private void Execute(object sender, EventArgs e)
      {
         ThreadHelper.ThrowIfNotOnUIThread();

         _ = new Configurations.UI.MainWindow().ShowDialog();
      }
   }
}
