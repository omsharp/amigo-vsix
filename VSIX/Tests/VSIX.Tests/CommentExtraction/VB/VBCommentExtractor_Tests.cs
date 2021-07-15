using System.Collections.ObjectModel;
using Xunit;
using Moq;
using Configurations.Core.Comments;
using VSIX.Package.Comments.Parsers;

namespace VSIX.Tests.CommentExtraction.CSharp
{
   public class VBCommentExtractor_Tests
   {
      const string TODO_TOKEN = "todo";
      const string COMMENT_CONTENT = "This is a comment";

      readonly string fullComment;
      readonly VBCommentExtractor extractor;

      public VBCommentExtractor_Tests()
      {
         var configMock = new Mock<ICommentConfiguration>();

         configMock.Setup(c => c.Classifications).Returns(
            new ObservableCollection<Classification>
            {
               new Classification { Name = "Todo", Token = TODO_TOKEN},
               new Classification { Name = "Hack", Token = "hack"},
               new Classification { Name = "Bug", Token = "bug"},

            });

         extractor = new VBCommentExtractor(configMock.Object);

         fullComment = $"' {TODO_TOKEN} {COMMENT_CONTENT}";
      }



      [Fact(DisplayName = "Classified comments")]
      public void Comment_LineBeginning_Classified()
      {
         var codeLines = new[]
         {
            "s = \"string\"",
            fullComment,
            "i = 23",
            "c = 234 " + fullComment
         };

         var results = extractor.Extract(codeLines);

         Assert.Equal(2, results.Count);

         Assert.Equal(TODO_TOKEN, results[0].Classification.Token);
         Assert.Equal(TODO_TOKEN, results[1].Classification.Token);

         Assert.Equal(2, results[0].Line);
         Assert.Equal(4, results[1].Line);

         Assert.Equal(COMMENT_CONTENT, results[0].Content);
         Assert.Equal(COMMENT_CONTENT, results[1].Content);
      }

      [Fact(DisplayName = "Comment after a standard string literal")]
      public void Comment_AfterStandardStringLiteral()
      {
         var codeLines = new[]
         {
            "s = \"string\"",
            // k = "// todo Not a comment"; //todo This is a comment
            "k = \"' Not a comment\"" + fullComment,
            "i = 23",
         };

         var results = extractor.Extract(codeLines);

         Assert.Single(results);
         Assert.Equal(TODO_TOKEN, results[0].Classification.Token);
         Assert.Equal(2, results[0].Line);
         Assert.Equal(COMMENT_CONTENT, results[0].Content);
      }
      
      [Fact(DisplayName = "Handle escaped (\")")]
      public void HandleEscapedDoubleQuotes_Verbatim()
      {
         var codeLines = new[]
         {
            // var k = " "" "; //todo This is a comment
            "k = \" \"\" \"" + fullComment
         };

         var results = extractor.Extract(codeLines);

         Assert.Single(results);
         Assert.Equal(TODO_TOKEN, results[0].Classification.Token);
         Assert.Equal(1, results[0].Line);
         Assert.Equal(COMMENT_CONTENT, results[0].Content);
      }

      [Fact(DisplayName = "Ignore comment-like text in a string")]
      public void NoComment_InsideStandardStringLiteral()
      {
         var codeLines = new[]
         {
            "d = 234.3",
            "s = \"' Not a comment\"", // s = "// not a comment"
            "i = 2343"
         };

         var results = extractor.Extract(codeLines);

         Assert.Empty(results);
      }

      [Fact(DisplayName = "Comment after an empty single line string literal")]
      public void Comment_AfterEmptyStringLiteral_SingleLine()
      {
         var codeLines = new[]
         {
            "s = \"string\"",
            // k = "" 'todo This is a comment
            "k = \"\" " + fullComment,
            "i = 23",
         };

         var results = extractor.Extract(codeLines);

         Assert.Single(results);
         Assert.Equal(TODO_TOKEN, results[0].Classification.Token);
         Assert.Equal(2, results[0].Line);
         Assert.Equal(COMMENT_CONTENT, results[0].Content);
      }

      [Fact(DisplayName = "Comment after string concatenation")]
      public void Comment_AfterVerbatimStandardConcat_SingleLine()
      {
         var codeLines = new[]
         {
            "s = \"string\"",
            // k = "' not a comment" + "'Not a comment"; 'todo This is a comment
            "k = \"' Not a comment\" + \" 'Not a comment \" " + fullComment,
            "i = 23 ",
         };

         var results = extractor.Extract(codeLines);

         Assert.Single(results);
         Assert.Equal(TODO_TOKEN, results[0].Classification.Token);
         Assert.Equal(2, results[0].Line);
         Assert.Equal(COMMENT_CONTENT, results[0].Content);
      }

      [Fact(DisplayName = "Comment after a multiline string - long last line")]
      public void Comment_AfterVerbatimStringLiteral_Multiline()
      {
         var codeLines = new[]
         {
            "q = \"starting line",
            "middle line",
            "end line - ' Not a comment \" " + fullComment,
            "s = \"hello\""

         };

         var results = extractor.Extract(codeLines);

         Assert.Single(results);
         Assert.Equal(TODO_TOKEN, results[0].Classification.Token);
         Assert.Equal(3, results[0].Line);
         Assert.Equal(COMMENT_CONTENT, results[0].Content);
      }

      [Fact(DisplayName = "Comment after a multiline string - short last line")]
      public void Comment_AfterVerbatimStringLiteral_Multiline2()
      {
         var codeLines = new[]
         {
            "q = \"starting line",
            "middle line",
            "end\" " + fullComment,

         };

         var results = extractor.Extract(codeLines);

         Assert.Single(results);
         Assert.Equal(TODO_TOKEN, results[0].Classification.Token);
         Assert.Equal(3, results[0].Line);
         Assert.Equal(COMMENT_CONTENT, results[0].Content);
      }

      [Fact(DisplayName = "Ignore comment-like text in a middle of multiline string")]
      public void NoComment_InsideVerbatimStringLiteral_Multiline()
      {
         var codeLines = new[]
         {
            "\"starting line",    // "starting line
            "' Not a comment\"",  // ' todo "Not a comment"
            "end line\""          // end line"
         };

         var results = extractor.Extract(codeLines);

         Assert.Empty(results);
      }

      [Fact(DisplayName = "Handle unclosed multiline string with comment like text")]
      public void HandleUnclosedVerbatim()
      {
         var codeLines = new[]
         {
            "\"starting line",   // "starting line
            "' Not a comment ",  // // Not a comment
            "end line"           // end line
         };

         var results = extractor.Extract(codeLines);

         Assert.Empty(results);
      }

      [Fact(DisplayName = "Handle escaped (\") before a (\")")]
      public void HandleEscapedSlashBeforeQuote()
      {
         var codeLines = new[]
          {
            // var k = " "" "; //todo This is a comment
            "k = \" \"\"\" " + fullComment
         };

         var results = extractor.Extract(codeLines);

         Assert.Single(results);
         Assert.Equal(TODO_TOKEN, results[0].Classification.Token);
         Assert.Equal(1, results[0].Line);
         Assert.Equal(COMMENT_CONTENT, results[0].Content);
      }

      [Fact(DisplayName = "Comment after multiline - empty first line")]
      public void CommentAfterVerbatim_FirstLineTickOnly()
      {
         var codeLines = new[]
         {
            "s = \"",
            "' Not a comment\"" + fullComment,
         };

         var results = extractor.Extract(codeLines);

         Assert.Single(results);
         Assert.Equal(TODO_TOKEN, results[0].Classification.Token);
         Assert.Equal(2, results[0].Line);
         Assert.Equal(COMMENT_CONTENT, results[0].Content);
      }

      [Fact(DisplayName = "Handle unclosed string with comment like text")]
      public void HandleUnclosedLiteralString1()
      {
         var codeLines = new[]
         {
            "\" ' Not a comment",  // " ' Not a comment
         };

         var results = extractor.Extract(codeLines);

         Assert.Empty(results);
      }

      [Fact(DisplayName = "Handle unclosed string concatenation")]
      public void HandleUnclosedLiteralString2()
      {
         var codeLines = new[]
         {
            "s = \" ' Not a comment\" + \" ' Not a comment",  // " ' Not a comment" + " ' Not a comment
         };

         var results = extractor.Extract(codeLines);

         Assert.Empty(results);
      }

      [Fact(DisplayName = "Handle line ends with unclosed string at \"")]
      public void HandleLineeEndsWithQuote()
      {
         var codeLines = new[]
         {
            "i = \"",
         };

         var results = extractor.Extract(codeLines);

         Assert.Empty(results);
      }
   }
}
