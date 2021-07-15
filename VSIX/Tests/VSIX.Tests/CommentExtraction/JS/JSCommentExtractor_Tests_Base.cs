using Configurations.Core.Comments;
using Moq;
using System.Collections.ObjectModel;
using VSIX.Package.Comments.Parsers;

namespace VSIX.Tests.CommentExtraction.JS
{
   public class JSCommentExtractor_Tests_Base
   {
      protected const string TODO_TOKEN = "todo";
      protected const string COMMENT_CONTENT = "This is a comment";
      protected readonly string FullComment;

      protected readonly JSCommentExtractor JSExtractor;

      public JSCommentExtractor_Tests_Base()
      {
         var configMock = new Mock<ICommentConfiguration>();

         configMock.Setup(c => c.Classifications).Returns(
            new ObservableCollection<Classification>
            {
               new Classification { Name = "Todo", Token = TODO_TOKEN},
               new Classification { Name = "Hack", Token = "hack"},
               new Classification { Name = "Bug", Token = "bug"},

            });

         JSExtractor = new JSCommentExtractor(configMock.Object);

         FullComment = $"// {TODO_TOKEN} {COMMENT_CONTENT}";
      }
   }
}
