using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System.Collections.Generic;
using System;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using System.Linq;
using VSIX.ConfigurationsService;

namespace VSIX.Package.Comments.UI.ViewModels
{
   public class WarningList
   {
      private readonly ErrorListProvider errorListProvider;
      private readonly List<ErrorTask> errorTasks = new List<ErrorTask>();

      public WarningList()
      {
         errorListProvider = new ErrorListProvider(DteRefs.Package)
         {
            ProviderName = "Amigo Warnings Provider",
            ProviderGuid = new Guid("86B98112-1BAF-4AD2-8E04-63366C455B21")
         };

         //errorListProvider.Show();

         ConfigService.Current.CommentConfiguration.ChangesSaved += CommentConfiguration_ChangesSaved;
      }

      private void CommentConfiguration_ChangesSaved(object sender, EventArgs e)
      {
         SyncWarnings();
      }

      public void SyncWarnings()
      {
         ClearWarnings();

         foreach (var proj in ViewModelLocator.Instance.BookmarksListViewModel.Projects)
         {
            foreach (var file in proj.Files)
            {
               foreach (var mark in file.Bookmarks.Where(b => b.Classification.RaiseWarning))
               {
                  AddWarning(proj.Project, mark.FilePath, mark.Content, mark.Line, mark.Column);
               }
            }
         }
      }

      private void AddWarning(Project project, string fileName, string message, int line, int column)
      {
         ThreadHelper.ThrowIfNotOnUIThread();

         ErrorHandler.ThrowOnFailure(
            DteRefs.Solution.GetProjectOfUniqueName(
               project.UniqueName, out IVsHierarchy projectHierarchy));

         var errorTask = new ErrorTask
         {
            ErrorCategory = TaskErrorCategory.Warning,
            HierarchyItem = projectHierarchy,
            Document = fileName,
            
            // VS uses indexes starting at 0 while the automation model uses indexes starting at 1
            Line = line - 1,
            Column = column,
            Text = message
         };

         errorTask.Navigate += NavigateTo;
         errorTasks.Add(errorTask);
         errorListProvider.Tasks.Add(errorTask);
      }

      private void ClearWarnings()
      {
         foreach (var item in errorTasks)
         {
            RemoveTask(item);
         }

         errorTasks.Clear();
      }

      private void RemoveTask(ErrorTask item)
      {
         ////try
         {
            errorListProvider.SuspendRefresh();
            item.Navigate -= NavigateTo;
            //errorTasks.Remove(objErrorTask);
            errorListProvider.Tasks.Remove(item);
            ////}
            ////catch (Exception)
            ////{
            ////   //// MessageBox.Show(objException.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            ////}
            ////finally
            ////{
            errorListProvider.ResumeRefresh();
            //errorListProvider.Show();
         }
      }

      private void NavigateTo(object sender, EventArgs e)
      {
         var errorTask = (ErrorTask)sender;
         errorTask.Line += 1; // Fix the index start
         var bResult = errorListProvider.Navigate(errorTask, new Guid(EnvDTE.Constants.vsViewKindCode));
         errorTask.Line -= 1; // Restore the index start
      }
   }
}
