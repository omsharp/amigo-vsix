using System.Collections.ObjectModel;
using Xunit;
using Moq;
using Configurations.Core.Comments;
using VSIX.Package.Comments.Parsers;

namespace VSIX.Tests.CommentExtraction.CSharp
{
   public class CSharpCommentExtractor_Tests
   {
      const string TODO_TOKEN = "todo";
      const string COMMENT_CONTENT = "This is a comment";

      readonly string fullComment;
      readonly CSharpCommentExtractor extractor;

      public CSharpCommentExtractor_Tests()
      {
         var configMock = new Mock<ICommentConfiguration>();

         configMock.Setup(c => c.Classifications).Returns(
            new ObservableCollection<Classification>
            {
               new Classification { Name = "Todo", Token = TODO_TOKEN},
               new Classification { Name = "Hack", Token = "hack"},
               new Classification { Name = "Bug", Token = "bug"},

            });

         extractor = new CSharpCommentExtractor(configMock.Object);

         fullComment = $"//{TODO_TOKEN} {COMMENT_CONTENT}";
      }



      [Fact(DisplayName = "Classified comments")]
      public void Comment_LineBeginning_Classified()
      {
         var codeLines = new[]
         {
            "var s = \"string\"",
            fullComment,
            "var i = 23;",
            "var c = 234;" + fullComment
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
            "var s = \"string\"",
            // var k = "// todo Not a comment"; //todo This is a comment
            "var k = \"// {TODO_TOKEN} Not a comment\";" + fullComment,
            "var i = 23;",
         };

         var results = extractor.Extract(codeLines);

         Assert.Single(results);
         Assert.Equal(TODO_TOKEN, results[0].Classification.Token);
         Assert.Equal(2, results[0].Line);
         Assert.Equal(COMMENT_CONTENT, results[0].Content);
      }

      [Fact(DisplayName = "Handle escaped (\")")]
      public void HandleEscapedDoubleQuotes()
      {
         var codeLines = new[]
         {
            // var k = " \" "; //todo This is a comment
            "var k = \" \\\" \";" + fullComment,
         };

         var results = extractor.Extract(codeLines);

         Assert.Single(results);
         Assert.Equal(TODO_TOKEN, results[0].Classification.Token);
         Assert.Equal(1, results[0].Line);
         Assert.Equal(COMMENT_CONTENT, results[0].Content);
      }

      [Fact(DisplayName = "Handle escaped (\") - Verbatim")]
      public void HandleEscapedDoubleQuotes_Verbatim()
      {
         var codeLines = new[]
         {
            // var k = @" "" "; //todo This is a comment
            "var k = @\" \"\" \";" + fullComment
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
            "double d = 234.3",
            "var s = \"// not a comment\";", // var s = "// not a comment"
            "var i = 2343;"
         };

         var results = extractor.Extract(codeLines);

         Assert.Empty(results);
      }

      [Fact(DisplayName = "Comment after an empty single line string literal")]
      public void Comment_AfterEmptyStringLiteral_SingleLine()
      {
         var codeLines = new[]
         {
            "var s = \"string\"",
            // var k = "" //todo This is a comment
            "var k = \"\";" + fullComment,
            "var i = 23;",
         };

         var results = extractor.Extract(codeLines);

         Assert.Single(results);
         Assert.Equal(TODO_TOKEN, results[0].Classification.Token);
         Assert.Equal(2, results[0].Line);
         Assert.Equal(COMMENT_CONTENT, results[0].Content);
      }

      [Fact(DisplayName = "Comment after an empty single line verbatim string literal")]
      public void Comment_AfterEmptyVirbatimStringLiteral_SingleLine()
      {
         var codeLines = new[]
         {
            "var s = \"string\"",
            // var k = @"" //todo This is a comment
            "var k = @\"\";" + fullComment,
            "var i = 23;",
         };

         var results = extractor.Extract(codeLines);

         Assert.Single(results);
         Assert.Equal(TODO_TOKEN, results[0].Classification.Token);
         Assert.Equal(2, results[0].Line);
         Assert.Equal(COMMENT_CONTENT, results[0].Content);
      }

      [Fact(DisplayName = "Comment after concatenation of verbatim and standard string literal")]
      public void Comment_AfterVerbatimStandardConcat_SingleLine()
      {
         var codeLines = new[]
         {
            "var s = \"string\"",
            // var k = @"// not a comment" + "//Not a comment"; //todo This is a comment
            "var k = @\"// Not a comment\" + \" //Not a comment \";" + fullComment,
            "var i = 23;",
         };

         var results = extractor.Extract(codeLines);

         Assert.Single(results);
         Assert.Equal(TODO_TOKEN, results[0].Classification.Token);
         Assert.Equal(2, results[0].Line);
         Assert.Equal(COMMENT_CONTENT, results[0].Content);
      }

      [Fact(DisplayName = "Comment after concatenation of verbatim to another verbatim string literal")]
      public void Comment_AfterVerbatimToVerbatimConcat_SingleLine()
      {
         var codeLines = new[]
         {
            "var s = \"string\"",
            // var k = @"// not a comment" + "//Not a comment\"; //todo This is a comment
            "var k = @\"// Not a comment\" + @\" //Not a comment \\\";" + fullComment,
            "var i = 23;",
         };

         var results = extractor.Extract(codeLines);

         Assert.Single(results);
         Assert.Equal(TODO_TOKEN, results[0].Classification.Token);
         Assert.Equal(2, results[0].Line);
         Assert.Equal(COMMENT_CONTENT, results[0].Content);
      }

      [Fact(DisplayName = "Comment after a single line verbatim string literal")]
      public void Comment_AfterVirbatimStringLiteral_SingleLine()
      {
         var codeLines = new[]
         {
            "var s = \"string\"",
            // var k = @"// not a comment" //todo This is a comment
            "var k = @\"// Not \"\" a comment\\\";" + fullComment,
            "var i = 23;",
         };

         var results = extractor.Extract(codeLines);

         Assert.Single(results);
         Assert.Equal(TODO_TOKEN, results[0].Classification.Token);
         Assert.Equal(2, results[0].Line);
         Assert.Equal(COMMENT_CONTENT, results[0].Content);
      }

      [Fact(DisplayName = "Comment after a verbatim last line - last line is longer than one before")]
      public void Comment_AfterVerbatimStringLiteral_Multiline()
      {
         var codeLines = new[]
         {
            "var q = @\"starting line",
            "middle line",
            "end line - // Not a comment \";" + fullComment,
            "var s = \"hello\""

         };

         var results = extractor.Extract(codeLines);

         Assert.Single(results);
         Assert.Equal(TODO_TOKEN, results[0].Classification.Token);
         Assert.Equal(3, results[0].Line);
         Assert.Equal(COMMENT_CONTENT, results[0].Content);
      }

      [Fact(DisplayName = "Comment after a verbatim last line - last line is shorter than one before")]
      public void Comment_AfterVerbatimStringLiteral_Multiline2()
      {
         var codeLines = new[]
         {
            "var q = @\"starting line",
            "middle line",
            "end\";" + fullComment,
            "var s = \"hello\""

         };

         var results = extractor.Extract(codeLines);

         Assert.Single(results);
         Assert.Equal(TODO_TOKEN, results[0].Classification.Token);
         Assert.Equal(3, results[0].Line);
         Assert.Equal(COMMENT_CONTENT, results[0].Content);
      }

      [Fact(DisplayName = "Ignore comment-like text in a single line verbatim string")]
      public void NoComment_InsideVerbatimStringLiteral_SingleLine()
      {
         var codeLines = new[]
         {
            "double d = 234.3",
            "var k = @\" // Not // a comment\\\"", // @" // todo Not // a comment";
            "var i = 2343;"
         };

         var results = extractor.Extract(codeLines);

         Assert.Empty(results);
      }

      [Fact(DisplayName = "Ignore comment-like text in a middle of multiline verbatim string")]
      public void NoComment_InsideVerbatimStringLiteral_Multiline()
      {
         var codeLines = new[]
         {
            "@\"starting line",     // @"starting line
            "// Not a comment\"",   // // todo "Not a comment"
            "end line\""            // end line"
         };

         var results = extractor.Extract(codeLines);

         Assert.Empty(results);
      }

      [Fact(DisplayName = "Handle unclosed verbatim string with comment like text")]
      public void HandleUnclosedVerbatim()
      {
         var codeLines = new[]
         {
            "@\"starting line",          // @"starting line
            "\"\" // Not a comment\"\" ", // "" // Not a comment""
            "end line"                   // end line
         };

         var results = extractor.Extract(codeLines);

         Assert.Empty(results);
      }

      [Fact(DisplayName = "Handle unclosed string literal after verbatim")]
      public void HandleUnclosedStringAfterVebratim()
      {
         var codeLines = new[]
         {
            "var s = \"string\"",
            // var k = @"// not a comment" + "//Not a comment //todo Not a comment
            "var k = @\"// Not a comment\" + \" //Not a comment // Not a comment",
            "var i = 23;",
         };

         var results = extractor.Extract(codeLines);

         Assert.Empty(results);
      }

      [Fact(DisplayName = "Handle escaped (\\) before a (\")")]
      public void HandleEscapedSlashBeforeQuote()
      {  
         var codeLines = new[]
         {
            "var k = \" \\\\\" \"; // Not a comment ",
         };
         
         var results = extractor.Extract(codeLines);

         Assert.Empty(results);
      }

      [Fact(DisplayName = "Handle escaped (\") before a (\") - Verbatim")]
      public void HandleEscapedSlashBeforeQuote_Verbatim()
      {
         var codeLines = new[]
         {
            "var k = @\" \"\"\" \"; // Not a comment ",
         };
         
         var results = extractor.Extract(codeLines);

         Assert.Empty(results);
      }

      [Fact(DisplayName = "Comment after verbatim - empty first line")]
      public void CommentAfterVerbatim_FirstLineTickOnly()
      {
         var codeLines = new[]
         {
            "var s = @\"",
            "// Not a comment\"" + fullComment,
         };

         var results = extractor.Extract(codeLines);

         Assert.Single(results);
         Assert.Equal(TODO_TOKEN, results[0].Classification.Token);
         Assert.Equal(2, results[0].Line);
         Assert.Equal(COMMENT_CONTENT, results[0].Content);
      }

      [Fact(DisplayName = "Handle line ends unclosed verbatim at @\"")]
      public void HandleUnclosedVebratimAfterQuote()
      {
         var codeLines = new[]
         {
            "var i = @\"",
         };

         var results = extractor.Extract(codeLines);

         Assert.Empty(results);
      }

      [Fact(DisplayName = "Handle unclosed string with comment like text")]
      public void HandleUnclosedLiteralString1()
      {
         var codeLines = new[]
         {
            "\" // Not a comment",  // " // Not a comment
         };

         var results = extractor.Extract(codeLines);

         Assert.Empty(results);
      }

      [Fact(DisplayName = "Handle unclosed normal strings (Double)")]
      public void HandleUnclosedLiteralString2()
      {
         var codeLines = new[]
         {
            "\" // Not a comment\" + \" // Not a comment",  // " // Not a comment" + " // Not a comment
         };

         var results = extractor.Extract(codeLines);

         Assert.Empty(results);
      }

      [Fact(DisplayName = "Handle line ends with @")]
      public void HandleUnclosedVebratimAfterAt()
      {
         var codeLines = new[]
         {
            "var i = @",
         };

         var results = extractor.Extract(codeLines);

         Assert.Empty(results);
      }

      [Fact(DisplayName = "Handle line ends at with single slash")]
      public void HandleLineEndsWithSingleSlash()
      {
         var codeLines = new[]
         {
            "var i = 2; /",
         };

         var results = extractor.Extract(codeLines);

         Assert.Empty(results);
      }

      [Fact(DisplayName = "Handle line ends with unclosed string at \"")]
      public void HandleLineeEndsWithQuote()
      {
         var codeLines = new[]
         {
            "var i = \"",
         };

         var results = extractor.Extract(codeLines);

         Assert.Empty(results);
      }
   }
}
