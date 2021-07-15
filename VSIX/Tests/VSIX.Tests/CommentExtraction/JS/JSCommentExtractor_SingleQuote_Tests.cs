using Xunit;

namespace VSIX.Tests.CommentExtraction.JS
{
   public class JSCommentExtractor_SingleQuote_Tests : JSCommentExtractor_Tests_Base
   {
      [Fact(DisplayName = "Comment after a single line string")]
      public void Comment_AfterStandardStringLiteral_SingleQuote()
      {
         var codeLines = new[]
         {
            "var s = \"string\"",
            // var k = '// todo Not a comment'; //todo This is a comment
            "var k = '// {TODO_TOKEN} Not a comment';" + FullComment,
            "var i = 23;",
         };

         var results = JSExtractor.Extract(codeLines);

         Assert.Single(results);
         Assert.Equal(TODO_TOKEN, results[0].Classification.Token);
         Assert.Equal(2, results[0].Line);
         Assert.Equal(COMMENT_CONTENT, results[0].Content);
      }

      [Fact(DisplayName = "Ignore comment-like text in a single line string")]
      public void NoComment_InsideStandardStringLiteral_SingleQuote()
      {
         var codeLines = new[]
         {
            "double d = 234.3",
            "var s = '// not a comment';", // var s = '// not a comment'
            "var i = 2343;"
         };

         var results = JSExtractor.Extract(codeLines);

         Assert.Empty(results);
      }

      [Fact(DisplayName = "Comment after an empty string")]
      public void Comment_AfterEmptyStringLiteral_SingleQuote()
      {
         var codeLines = new[]
         {
            "var s = \"string\"",
            // var k = ''; //todo This is a comment
            "var k = '';" + FullComment,
            "var i = 23;",
         };

         var results = JSExtractor.Extract(codeLines);

         Assert.Single(results);
         Assert.Equal(TODO_TOKEN, results[0].Classification.Token);
         Assert.Equal(2, results[0].Line);
         Assert.Equal(COMMENT_CONTENT, results[0].Content);
      }

      [Fact(DisplayName = "Handle escaped (')")]
      public void HandleEscapedQuote_SingleQuote()
      {
         var codeLines = new[]
         {
            "var s = \"string\"",
            // var k = ' \' '; //todo This is a comment
            "var k = ' \\' ';" + FullComment,
            "var i = 23;",
         };

         var results = JSExtractor.Extract(codeLines);

         Assert.Single(results);
         Assert.Equal(TODO_TOKEN, results[0].Classification.Token);
         Assert.Equal(2, results[0].Line);
         Assert.Equal(COMMENT_CONTENT, results[0].Content);
      }

      [Fact(DisplayName = "Comment after concatenation")]
      public void Comment_AfterConcat_SingleQuote()
      {
         var comment = $"This \"is\" comment";
         var fullComment = $"// {TODO_TOKEN} {comment}";
         var codeLines = new[]
         {
            "var s = \"string\"",
            // var k = '// not a \' comment' + '//Not a comment'; //todo This is a comment
            "var k = '// Not a \\' comment' + ' //Not a comment ';" + fullComment,
            "var i = 23;",
         };

         var results = JSExtractor.Extract(codeLines);

         Assert.Single(results);
         Assert.Equal(TODO_TOKEN, results[0].Classification.Token);
         Assert.Equal(2, results[0].Line);
         Assert.Equal(comment, results[0].Content);
      }

      [Fact(DisplayName = "Comment after a multiline string - last line is longer than one before")]
      public void Comment_AfterStringLiteral_Multiline()
      {
         var codeLines = new[]
         {
            "var s = 'starting line\\",
            "middle line\\",
            "end line - // Not a comment ';" + FullComment,
            "var s = \"hello\""
         };

         var results = JSExtractor.Extract(codeLines);

         Assert.Single(results);
         Assert.Equal(TODO_TOKEN, results[0].Classification.Token);
         Assert.Equal(3, results[0].Line);
         Assert.Equal(COMMENT_CONTENT, results[0].Content);
      }

      [Fact(DisplayName = "Comment after a multiline string - last line is shorter than one before")]
      public void Comment_AfterStringLiteral_Multiline2()
      {
         var codeLines = new[]
         {
            "var s = 'starting line\\",
            "middle line\\",
            "end';" + FullComment,
            "var s = \"hello\""
         };

         var results = JSExtractor.Extract(codeLines);

         Assert.Single(results);
         Assert.Equal(TODO_TOKEN, results[0].Classification.Token);
         Assert.Equal(3, results[0].Line);
         Assert.Equal(COMMENT_CONTENT, results[0].Content);
      }

      [Fact(DisplayName = "Ignore comment-like text in a middle of multiline string")]
      public void NoComment_InsideStringLiteral_Multiline()
      {
         var codeLines = new[]
         {
            "var s = 'starting line\\",
            "// Not a comment \\",
            $"end line';",
         };

         var results = JSExtractor.Extract(codeLines);

         Assert.Empty(results);
      }

      [Fact(DisplayName = "Handle escaped (\\) before a (\")")]
      public void HandleEscapedSlashBeforeQuote()
      {
         var codeLines = new[]
         {
            "var k = ' \\\\' '; // Not a comment ",
         };
         var results = JSExtractor.Extract(codeLines);

         Assert.Empty(results);
      }

      [Fact(DisplayName = "Handle multiline string - line doesn't end with slash")]
      public void HandleBrokenMultilineString()
      {
         var comment = $"This is a comment";
         var fullComment = $"// {TODO_TOKEN} {comment}";

         var codeLines = new[]
         {
            "var s = 'starting line",
            fullComment,
            "end line' // Some comment",
         };

         var results = JSExtractor.Extract(codeLines);

         Assert.Single(results);
         Assert.Equal(2, results[0].Line);
         Assert.Equal(TODO_TOKEN, results[0].Classification.Token);
      }

      [Fact(DisplayName = "Handle multiline string line ends with double slash")]
      public void HandleMultilineStringEndsWithDoubleSlash()
      {
         var comment = $"This is a comment";
         var fullComment = $"// {TODO_TOKEN} {comment}";

         var codeLines = new[]
         {
            "var s = 'starting line\\\\",
            fullComment,
            $"end line' // Some comment",
         };

         var results = JSExtractor.Extract(codeLines);

         Assert.Single(results);
         Assert.Equal(2, results[0].Line);
         Assert.Equal(TODO_TOKEN, results[0].Classification.Token);
      }

      [Fact(DisplayName = "Handle unclosed string literal after multiline string")]
      public void HandleUnclosedStringAfterMultilineString()
      {
         var codeLines = new[]
         {
            "var s = 'starting line\\",
            "// middle line \\",
            "end line' + ' // Not a comment",
         };

         var results = JSExtractor.Extract(codeLines);

         Assert.Empty(results);
      }

      [Fact(DisplayName = "Handle unclosed string with comment like text")]
      public void HandleUnclosedLiteralString1()
      {
         var codeLines = new[]
         {
            "var s = ' // Not a comment",  // " // Not a comment
         };

         var results = JSExtractor.Extract(codeLines);

         Assert.Empty(results);
      }

      [Fact(DisplayName = "Handle unclosed normal strings (Double)")]
      public void HandleUnclosedLiteralString2()
      {
         var codeLines = new[]
         {
            "var s = ' // Not a comment' + ' // Not a comment",  // " // Not a comment" + " // Not a comment
         };

         var results = JSExtractor.Extract(codeLines);

         Assert.Empty(results);
      }

      [Fact(DisplayName = "Handle line ends with unclosed string at \"")]
      public void HandleLineeEndsWithQuote()
      {
         var codeLines = new[]
         {
            $"var i = '",
         };

         var results = JSExtractor.Extract(codeLines);

         Assert.Empty(results);
      }
   }
}
