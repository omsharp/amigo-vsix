using Configurations.Core;
using Configurations.UI.ViewModels.Comments;
using Xunit;

namespace WPF.Tests.Comments.ViewModels
{
   public class StylesPageViewModel_Add
   {
      [Fact(DisplayName = "Add New Style")]
      public void Add_Ok()
      {
         var config = Mocker.GetConfigEmpty();

         var viewModel = new StylesPageViewModel(config, Mocker.GetDialogService(true));

         viewModel.AddCommand.Execute(null);

         Assert.Single(viewModel.Styles);
         Assert.Single(config.Styles);
      }
   }
}
