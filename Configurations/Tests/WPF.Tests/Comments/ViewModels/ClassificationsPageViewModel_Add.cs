using Configurations.UI.ViewModels.Comments;
using Xunit;

namespace WPF.Tests.Comments.ViewModels
{
   public class ClassificationsPageViewModel_Add
   {
      [Fact(DisplayName = "Add New Classification")]
      public void AddClassification_Yes()
      {
         var config = Mocker.GetConfig();
         var viewModel = new ClassificationsPageViewModel(config, Mocker.GetDialogService(true));

         var configCount = config.Styles.Count + 1;
         var vmCount = viewModel.Classifications.Count + 1;

         viewModel.AddCommand.Execute(null);

         Assert.Equal(configCount, config.Classifications.Count);
         Assert.Equal(vmCount, viewModel.Classifications.Count);
      }
   }
}
