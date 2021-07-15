using Configurations.Core.Comments;
using System.Collections.Generic;
using System.Linq;

namespace VSIX.Package.Comments.Parsers
{
   public enum ExtractionBehaviour
   {
      AnyComment,
      ClassifiedOnly,
   }

   public interface IBookmarkExtractor
   {
      List<CommentExtract> Extract(string[] lines);
   }
}