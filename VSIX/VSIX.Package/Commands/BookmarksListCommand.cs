using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using VSIX.Package.Comments.UI;
using Task = System.Threading.Tasks.Task;

namespace VSIX.Package.Commands
{
   internal sealed class BookmarksListCommand
   {
      private readonly AsyncPackage package;

      public const int COMMAND_ID = 0x0100;

      private BookmarksListCommand(AsyncPackage package, OleMenuCommandService commandService)
      {
         this.package = package ?? throw new ArgumentNullException(nameof(package));
         commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

         var menuCommandID = new CommandID(Guids.CommandSet, COMMAND_ID);
         var menuItem = new MenuCommand(Execute, menuCommandID);
         commandService.AddCommand(menuItem);
      }

      public static BookmarksListCommand Instance { get; private set; }

      public static async Task InitializeAsync(AsyncPackage package)
      {
         await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

         var commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
         Instance = new BookmarksListCommand(package, commandService);
      }

      private void Execute(object sender, EventArgs e)
      {
         ThreadHelper.ThrowIfNotOnUIThread();

         var window = package.FindToolWindow(typeof(CommentsListToolWindow), 0, true);
         if (null == window || null == window.Frame)
            throw new NotSupportedException("Cannot create tool window");

         var windowFrame = (IVsWindowFrame)window.Frame;
         Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(windowFrame.Show());
      }
   }
}
