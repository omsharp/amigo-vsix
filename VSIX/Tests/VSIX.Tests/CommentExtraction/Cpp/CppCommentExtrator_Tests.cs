using Configurations.Core.Comments;
using Moq;
using System.Collections.ObjectModel;
using VSIX.Package.Comments.Parsers;
using Xunit;

namespace VSIX.Tests.CommentExtraction.Cpp
{
   public class CppCommentExtrator_Tests
   {
      const string TODO_TOKEN = "todo";
      const string COMMENT_CONTENT = "This is a comment";
      readonly string fullComment;

      readonly CppCommentExtractor extractor;

      public CppCommentExtrator_Tests()
      {
         var configMock = new Mock<ICommentConfiguration>();

         configMock.Setup(c => c.Classifications).Returns(
            new ObservableCollection<Classification>
            {
               new Classification { Name = "Todo", Token = TODO_TOKEN},
               new Classification { Name = "Hack", Token = "hack"},
               new Classification { Name = "Bug", Token = "bug"},

            });

         extractor = new CppCommentExtractor(configMock.Object);

         fullComment = $"// {TODO_TOKEN} {COMMENT_CONTENT}";
      }

      [Fact(DisplayName = "Classified comment")]
      public void Comment_LineBeginning_Classified()
      {
         var codeLines = new[]
         {
            "auto s = \"string\"",
            fullComment,
            "auto i = 23;",
            "auto c = 234;" + fullComment  // auto c = 234;// Token Comment
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
            "auto k = \"string\"",
            // var k = "// todo Not a comment"; // Token Comment
            "auto s = \"// {TODO_TOKEN} Not \\\" a comment\";" + fullComment,
            "auto i = 23;",
         };

         var results = extractor.Extract(codeLines);

         Assert.Single(results);
         Assert.Equal(TODO_TOKEN, results[0].Classification.Token);
         Assert.Equal(2, results[0].Line);
         Assert.Equal(COMMENT_CONTENT, results[0].Content);
      }

      [Fact(DisplayName = "Ignore comment-like text in a string")]
      public void NoComment_InsideStandardStringLiteral()
      {
         var codeLines = new[]
         {
            "double d = 234.3",
            "auto s = \"// not a comment\";", // auto s = "// not a comment";
            "auto i = 2343;"
         };

         var results = extractor.Extract(codeLines);

         Assert.Empty(results);
      }

      [Fact(DisplayName = "Comment after an empty single line raw string")]
      public void Comment_AfterEmptyRawStringLiteral_SingleLine()
      {
         var codeLines = new[]
         {
            "auto s = \"string\"",
            "auto s = R\"~()~\";" + fullComment,  // auto s = R"~()~";// Token Comment
            "auto i = 23;",
         };

         var results = extractor.Extract(codeLines);

         Assert.Single(results);
         Assert.Equal(TODO_TOKEN, results[0].Classification.Token);
         Assert.Equal(2, results[0].Line);
         Assert.Equal(COMMENT_CONTENT, results[0].Content);
      }

      [Fact(DisplayName = "Comment after an empty single")]
      public void Comment_AfterEmptyStringLiteral_SingleLine()
      {
         var codeLines = new[]
         {
            "auto s = \"string\"",
            "auto s = \"\";" + fullComment,  // auto s = R"~()~";// Token Comment
            "auto i = 23;",
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
            "auto k = \" \\\" \";" + fullComment,
         };
         
         var results = extractor.Extract(codeLines);

         Assert.Single(results);
         Assert.Equal(TODO_TOKEN, results[0].Classification.Token);
         Assert.Equal(1, results[0].Line);
         Assert.Equal(COMMENT_CONTENT, results[0].Content);
      }

      [Fact(DisplayName = "Handle escaped (\\) before a (\")")]
      public void HandleEscapedSlashBeforeQuote()
      {
         var codeLines = new[]
         {
            //! bookmark
            //var k = " \ \" "; // comment
            //var k = " \\" "; // comment
            "auto k = \" \\\\\" \"; // Not a comment",
         };
         //var k = " \\" "; // comment
         var results = extractor.Extract(codeLines);

         Assert.Empty(results);
      }


      [Fact(DisplayName = "Comment after concatenation of raw and standard strings")]
      public void Comment_AfterRawStandardConcat_SingleLine()
      {
         var codeLines = new[]
         {
            "auto s = \"string\"",
            // auto r = R"~(// Not a \"\" comment\")~" " //Not a comment "; // Token Comment
            "auto r = R\"~(// Not a \" comment\")~\" \" //Not a comment \";" + fullComment,
            "auto i = 23;",
         };

         var results = extractor.Extract(codeLines);

         Assert.Single(results);
         Assert.Equal(TODO_TOKEN, results[0].Classification.Token);
         Assert.Equal(2, results[0].Line);
         Assert.Equal(COMMENT_CONTENT, results[0].Content);
      }

      [Fact(DisplayName = "Comment after concatenation of raw string to another raw")]
      public void Comment_AfterRawToVerbatimConcat_SingleLine()
      {
         var codeLines = new[]
         {
            "auto s = \"string\"",
            // auto r = R"~(// Not a \"\" comment\")~" R"(//Not a comment)"; // Token Comment
            "auto r = R\"~(// Not a \\\" comment\")~\" R\"(//Not a comment)\";" + fullComment,
            "auto i = 23;",
         };

         var results = extractor.Extract(codeLines);

         Assert.Single(results);
         Assert.Equal(TODO_TOKEN, results[0].Classification.Token);
         Assert.Equal(2, results[0].Line);
         Assert.Equal(COMMENT_CONTENT, results[0].Content);
      }

      [Fact(DisplayName = "Comment after a single line raw string literal")]
      public void Comment_AfterRawStringLiteral_SingleLine()
      {
         var codeLines = new[]
         {
            "var s = \"string\"",
            // auto r = R"DEL({TODO_TOKEN} Not \"\" a comment)DEL"; // Token Comment
            "auto r = R\"DEL({TODO_TOKEN} Not \"\" a comment)DEL\";" + fullComment,
            "auto i = 23;",
         };

         var results = extractor.Extract(codeLines);

         Assert.Single(results);
         Assert.Equal(TODO_TOKEN, results[0].Classification.Token);
         Assert.Equal(2, results[0].Line);
         Assert.Equal(COMMENT_CONTENT, results[0].Content);
      }

      [Fact(DisplayName = "Comment after a raw's last line - last line is longer than one before")]
      public void Comment_AfterRawStringLiteral_Multiline()
      {
         var codeLines = new[]
         {
            "auto r = R\"\"\"(starting line",                  // auto r = R"""(starting line
            "middle // line",                                  // middle // line
            "end line - // Not Comment)\"\"\";" + fullComment, // end line - // Not Comment)"""; // Token Comment
            "auto s = \"hello\""

         };

         var results = extractor.Extract(codeLines);

         Assert.Single(results);
         Assert.Equal(TODO_TOKEN, results[0].Classification.Token);
         Assert.Equal(3, results[0].Line);
         Assert.Equal(COMMENT_CONTENT, results[0].Content);
      }

      [Fact(DisplayName = "Comment after a raw's last line - last line is shorter than one before")]
      public void Comment_AfterRawStringLiteral_Multiline2()
      {
         var codeLines = new[]
         {
            "auto r = R\"\"\"(starting line",  // auto r = R"""(starting line
            "middle // line",                  // middle // line
            "end)\"\"\";" + fullComment,       // end)"""; // Token Comment
            "auto s = \"hello\""

         };

         var results = extractor.Extract(codeLines);

         Assert.Single(results);
         Assert.Equal(TODO_TOKEN, results[0].Classification.Token);
         Assert.Equal(3, results[0].Line);
         Assert.Equal(COMMENT_CONTENT, results[0].Content);
      }

      [Fact(DisplayName = "Comment after a raw's - empty first line")]
      public void Comment_AfterRawStringLiteral_EmptyFirstLine()
      {
         var codeLines = new[]
         {
            "auto r = R\"\"\"(",
            "// Not a comment)\"\"\";" + fullComment,     

         };

         var results = extractor.Extract(codeLines);

         Assert.Single(results);
         Assert.Equal(TODO_TOKEN, results[0].Classification.Token);
         Assert.Equal(2, results[0].Line);
         Assert.Equal(COMMENT_CONTENT, results[0].Content);
      }

      [Fact(DisplayName = "Ignore comment-like text in a single line raw string")]
      public void NoComment_InsideRawStringLiteral_SingleLine()
      {
         var codeLines = new[]
         {
            "double d = 234.3",
            "auto r = R\"\"\"( // Not a comment )\"\"\";",
            "int i = 2343;"
         };

         var results = extractor.Extract(codeLines);

         Assert.Empty(results);
      }

      [Fact(DisplayName = "Ignore comment-like text in a multiline raw string")]
      public void NoComment_InsideRawStringLiteral_Multiline()
      {
         var codeLines = new[]
         {
            "auto r = R\"\"\"(starting line",   
            "// Not a comment",
            "end line)\"\"\";"
         };

         var results = extractor.Extract(codeLines);

         Assert.Empty(results);
      }

      [Fact(DisplayName = "Handle unclosed raw strings")]
      public void HandleUnclosedVerbatim()
      {
         var codeLines = new[]
         {
            "auto r = R\"~(starting line",
            "middle line",
            "end line)\"" // no delimiter (~) = not closed
         };

         var results = extractor.Extract(codeLines);

         Assert.Empty(results);
      }

      [Fact(DisplayName = "Handle unclosed string literal after a closed raw")]
      public void HandleUnclosedStringAfterVebratim()
      {
         var codeLines = new[]
         {
            "auto s = \"string\"",
            "auto r = R\"~(raw string)~\" \"unclosed normal string // NOT a comment\"",
            "auto i = 23;",
         };

         var results = extractor.Extract(codeLines);

         Assert.Empty(results);
      }

      [Fact(DisplayName = "Handle uninitialized raw after R\"")]
      public void HandleUninitializedRawString()
      {
         var codeLines = new[]
         {
            "auto s = R\"",
         };

         var results = extractor.Extract(codeLines);

         Assert.Empty(results);

      }

      [Fact(DisplayName = "Handle line ends with unclosed raw at R\"~(")]
      public void HandleUnclosedRawAfterQuote()
      {
         var codeLines = new[]
         {
            "auto r = R\"~(",
         };
         
         var results = extractor.Extract(codeLines);

         Assert.Empty(results);
      }

      [Fact(DisplayName = "Handle unclosed normal strings")]
      public void HandleUnclosedLiteralString1()
      {
         var codeLines = new[]
         {
            "\" // Not a comment",  // " // Not a comment
         };

         var results = extractor.Extract(codeLines);

         Assert.Empty(results);
      }

      [Fact(DisplayName = "Handle unclosed string literal after closed one on same line")]
      public void HandleUnclosedLiteralString2()
      {
         var codeLines = new[]
         {
            "auto r = \"some\"  \" // NOT a comment",  // auto r = "some"  " // NOT a comment
         };

         var results = extractor.Extract(codeLines);
            
         Assert.Empty(results);
      }

      [Fact(DisplayName = "Handle line ends with R")]
      public void HandleUnclosedVebratimAfterAt()
      {
         var codeLines = new[]
         {
            "auto s = R",
         };

         var results = extractor.Extract(codeLines);

         Assert.Empty(results);
      }

      [Fact(DisplayName = "Handle line ends at with single slash")]
      public void HandleLineEndsWithSingleSlash()
      {
         var codeLines = new[]
         {
            "auto i = 2; /",
         };

         var results = extractor.Extract(codeLines);

         Assert.Empty(results);
      }

      [Fact(DisplayName = "Handle line ends with unclosed string at \"")]
      public void HandleLineeEndsWithQuote()
      {
         var codeLines = new[]
         {
            "auto i = \"",
         };

         var results = extractor.Extract(codeLines);

         Assert.Empty(results);
      }

      [Fact(DisplayName = "Handle raw string  with space in delimiter")]
      public void HandleUninitializedRawString_2()
      {
         var codeLines = new[]
         {
            "auto r = R\" (string) \";" + fullComment,
         };

         var results = extractor.Extract(codeLines);

         Assert.Single(results);
         Assert.Equal(TODO_TOKEN, results[0].Classification.Token);
         Assert.Equal(COMMENT_CONTENT, results[0].Content);
      }
   }
}
