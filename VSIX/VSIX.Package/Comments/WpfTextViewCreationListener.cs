using Configurations.Core.Comments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using VSIX.ConfigurationsService;
using VSIX.Package.Utils;

namespace VSIX.Package.Comments
{
   //! Single isntance of this class is created for the whole preject.
   //? Not sure if the same instance is working for the whole solution tho!

   [ContentType("code")]
   [Export(typeof(IWpfTextViewCreationListener))]
   [TextViewRole(PredefinedTextViewRoles.Document)]
   internal sealed class WpfTextViewCreationListener : IWpfTextViewCreationListener
   {

      ICommentConfiguration configs;
      IClassificationFormatMap map;

#pragma warning disable 0649
      [Import]
      private IClassificationFormatMapService formatMapService;

      [Import]
      private IClassificationTypeRegistryService classificationService;
#pragma warning restore 0649

      public WpfTextViewCreationListener()
      {
         configs = ConfigService.Current.CommentConfiguration;
         configs.ChangesSaved += CommentConfiguration_ChangesSaved;
      }

      public void TextViewCreated(IWpfTextView view)
      {
         if (map == null)
            map = formatMapService.GetClassificationFormatMap(view);

         classificationService.SyncWithConfigs();

         Decorate();
      }

      private void Decorate()
      {
         //? Have some sort of a look up table for classification or something!

         // Handle normal comments
         var q = map.CurrentPriorityOrder.Where(t => t != null && t.Classification.ToLower().Contains("comment"));
         foreach (var t in q)
            SetProperties(t, configs.Defaults, map);

         // Handle classified comments
         foreach (var classification in configs.Classifications)
         {
            SetProperties(
               classificationService.GetClassificationType(classification.Key),
               classification.Style,
               map);
         }
      }

      private void CommentConfiguration_ChangesSaved(object sender, System.EventArgs e)
      {
         classificationService.SyncWithConfigs();
         Decorate();
      }

      private void SetProperties(IClassificationType type, IStyle style, IClassificationFormatMap map)
      {
         var props = map.GetExplicitTextProperties(type)
                        .SetTypeface(new Typeface(style.Font))
                        .SetForeground(style.Foreground)
                        .SetBackground(style.Background)
                        .SetForegroundOpacity(style.Opacity)
                        .SetFontRenderingEmSize(style.Size)
                        .SetItalic(style.Italic)
                        .SetBold(style.Bold);

         var textDecorations = new TextDecorationCollection();

         //? props.TextDecorations.Add(TextDecorations.Underline);
         if (style.Underline)
            textDecorations.Add(TextDecorations.Underline);

         //? props.TextDecorations.Add(TextDecorations.Strikethrough);
         if (style.Strikethrough)
            textDecorations.Add(TextDecorations.Strikethrough);

         //? remove this line when using TextDecorations.Add()
         props = props.SetTextDecorations(textDecorations);

         map.AddExplicitTextProperties(type, props);
      }
   }
}
