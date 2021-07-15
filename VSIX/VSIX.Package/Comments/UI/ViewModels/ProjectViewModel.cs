using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Mvvm.Core;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using VSIX.Package.Utils;

namespace VSIX.Package.Comments.UI.ViewModels
{
   public class ProjectViewModel : ViewModelBase
   {
      private string classificationFilter;

      public Project Project { get; private set; }

      public string ProjectName { get; set; }

      //public BookmarkViewModel SelectedComment { get; set; }

      public ObservableCollection<FileViewModel> Files { get; set; }

      public string ClassificationFilter
      {
         get => classificationFilter;
         set
         {
            classificationFilter = value;
            foreach (var file in Files)
               file.ClassificationFilter = value;
         }
      }

      public ProjectViewModel(Project source, ObservableCollection<FileViewModel> files)
      {
         ThreadHelper.ThrowIfNotOnUIThread();

         Project = source;
         ProjectName = source.Name;
         this.Files = files;
      }
   }

   public class FileViewModel : ViewModelBase
   {
      readonly ObservableCollection<BookmarkViewModel> bookmarks;

      public string FilePath { get; set; }
      public string FileName => Path.GetFileName(FilePath);

      public string ClassificationFilter { get; set; }

      public ObservableCollection<BookmarkViewModel> Bookmarks
      {
         get
         {
            if (string.IsNullOrEmpty(ClassificationFilter))
               return bookmarks;

            return bookmarks.Filter(b => b.Classification.Name == ClassificationFilter);
         }
      }

      public FileViewModel(string filePath, ObservableCollection<BookmarkViewModel> comments)
      {
         FilePath = filePath;
         bookmarks = comments;
      }
   }
}
