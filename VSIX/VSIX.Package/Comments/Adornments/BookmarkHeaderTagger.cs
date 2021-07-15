using System;
using System.Collections.Generic;
using Configurations.Core.Comments;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;
using VSIX.ConfigurationsService;
using VSIX.Package.Comments.Parsers;
using VSIX.Package.Utils;

namespace VSIX.Package.Comments.Adornments
{
   /// <summary>
   /// Determines which spans of text likely refer to color values.
   /// </summary>
   /// <remarks>
   /// <para>
   /// This is a data-only component. The tagging system is a good fit for presenting data-about-text.
   /// The <see cref="ColorAdornmentTagger"/> takes color tags produced by this tagger and creates corresponding UI for this data.
   /// </para>
   /// <para>
   /// This class is a sample usage of the <see cref="RegexTagger"/> utility base class.
   /// </para>
   /// </remarks>
   public sealed class BookmarkHeaderTagger : ITagger<BookmarkHeaderTag>
   {

      private readonly ICommentConfiguration configs;
      private readonly ITagAggregator<IClassificationTag> tagAggregator;
      private readonly string commentDelimiter;
      private readonly IContentType contentType;

      public event EventHandler<SnapshotSpanEventArgs> TagsChanged;


      public BookmarkHeaderTagger(ITextBuffer buffer, ITagAggregator<IClassificationTag> agg, IContentType contentType)
      {
         configs = ConfigService.Current.CommentConfiguration;
         buffer.Changed += (sender, args) => HandleBufferChanged(args);
         tagAggregator = agg;
         this.contentType = contentType;
         commentDelimiter = contentType.GetCommentDelimiter();
      }

      public IEnumerable<ITagSpan<BookmarkHeaderTag>> GetTags(NormalizedSnapshotSpanCollection source)
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

            spanText = spanText.Substring(commentDelimiter.Length).TrimStart();

            foreach (var cls in configs.Classifications)
            {
               if (spanText.StartsWith($"{cls.Token} ", StringComparison.OrdinalIgnoreCase))
               {
                  var tokenIndex = targetSpan.GetText().IndexOf(cls.Token, commentDelimiter.Length, StringComparison.OrdinalIgnoreCase);
                  var emptySpace = tokenIndex - commentDelimiter.Length;
                  var length = commentDelimiter.Length + emptySpace + cls.Token.Length;

                  var newSpan = new SnapshotSpan(targetSpan.Start, length);

                  yield return new TagSpan<BookmarkHeaderTag>(
                     newSpan,
                     new BookmarkHeaderTag(cls)
                     );
               }
            }
         }
      }

      /// <summary>
      /// Handle buffer changes. The default implementation expands changes to full lines and sends out
      /// a <see cref="TagsChanged"/> event for these lines.
      /// </summary>
      /// <param name="args">The buffer change arguments.</param>
      private void HandleBufferChanged(TextContentChangedEventArgs args)
      {
         if (args.Changes.Count == 0)
            return;

         var temp = TagsChanged;
         if (temp == null)
            return;

         // Combine all changes into a single span so that
         // the ITagger<>.TagsChanged event can be raised just once for a compound edit
         // with many parts.

         var snapshot = args.After;

         var start = args.Changes[0].NewPosition;
         var end = args.Changes[args.Changes.Count - 1].NewEnd;

         var totalAffectedSpan = new SnapshotSpan(
             snapshot.GetLineFromPosition(start).Start,
             snapshot.GetLineFromPosition(end).End);

         temp(this, new SnapshotSpanEventArgs(totalAffectedSpan));
      }
   }
}