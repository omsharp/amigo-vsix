using System;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text.Classification;
using VSIX.ConfigurationsService;
using System.ComponentModel.Composition;
using Task = System.Threading.Tasks.Task;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio;
using EnvDTE;
using System.Threading.Tasks;
using Microsoft;
using EnvDTE80;
using VSIX.Package.Comments.UI;
using VSIX.Package.Utils;
using VSIX.Package.Commands;
using Configurations.Core.Comments;
using System.Windows.Media;
using Microsoft.VisualStudio.Utilities;

namespace VSIX.Package
{
   static class DteRefs
   {
      public static DTE DTE;
      public static DTE2 DTE2;
      public static Package Package;
      public static IVsSolution Solution;
      public static IVsRunningDocumentTable RDT;
   }


   //[ProvideAutoLoad(VSConstants.UICONTEXT.FolderOpened_string, PackageAutoLoadFlags.BackgroundLoad)]
   [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionOpening_string, PackageAutoLoadFlags.BackgroundLoad)]
   //[ProvideAutoLoad(VSConstants.UICONTEXT.CSharpProject_string, PackageAutoLoadFlags.BackgroundLoad)]
   //[ProvideAutoLoad(VSConstants.UICONTEXT.VBProject_string, PackageAutoLoadFlags.BackgroundLoad)]
   //[ProvideAutoLoad(VSConstants.UICONTEXT.FSharpProject_string, PackageAutoLoadFlags.BackgroundLoad)]
   //[ProvideAutoLoad(VSConstants.UICONTEXT.VCProject_string, PackageAutoLoadFlags.BackgroundLoad)]
   [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
   [ProvideMenuResource("Menus.ctmenu", 1)]
   [ProvideToolWindow(typeof(CommentsListToolWindow))]
   [Guid(Guids.Package_String)]
   public sealed class Package : AsyncPackage
   {

      [Import]
      private IClassificationTypeRegistryService clsService;

      private IVsSolution solService;

      private readonly ICommentConfiguration commentConfigs
         = ConfigService.Current.CommentConfiguration;

      protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
      {

         await base.InitializeAsync(cancellationToken, progress);


         //ToDo move this down - just before command initializations
         // When initialized asynchronously, the current thread may be a background thread at this point.
         // Do any initialization that requires the UI thread after switching to the UI thread.
         await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

         var dte = (DTE)await GetServiceAsync(typeof(DTE));
         Assumes.Present(dte);

         var dte2 = (DTE2)await GetServiceAsync(typeof(SDTE));
         Assumes.Present(dte2);

         solService = await GetServiceAsync(typeof(SVsSolution)) as IVsSolution;
         Assumes.Present(solService);

         var rdt = await GetServiceAsync(typeof(SVsRunningDocumentTable)) as IVsRunningDocumentTable;
         Assumes.Present(rdt);

         //! Set globals
         DteRefs.Package = this;
         DteRefs.DTE = dte;
         DteRefs.DTE2 = dte2;
         DteRefs.Solution = solService;
         DteRefs.RDT = rdt;

         
         //! Activate MEF
         ActivateMEF();

         //! Get VS editor's background color
         var vsProperties = dte.Properties["FontsAndColors", "TextEditor"];
         var fontsAndColors = vsProperties.Item("FontsAndColorsItems").Object as FontsAndColorsItems;
         var plainText = fontsAndColors.Item("Plain Text");

         //! Set VS editor's background in configs
         var bg = ColorExtensions.FromInt((int)plainText.Background);
         commentConfigs.Defaults.VSBackground = Color.FromArgb(255, bg.R, bg.G, bg.B);

         //! Register classifications
         //? This might be redundant!
         clsService.SyncWithConfigs();
         

         //! Hook events
         solService.AdviseSolutionEvents(ViewModelLocator.Instance, out var cookie);

         var events = dte2.Events as Events2;
         if (events != null)
            ViewModelLocator.Instance.SubscribeToEvents(events);

         //! Handle solution load
         if (await IsSolutionLoadedAsync())
         {
            //todo Remove this if not used
            
            //! Only handle it if it's solution, and ignore it if it's a folder.
            //solService.GetProperty((int)__VSPROPID.VSPROPID_IsSolutionOpen, out var value);
            //if (await IsSolutionLoadedAsync())
               //HandleOpenSolution();

           // ViewModelLocator.Instance.Scanner.ScanSolution();
         }

         await BookmarksListCommand.InitializeAsync(this);
         await SettingsCommand.InitializeAsync(this);
      }
      
      private async Task<bool> IsSolutionLoadedAsync()
      {
         await JoinableTaskFactory.SwitchToMainThreadAsync();

         ErrorHandler.ThrowOnFailure(solService.GetProperty((int)__VSPROPID.VSPROPID_IsSolutionOpen, out var value));

         return value is bool isSolOpen && isSolOpen;
      }

      private void HandleOpenSolution(object sender = null, EventArgs e = null)
      {
         ThreadHelper.ThrowIfNotOnUIThread();

      }


      #region MEF Activation Stuff
      private static IComponentModel compositionService = null;

      //! Call this in InitializeAsync to activate MEF
      private void ActivateMEF()
      {
         ThreadHelper.ThrowIfNotOnUIThread();

         if (compositionService == null)
            compositionService = ServiceProvider.GlobalProvider.GetService(typeof(SComponentModel)) as IComponentModel;

         if (compositionService != null)
            compositionService.DefaultCompositionService.SatisfyImportsOnce(this);
      }
      #endregion
   }
}
