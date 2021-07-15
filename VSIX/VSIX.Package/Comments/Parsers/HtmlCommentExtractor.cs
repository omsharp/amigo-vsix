
using Configurations.Core.Comments;
using System.Collections.Generic;

namespace VSIX.Package.Comments.Parsers
{
   public class HtmlCommentExtractor : IBookmarkExtractor
   {

      public HtmlCommentExtractor(ICommentConfiguration commentConfig)
        { }

      public List<CommentExtract> Extract(string[] lines)
      {
         //todo find <script> </script> and pass it to js extractor




         throw new System.NotImplementedException();
      }
   }
}
