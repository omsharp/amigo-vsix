using Configurations.Core.Comments;
using Configurations.UI.Dialogs;
using Moq;
using System.Collections.ObjectModel;

namespace WPF.Tests.Comments.ViewModels
{
   internal static class Mocker
   {

      static internal ICommentConfiguration GetConfig()
      {
         var configMock = new Mock<ICommentConfiguration>();
         var styles = new ObservableCollection<CustomStyle> { new CustomStyle { Name = "Style1" } };
         var classifications = new ObservableCollection<Classification>
         {
            new Classification
            {
               Name = "Classifications1",
               Style = styles[0],
               Token = "token"
            }
         };


         configMock.Setup(c => c.Styles).Returns(styles);
         configMock.Setup(c => c.Classifications).Returns(classifications);
         return configMock.Object;
      }

      static internal ICommentConfiguration GetConfig_WithUnattachedStyle()
      {
         var configMock = new Mock<ICommentConfiguration>();
         var styles = new ObservableCollection<CustomStyle> { new CustomStyle { Name = "Style1" } };
         var classifications = new ObservableCollection<Classification>
         {
            new Classification
            {
               Name = "Classifications1",
               Style = new CustomStyle(),
               Token = "token"
            }
         };


         configMock.Setup(c => c.Styles).Returns(styles);
         configMock.Setup(c => c.Classifications).Returns(classifications);
         return configMock.Object;
      }


      static internal ICommentConfiguration GetConfigEmpty()
      {
         var configMock = new Mock<ICommentConfiguration>();
         var data = new ObservableCollection<CustomStyle>();
         configMock.Setup(c => c.Styles).Returns(data);
         return configMock.Object;
      }

      static internal IDialogService GetDialogService<T>(T returnValue)
      {
         var serviceMock = new Mock<IDialogService>();
         serviceMock.Setup(s => s.OpenDialog(It.IsAny<DialogViewModelBase<T>>())).Returns(returnValue);
         return serviceMock.Object;
      }
   }
}
