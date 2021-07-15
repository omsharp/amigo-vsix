using System.Collections.ObjectModel;
using Xunit;
using Moq;
using Configurations.Core.Comments;
using VSIX.Package.Comments.Parsers;

namespace VSIX.Tests.CommentExtraction.Markup
{
   public class HtmlCommentExtractor_Tests
   {
      const string TODO_TOKEN = "todo";
      const string COMMENT_CONTENT = "This is a comment";

      readonly string fullComment;
      readonly HtmlCommentExtractor extractor;

      public HtmlCommentExtractor_Tests()
      {
         var configMock = new Mock<ICommentConfiguration>();

         configMock.Setup(c => c.Classifications).Returns(
            new ObservableCollection<Classification>
            {
               new Classification { Name = "Todo", Token = TODO_TOKEN},
               new Classification { Name = "Hack", Token = "hack"},
               new Classification { Name = "Bug", Token = "bug"},

            });

         extractor = new HtmlCommentExtractor(configMock.Object);

         fullComment = $"' {TODO_TOKEN} {COMMENT_CONTENT}";
      }

      [Fact]
      public void Test1()
      {
         Assert.True(false);
      }
   }
}
