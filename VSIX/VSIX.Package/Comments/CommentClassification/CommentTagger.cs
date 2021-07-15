using Configurations.Core.Comments;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using VSIX.ConfigurationsService;
using VSIX.Package.Utils;

namespace VSIX.Package.Comments.CommentClassification
{
   public class CommentTagger : ITagger<IClassificationTag>
   {
      readonly ICommentConfiguration config;
      readonly IClassificationTypeRegistryService classReg;
      readonly ITagAggregator<IClassificationTag> tagAggregator;
      readonly string commentDelimiter;
      readonly IContentType contentType;

      public event EventHandler<SnapshotSpanEventArgs> TagsChanged;

      public CommentTagger(IClassificationTypeRegistryService reg,
                           ITagAggregator<IClassificationTag> agg,
                           IContentType contentType)
      {
         classReg = reg;
         tagAggregator = agg;
         this.contentType = contentType;

         config = ConfigService.Current.CommentConfiguration;

         commentDelimiter = contentType.GetCommentDelimiter();
      }

      public IEnumerable<ITagSpan<IClassificationTag>> GetTags(NormalizedSnapshotSpanCollection source)
      {
         foreach (var curSpan in tagAggregator.GetCommentSnapshotSpans(source))
         {
            var targetSpan = curSpan;

            if (contentType.IsOfType("TypeScript"))
            {
               if (!targetSpan.GetText().StartsWith("//"))
                  continue;

               var line = targetSpan.Snapshot.GetLineFromPosition(targetSpan.Start);
               var offset = line.GetText().IndexOf(commentDelimiter);

               targetSpan = new SnapshotSpan(targetSpan.Snapshot, line.Start + offset, line.Length - offset);
            }


            var spanText = targetSpan.GetText();

            // Make sure text is not empty, and that it's more than an empty comment
            if (spanText.Length < commentDelimiter.Length)
               continue;

            spanText = spanText.Substring(commentDelimiter.Length).Trim();

            foreach (var cls in config.Classifications)
            {
               if (spanText.StartsWith($"{cls.Token} ", StringComparison.OrdinalIgnoreCase))
               {
                  var newSpan = new SnapshotSpan(targetSpan.Snapshot,
                     targetSpan.Start + cls.Token.Length + commentDelimiter.Length,
                     targetSpan.Length - cls.Token.Length - commentDelimiter.Length);

                  yield return new TagSpan<IClassificationTag>(
                     newSpan,
                     new ClassificationTag(classReg.GetClassificationType(cls.Key))
                     );
               }
            }
         }
      }

      ////protected SnapshotSpan BuildSnapshotSpan(SnapshotSpan source, int start)
      ////{
      ////   var spanLength = source.Length - start;

      ////   //if (commentDelimiter == "<!--")
      ////   //   spanLength -= commentDelimiter.Length - 4;

      ////   return new SnapshotSpan(
      ////      source.Snapshot,
      ////      start,
      ////      spanLength);
      ////}
   }
}
