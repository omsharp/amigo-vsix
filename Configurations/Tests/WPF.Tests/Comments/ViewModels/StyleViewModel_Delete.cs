using Configurations.UI.ViewModels.Comments;
using System.Linq;
using Xunit;

namespace WPF.Tests.Comments.ViewModels
{
   public class StyleViewModel_Delete
   {
      [Fact(DisplayName = "Delete Style")]
      public void Delete_Yes()
      {
         var config = Mocker.GetConfig_WithUnattachedStyle();
         var viewModel = new StylesPageViewModel(config,
            Mocker.GetDialogService(true));

         viewModel.Styles.First().DeleteCommand.Execute(null);

         Assert.Empty(viewModel.Styles);
         Assert.Empty(config.Styles);
      }


      [Fact(DisplayName = "Delete Style - Cancel")]
      public void Delete_No()
      {
         var config = Mocker.GetConfig_WithUnattachedStyle();
         var viewModel = new StylesPageViewModel(config,
            Mocker.GetDialogService(false));

         viewModel.Styles.First().DeleteCommand.Execute(null);

         Assert.Single(viewModel.Styles);
         Assert.Single(config.Styles);
      }
   }
}
