using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;
using System.Linq;

namespace VSIX.Package.Utils
{
   public static class TagAggregatorExtensions
   {
      public static IEnumerable<SnapshotSpan> GetCommentSnapshotSpans(
         this ITagAggregator<IClassificationTag> agg,
         NormalizedSnapshotSpanCollection source)
      {
         return agg.GetTags(source)
                   .Where(m => m.Tag.ClassificationType.Classification.ToLower().Contains("comment"))
                   .SelectMany(ts => ts.Span.GetSpans(source[0].Snapshot));
      }
   }
}
