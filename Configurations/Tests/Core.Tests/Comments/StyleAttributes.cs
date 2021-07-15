using Xunit;
using System.Windows.Media;
using Configurations.Core.Comments;

namespace Core.Tests.Comments
{
   public class StyleAttributes
   {
      public StyleAttributes()
      {
         DefaultStyle.Instance.Font = "Consolas";
         DefaultStyle.Instance.Foreground = Colors.Purple;
         DefaultStyle.Instance.Background = Colors.CadetBlue;
         DefaultStyle.Instance.Opacity = 0.8d;
         DefaultStyle.Instance.Size = 23d;
         DefaultStyle.Instance.Italic = false;
         DefaultStyle.Instance.Bold = false;
         DefaultStyle.Instance.Underline = false;
         DefaultStyle.Instance.Strikethrough = false;
      }

      [Fact(DisplayName = "Font : Get Value")]
      public void FontAttribute_GetValue()
      {
         var style = new CustomStyle();
         var arial = "Arial";
         style.Font = arial;

         Assert.Equal(arial, style.Font);
      }

      [Fact(DisplayName = "Font : Get Default")]
      public void FontAttribute_GetDefaultValue()
      {
         var style = new CustomStyle();
         style.Font = "Arial";
         style.UseDefaultFont = true;

         Assert.Equal(DefaultStyle.Instance.Font, style.Font);
      }

      [Fact(DisplayName = "Foreground : Get Value")]
      public void ForegroundAttribute_GetValue()
      {
         var style = new CustomStyle();
         var color = Colors.Red;
         style.Foreground = color;

         Assert.Equal(color, style.Foreground);
      }

      [Fact(DisplayName = "Foreground : Get Default")]
      public void ForegroundAttribute_GetDefaultValue()
      {
         var style = new CustomStyle();
         style.Foreground = Colors.Red;
         style.UseDefaultForeground = true;

         Assert.Equal(DefaultStyle.Instance.Foreground, style.Foreground);
      }

      [Fact(DisplayName = "Background : Get Value")]
      public void BackgroundAttribute_GetValue()
      {
         var style = new CustomStyle();
         var color = Colors.Red;
         style.Background = color;

         Assert.Equal(color, style.Background);
      }

      [Fact(DisplayName = "Background : Get Default")]
      public void BackgroundAttribute_GetDefaultValue()
      {
         var style = new CustomStyle();
         style.UseDefaultBackground = true;

         Assert.Equal(DefaultStyle.Instance.Background, style.Background);
      }

      [Fact(DisplayName = "Background : Get Default - VS")]
      public void BackgroundAttribute_GetDefaultValueVS()
      {
         DefaultStyle.Instance.VSBackground = Colors.Purple;
         DefaultStyle.Instance.UseVSBackground = true;

         var style = new CustomStyle();
         style.UseDefaultBackground = true;

         Assert.Equal(Colors.Transparent, style.Background);
      }


      [Fact(DisplayName = "Opacity : Get Value")]
      public void OpacityAttribute_GetValue()
      {
         var style = new CustomStyle();
         var value = 1.0d;
         style.Opacity = value; ;

         Assert.Equal(value, style.Opacity);
      }

      [Fact(DisplayName = "Opacity : Get Default")]
      public void OpacityAttribute_GetDefaultValue()
      {
         var style = new CustomStyle();
         style.UseDefaultOpacity = true;

         Assert.Equal(DefaultStyle.Instance.Opacity, style.Opacity);
      }

      [Fact(DisplayName = "Size : Get Value")]
      public void SizeAttribute_GetValue()
      {
         var style = new CustomStyle();
         var value = 1.0d;
         style.Size = value; ;

         Assert.Equal(value, style.Size);
      }

      [Fact(DisplayName = "Size : Get Default")]
      public void SizeAttribute_GetDefaultValue()
      {
         var style = new CustomStyle();
         style.UseDefaultSize = true;

         Assert.Equal(DefaultStyle.Instance.Size, style.Size);
      }

      [Fact(DisplayName = "Italic : Get Value")]
      public void ItalicAttribute_GetValue()
      {
         var style = new CustomStyle();
         var value = true;
         style.Italic = value; ;

         Assert.Equal(value, style.Italic);
      }

      [Fact(DisplayName = "Italic : Get Default")]
      public void ItalicAttribute_GetDefaultValue()
      {
         var style = new CustomStyle();
         style.UseDefaultItalic = true;

         Assert.Equal(DefaultStyle.Instance.Italic, style.Italic);
      }

      [Fact(DisplayName = "Bold : Get Value")]
      public void BoldAttribute_GetValue()
      {
         var style = new CustomStyle();
         var value = true;
         style.Bold = value; ;

         Assert.Equal(value, style.Bold);
      }

      [Fact(DisplayName = "Bold : Get Default")]
      public void BoldAttribute_GetDefaultValue()
      {
         var style = new CustomStyle();
         style.UseDefaultBold = true;

         Assert.Equal(DefaultStyle.Instance.Bold, style.Bold);
      }

      [Fact(DisplayName = "Underline : Get Value")]
      public void UnderlineAttribute_GetValue()
      {
         var style = new CustomStyle();
         var value = true;
         style.Underline = value; ;

         Assert.Equal(value, style.Underline);
      }

      [Fact(DisplayName = "Underline : Get Default")]
      public void UnderlineAttribute_GetDefaultValue()
      {
         var style = new CustomStyle();
         style.UseDefaultUnderline = true;

         Assert.Equal(DefaultStyle.Instance.Underline, style.Underline);
      }

      [Fact(DisplayName = "Strikethrough : Get Value")]
      public void StrikethroughAttribute_GetValue()
      {
         var style = new CustomStyle();
         var value = true;
         style.Strikethrough = value; ;

         Assert.Equal(value, style.Strikethrough);
      }

      [Fact(DisplayName = "Strikethrough : Get Default")]
      public void StrikethroughAttribute_GetDefaultValue()
      {
         var style = new CustomStyle();
         style.UseDefaultStrikethrough = true;

         Assert.Equal(DefaultStyle.Instance.Strikethrough, style.Strikethrough);
      }
   }

}
