using System.Windows.Media;

namespace Configurations.Core.Comments
{
   public class CustomStyle : IStyle
   {
      static readonly DefaultStyle defaults
         = DefaultStyle.Instance;
      
      #region Original Attributes
      public string Name { get; set; }
      public string OriginalFont { get; private set; } = defaults.Font;
      public Color OriginalForeground { get; private set; } = defaults.Foreground;
      public Color OriginalBackground { get; private set; } = defaults.Background;
      public double OriginalOpacity { get; private set; } = defaults.Opacity;
      public double OriginalSize { get; private set; } = defaults.Size;
      public bool OriginalItalic { get; private set; } = defaults.Italic;
      public bool OriginalBold { get; private set; } = defaults.Bold;
      public bool OriginalUnderline { get; private set; } = defaults.Underline;
      public bool OriginalStrikethrough { get; private set; } = defaults.Strikethrough;
      #endregion

      
      public bool UseDefaultFont { get; set; }
      public string Font
      {
         get { return UseDefaultFont ? defaults.Font : OriginalFont; }
         set { OriginalFont = value; }
      }

      public bool UseDefaultForeground { get; set; }
      public Color Foreground
      {
         get { return UseDefaultForeground ? defaults.Foreground : OriginalForeground; }
         set { OriginalForeground = value; }
      }

      public bool UseDefaultBackground { get; set; }
      public Color Background
      {
         get { return UseDefaultBackground ? defaults.Background : OriginalBackground; }
         set { OriginalBackground = value; }
      }

      public bool UseDefaultOpacity { get; set; }
      public double Opacity
      {
         get { return UseDefaultOpacity ? defaults.Opacity : OriginalOpacity; }
         set { OriginalOpacity = value; }
      }

      public bool UseDefaultSize { get; set; }
      public double Size
      {
         get { return UseDefaultSize ? defaults.Size : OriginalSize; }
         set { OriginalSize = value; }
      }

      public bool UseDefaultItalic { get; set; }
      public bool Italic
      {
         get { return UseDefaultItalic ? defaults.Italic : OriginalItalic; }
         set { OriginalItalic = value; }
      }

      public bool UseDefaultBold { get; set; }
      public bool Bold
      {
         get { return UseDefaultBold ? defaults.Bold : OriginalBold; }
         set { OriginalBold = value; }
      }

      public bool UseDefaultUnderline { get; set; }
      public bool Underline
      {
         get { return UseDefaultUnderline ? defaults.Underline : OriginalUnderline; }
         set { OriginalUnderline = value; }
      }

      public bool UseDefaultStrikethrough { get; set; }
      public bool Strikethrough
      {
         get { return UseDefaultStrikethrough ? defaults.Strikethrough : OriginalStrikethrough; }
         set { OriginalStrikethrough = value; }
      }

      public override string ToString() => Name;
   }
}