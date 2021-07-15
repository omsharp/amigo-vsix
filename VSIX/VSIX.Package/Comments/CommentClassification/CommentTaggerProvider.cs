using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;
using System;
using System.ComponentModel.Composition;
using VSIX.Package.Utils;

namespace VSIX.Package.Comments.CommentClassification
{
   [ContentType("csharp")]
   [ContentType("c/c++")]
   //[ContentType("JScript")]
   //[ContentType("TypeScript")]
   [ContentType("basic")]
   [TagType(typeof(IClassificationTag))]
   [Export(typeof(IViewTaggerProvider))]
   internal class CommentTaggerProvider : IViewTaggerProvider
   {

#pragma warning disable 0649
      [Import]
      internal IClassificationTypeRegistryService reg;

      [Import]
      internal IBufferTagAggregatorFactoryService aggService;
#pragma warning restore 0649

      public ITagger<T> CreateTagger<T>(ITextView view, ITextBuffer buffer) where T : ITag
      {
         reg.SyncWithConfigs();

         buffer.Changed += Buffer_Changed;

         return view.Properties.GetOrCreateSingletonProperty(() =>
            new CommentTagger(reg, aggService.CreateTagAggregator<IClassificationTag>(buffer),
            buffer.ContentType))
            as ITagger<T>;
      }

      private void Buffer_Changed(object sender, TextContentChangedEventArgs e)
      {
         ThreadHelper.ThrowIfNotOnUIThread();

         ViewModelLocator.Instance.Scanner.ScanActivedocument();
      }
   }
}
