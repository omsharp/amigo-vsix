using Configurations.Core.Comments;

namespace VSIX.Package.Comments.Parsers
{
   public class CommentExtract
   {
      public int Line { get; }
      public int Column { get; }
      public Classification Classification { get; }
      public string Content { get; }

      public CommentExtract(int line, int column, Classification classification, string comment)
      {
         Line = line;
         Column = column;
         Classification = classification;
         Content = comment;
      }
   }
}