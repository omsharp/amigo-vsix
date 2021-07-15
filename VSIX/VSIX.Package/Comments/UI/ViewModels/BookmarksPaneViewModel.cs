using Configurations.Core.Comments;
using Mvvm.Core;
using Mvvm.Core.Command;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace VSIX.Package.Comments.UI.ViewModels
{
   public sealed class BookmarksPaneViewModel : ViewModelBase
   {
      readonly ICommentConfiguration config;
      readonly BookmarkScanner scanner;
      private string selectedClassificationFilter;

      public ObservableCollection<ProjectViewModel> Projects => scanner.Projects;

      public ObservableCollection<string> ClassificationFilters { get; }
         = new ObservableCollection<string>();

      public RelayCommand ScanSolutionCommand { get; set; }

      public string SelectedClassificationFilter
      {
         get => selectedClassificationFilter;
         set
         {
            selectedClassificationFilter = value;

            if (value == "Any")
               scanner.ClassificationFilter = string.Empty;
            else
               scanner.ClassificationFilter = value;
         }
      }

      public BookmarksPaneViewModel(ICommentConfiguration configs, BookmarkScanner scanner)
      {
         config = configs;
         configs.ChangesSaved += Configs_ChangesSaved;

         this.scanner = scanner;

         ScanSolutionCommand = new RelayCommand(scanner.ScanSolution, () => true);

         SetClassificationFilters();

         SelectedClassificationFilter = "Any";
      }

      private void SetClassificationFilters()
      {
         var selectedFilter = SelectedClassificationFilter;

         ClassificationFilters.Clear();
         ClassificationFilters.Add("Any");

         foreach (var cls in config.Classifications)
            ClassificationFilters.Add(cls.Name);

         // To Rest the selected filter in the view
         SelectedClassificationFilter 
            = ClassificationFilters.Contains(selectedFilter) 
            ? selectedFilter 
            : "Any";
      }

      private void Configs_ChangesSaved(object sender, System.EventArgs e)
      {
         SetClassificationFilters();
      }
   }
}
