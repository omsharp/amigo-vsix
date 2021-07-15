using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;
using VSIX.Package.Utils;

namespace VSIX.Package.Comments.Adornments
{
   [ContentType("csharp")]
   [ContentType("c/c++")]
   //[ContentType("JScript")]
   //[ContentType("TypeScript")]
   [ContentType("basic")]
   [TagType(typeof(BookmarkHeaderTag))]
   [Export(typeof(ITaggerProvider))]
   internal sealed class BookmarkHeaderTaggerProvider : ITaggerProvider
   {

#pragma warning disable 0649
      [Import]
      internal IClassificationTypeRegistryService reg;

      [Import]
      internal IBufferTagAggregatorFactoryService aggService;
#pragma warning restore 0649

      public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
      {
         if (buffer == null)
            throw new ArgumentNullException("buffer");

         reg.SyncWithConfigs();
         
         var agg = aggService.CreateTagAggregator<IClassificationTag>(buffer);

         return buffer.Properties.GetOrCreateSingletonProperty(
            () => new BookmarkHeaderTagger(buffer, agg, buffer.ContentType))
            as ITagger<T>;
      }
   }
}
