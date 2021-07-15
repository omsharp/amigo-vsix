using Configurations.UI.ViewModels.Comments;
using System.Linq;
using Xunit;

namespace WPF.Tests.Comments.ViewModels
{
   public class ClassificationDetailsViewModel_Delete
   {
      [Fact(DisplayName = "Delete Classification")]
      public void Delete_Yes()
      {
         var config = Mocker.GetConfig();
         var viewModel = new ClassificationsPageViewModel(config, Mocker.GetDialogService(true));

         viewModel.Classifications.First().DeleteCommand.Execute(null);

         Assert.Empty(viewModel.Classifications);
         Assert.Empty(config.Classifications);
      }

      [Fact(DisplayName = "Delete Classification - Cancel")]
      public void Delete_No()
      {
         var config = Mocker.GetConfig();
         var viewModel = new ClassificationsPageViewModel(config, Mocker.GetDialogService(false));

         viewModel.Classifications.First().DeleteCommand.Execute(null);

         Assert.Single(viewModel.Classifications);
         Assert.Single(config.Classifications);
      }
   }
}
