using Configurations.Core.Comments;
using Configurations.UI.ViewModels.Comments;
using System.Windows.Media;
using Xunit;

namespace WPF.Tests.Comments.ViewModels
{ 
   public class StyleViewModel_Attributes
   {
      readonly DefaultStyle defaults = DefaultStyle.Instance;

      [Fact(DisplayName = "Name Attribute")]
      public void Name()
      {
         var style = new CustomStyle();
         var viewModel = new StyleViewModel(style, null);

         viewModel.Name = "Note";
         Assert.Equal(viewModel.Name, style.Name);
      }

      [Fact(DisplayName = "Font Attribute")]
      public void Font()
      {
         var style = new CustomStyle();
         var viewModel = new StyleViewModel(style, null);

         viewModel.Font = "Consolas";
         Assert.Equal(viewModel.Font, style.Font);

         // testing UseDefault
         viewModel.UseDefaultFont = true;

         Assert.True(style.UseDefaultFont);
         Assert.Equal(defaults.Font, style.Font);
         Assert.Equal(defaults.Font, viewModel.Font);
      }

      [Fact(DisplayName = "Foreground Attribute")]
      public void Foreground()
      {
         var style = new CustomStyle();
         var viewModel = new StyleViewModel(style, null);

         viewModel.Foreground = Colors.Purple;
         Assert.Equal(viewModel.Foreground, style.Foreground);

         // testing UseDefault
         viewModel.UseDefaultForeground = true;
         Assert.True(style.UseDefaultForeground);
         Assert.Equal(defaults.Foreground, style.Foreground);
         Assert.Equal(defaults.Foreground, viewModel.Foreground);
      }

      [Fact(DisplayName = "Background Attribute")]
      public void Background()
      {
         var style = new CustomStyle();
         var viewModel = new StyleViewModel(style, null);

         viewModel.Background = Colors.Purple;
         Assert.Equal(viewModel.Background, style.Background);

         // testing UseDefault
         viewModel.UseDefaultBackground = true;
         Assert.True(style.UseDefaultBackground);
         Assert.Equal(defaults.Background, style.Background);
         Assert.Equal(defaults.Background, viewModel.Background);
      }

      [Fact(DisplayName = "Opacity Attribute")]
      public void Opacity()
      {
         var style = new CustomStyle();
         var viewModel = new StyleViewModel(style, null);

         viewModel.Opacity = 0.4;
         Assert.Equal(viewModel.Opacity, style.Opacity);

         // testing UseDefault
         viewModel.UseDefaultOpacity = true;
         Assert.True(style.UseDefaultOpacity);
         Assert.Equal(defaults.Opacity, style.Opacity);
         Assert.Equal(defaults.Opacity, viewModel.Opacity);
      }

      [Fact(DisplayName = "Size Attribute")]
      public void Size()
      {
         var style = new CustomStyle();
         var viewModel = new StyleViewModel(style, null);

         viewModel.Size = 0.4;
         Assert.Equal(viewModel.Size, style.Size);

         // testing UseDefault
         viewModel.UseDefaultSize = true;
         Assert.True(style.UseDefaultSize);
         Assert.Equal(defaults.Size, style.Size);
         Assert.Equal(defaults.Size, viewModel.Size);
      }

      [Fact(DisplayName = "Italic Attribute")]
      public void Italic()
      {
         var style = new CustomStyle();
         var viewModel = new StyleViewModel(style, null);

         viewModel.Italic = true;
         Assert.Equal(viewModel.Italic, style.Italic);

         // testing UseDefault
         viewModel.UseDefaultItalic = true;
         Assert.True(style.UseDefaultItalic);
         Assert.Equal(defaults.Italic, style.Italic);
         Assert.Equal(defaults.Italic, viewModel.Italic);
      }


      [Fact(DisplayName = "Bold Attribute")]
      public void Bold()
      {
         var style = new CustomStyle();
         var viewModel = new StyleViewModel(style, null);

         viewModel.Bold = true;
         Assert.Equal(viewModel.Bold, style.Bold);

         // testing UseDefault
         viewModel.UseDefaultBold = true;
         Assert.True(style.UseDefaultBold);
         Assert.Equal(defaults.Bold, style.Bold);
         Assert.Equal(defaults.Bold, viewModel.Bold);
      }

      [Fact(DisplayName = "Underline Attribute")]
      public void Underline()
      {
         var style = new CustomStyle();
         var viewModel = new StyleViewModel(style, null);

         viewModel.Underline = true;
         Assert.Equal(viewModel.Underline, style.Underline);

         // testing UseDefault
         viewModel.UseDefaultUnderline = true;
         Assert.True(style.UseDefaultUnderline);
         Assert.Equal(defaults.Underline, style.Underline);
         Assert.Equal(defaults.Underline, viewModel.Underline);
      }

      [Fact(DisplayName = "Strikethrough Attribute")]
      public void Strikethrough()
      {
         var style = new CustomStyle();
         var viewModel = new StyleViewModel(style, null);

         viewModel.Strikethrough = true;
         Assert.Equal(viewModel.Strikethrough, style.Strikethrough);

         // testing UseDefault
         viewModel.UseDefaultStrikethrough = true;
         Assert.True(style.UseDefaultStrikethrough);
         Assert.Equal(defaults.Strikethrough, style.Strikethrough);
         Assert.Equal(defaults.Strikethrough, viewModel.Strikethrough);
      }
   }
}
