using Xunit;

namespace VSIX.Tests.CommentExtraction.JS
{
   public class JSCommentExtractor_Misc_Tests : JSCommentExtractor_Tests_Base
   {
      [Fact(DisplayName = "Classified comments")]
      public void Comment_LineBeginning_Classified()
      {
         var codeLines = new[]
         {
            "var s = \"string\"",
            FullComment,
            "var i = 23;",
            "var c = 234;" + FullComment
         };

         var results = JSExtractor.Extract(codeLines);

         Assert.Equal(2, results.Count);

         Assert.Equal(TODO_TOKEN, results[0].Classification.Token);
         Assert.Equal(TODO_TOKEN, results[1].Classification.Token);

         Assert.Equal(2, results[0].Line);
         Assert.Equal(4, results[1].Line);

         Assert.Equal(COMMENT_CONTENT, results[0].Content);
         Assert.Equal(COMMENT_CONTENT, results[1].Content);
      }

      [Fact(DisplayName = "Comment after concatenation of (') string with a (\") one")]
      public void Comment_AfterConcat_Mix()
      {
         var codeLines = new[]
         {
            "var s = \"string\"",
            // var k = '// not a comment' + "//Not a comment"; //todo This is a comment
            "var k = '// Not a \"\" comment' + \" //Not a comment \";" + FullComment,
            "var i = 23;",
         };

         var results = JSExtractor.Extract(codeLines);

         Assert.Single(results);
         Assert.Equal(TODO_TOKEN, results[0].Classification.Token);
         Assert.Equal(2, results[0].Line);
         Assert.Equal(COMMENT_CONTENT, results[0].Content);
      }

      [Fact(DisplayName = "Handle unclosed normal strings after another one")]
      public void HandleUnclosedLiteralStringAfterAnother()
      {
         var codeLines = new[]
         {
            " var s = \" // Not a comment\" + ' // Not a comment",
         };

         var results = JSExtractor.Extract(codeLines);

         Assert.Empty(results);
      }

      [Fact(DisplayName = "Handle unclosed normal strings after another one (Reverse)")]
      public void HandleUnclosedLiteralStringAfterAnother2()
      {
         var codeLines = new[]
         {
            " var s = ' // Not a comment' + \" // Not a comment",
         };

         var results = JSExtractor.Extract(codeLines);

         Assert.Empty(results);
      }

      [Fact(DisplayName = "Handle line ends with single slash")]
      public void HandleLineEndsWithSingleSlash()
      {
         var codeLines = new[]
         {
            $"var i = 2; /",
         };

         var results = JSExtractor.Extract(codeLines);

         Assert.Empty(results);
      }

      //! Handling ( ` ) strings
      [Fact(DisplayName = "Comment after tick string - single line")]
      public void Comment_AfterTickString()
      {
         var codeLines = new[]
         {
            "var s = \"string\"",
            "var k = `// Not a comment`" + FullComment,
            "var i = 23;",
         };

         var results = JSExtractor.Extract(codeLines);

         Assert.Single(results);
         Assert.Equal(TODO_TOKEN, results[0].Classification.Token);
         Assert.Equal(2, results[0].Line);
         Assert.Equal(COMMENT_CONTENT, results[0].Content);
      }

      [Fact(DisplayName = "Handle escaped tick")]
      public void HandleEscapedTick()
      {
         var codeLines = new[]
         {
            "var s = `First line \\` ",
            "// Not a comment`" + FullComment,
         };

         var results = JSExtractor.Extract(codeLines);

         Assert.Single(results);
         Assert.Equal(TODO_TOKEN, results[0].Classification.Token);
         Assert.Equal(2, results[0].Line);
         Assert.Equal(COMMENT_CONTENT, results[0].Content);
      }

      [Fact(DisplayName = "Handle escaped (\\) in template literal")]
      public void HandleEscapedSlashInTickString()
      {
         var codeLines = new[]
         {
            "var s = `First line",
            "// Not a comment \\\\`" + FullComment,
         };

         var results = JSExtractor.Extract(codeLines);

         Assert.Single(results);
         Assert.Equal(TODO_TOKEN, results[0].Classification.Token);
         Assert.Equal(2, results[0].Line);
         Assert.Equal(COMMENT_CONTENT, results[0].Content);
      }

      [Fact(DisplayName = "Ignore comment-like text in middle of template literal")]
      public void IgnoreCommentLikeInTemplateLiterals()
      {
         var codeLines = new[]
         {
            "var s = `First line",
            "// Not a comment ",
            "end line`",
         };

         var results = JSExtractor.Extract(codeLines);

         Assert.Empty(results);
      }


      [Fact(DisplayName = "Comment after template literal - short last line")]
      public void CommentAfterMultiline_ShortLine()
      {
         var codeLines = new[]
         {
            "var s = `First line",
            "end`" + FullComment,
         };

         var results = JSExtractor.Extract(codeLines);

         Assert.Single(results);
         Assert.Equal(TODO_TOKEN, results[0].Classification.Token);
         Assert.Equal(2, results[0].Line);
         Assert.Equal(COMMENT_CONTENT, results[0].Content);
      }

      [Fact(DisplayName = "Comment after template literal - long last line")]
      public void CommentAfterMultiline_LongLine()
      {
         var codeLines = new[]
         {
            "var s = `First line",
            "long last line for testing`" + FullComment,
         };

         var results = JSExtractor.Extract(codeLines);

         Assert.Single(results);
         Assert.Equal(TODO_TOKEN, results[0].Classification.Token);
         Assert.Equal(2, results[0].Line);
         Assert.Equal(COMMENT_CONTENT, results[0].Content);
      }

      [Fact(DisplayName = "Handle unclosed template literals")]
      public void UnclosedTemplateLiteral()
      {
         var codeLines = new[]
         {
            "var s = `First line",
            "// Not a comment",
         };

         var results = JSExtractor.Extract(codeLines);

         Assert.Empty(results);
      }

      [Fact(DisplayName = "Comment after template literals - empty first line")]
      public void CommentAfterTemplateLiteral_FirstLineTickOnly()
      {
         var codeLines = new[]
         {
            "var s = `",
            "// Not a comment`" + FullComment,
         };

         var results = JSExtractor.Extract(codeLines);

         Assert.Single(results);
         Assert.Equal(TODO_TOKEN, results[0].Classification.Token);
         Assert.Equal(2, results[0].Line);
         Assert.Equal(COMMENT_CONTENT, results[0].Content);
      }

      [Fact(DisplayName = "Handle unclosed template literals")]
      public void UnclosedTemplateLiteral_FirstLineTickOnly()
      {
         var codeLines = new[]
         {
            "var s = `",
            "// Not a comment",
         };

         var results = JSExtractor.Extract(codeLines);

         Assert.Empty(results);
      }

      [Fact(DisplayName = "Handle unclosed template literals - single tick on line")]
      public void UnclosedTemplateLiteral_TickOnly()
      {
         var codeLines = new[]
         {
            "var s = `",
         };

         var results = JSExtractor.Extract(codeLines);

         Assert.Empty(results);
      }

      [Fact(DisplayName = "Concat Template Literal with single quote string")]
      public void ConcatToSingleQuote()
      {
         var codeLines = new[]
         {
            "var s = `First line ",
            "end line`" + "'// Not a comment'" + FullComment,
         };

         var results = JSExtractor.Extract(codeLines);

         Assert.Single(results);
         Assert.Equal(TODO_TOKEN, results[0].Classification.Token);
         Assert.Equal(2, results[0].Line);
         Assert.Equal(COMMENT_CONTENT, results[0].Content);
      }

      [Fact(DisplayName = "Concat Template Literal with double quote string")]
      public void ConcatToDoubleQuote()
      {
         var codeLines = new[]
         {
            "var s = `First line",
            "end line`" + "\"// Not a comment\"" + FullComment,
         };

         var results = JSExtractor.Extract(codeLines);

         Assert.Single(results);
         Assert.Equal(TODO_TOKEN, results[0].Classification.Token);
         Assert.Equal(2, results[0].Line);
         Assert.Equal(COMMENT_CONTENT, results[0].Content);
      }
   }
}
