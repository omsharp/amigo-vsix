using Configurations.Core.Comments;
using Mvvm.Core;
using PropertyChanged;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Configurations.UI.ViewModels.Comments
{
   public class DefaultsPageViewModel : ViewModelBase, IPageViewModel
   {
      private readonly ICommentConfiguration config;

      public string Title => "Defaults";

      public string Font
      {
         get => config.Defaults.Font;
         set => config.Defaults.Font = value;
      }

      public Color Foreground
      {
         get => config.Defaults.Foreground;
         set => config.Defaults.Foreground = value;
      }

      public bool UseVSBackground
      {
         get => config.Defaults.UseVSBackground;
         set
         {
            config.Defaults.UseVSBackground = value;
         }
      }

      public Color VSBackground => config.Defaults.VSBackground;

      [AlsoNotifyFor(nameof(UseVSBackground))]
      public Color Background
      {
         get
         {
            if (UseVSBackground)
               return config.Defaults.VSBackground;
            else
               return config.Defaults.Background;
         }
         set => config.Defaults.Background = value;
      }

      public double Opacity
      {
         get => config.Defaults.Opacity;
         set => config.Defaults.Opacity = value;
      }

      public double Size
      {
         get => config.Defaults.Size;
         set => config.Defaults.Size = value;
      }

      public bool Italic
      {
         get => config.Defaults.Italic;
         set => config.Defaults.Italic = value;
      }

      public bool Bold
      {
         get => config.Defaults.Bold;
         set => config.Defaults.Bold = value;
      }

      public bool Underline
      {
         get => config.Defaults.Underline;
         set => config.Defaults.Underline = value;
      }

      public bool Strikethrough
      {
         get => config.Defaults.Strikethrough;
         set => config.Defaults.Strikethrough = value;
      }

      public FontStyle FontStyle => Italic ? FontStyles.Italic : FontStyles.Normal;

      public FontWeight FontWeight => Bold ? FontWeights.Bold : FontWeights.Normal;

      public TextDecorationCollection TextDecoration
      {
         get
         {
            var decorations = new TextDecorationCollection();

            if (Underline)
               decorations.Add(TextDecorations.Underline);

            if (Strikethrough)
               decorations.Add(TextDecorations.Strikethrough);

            return decorations;
         }
      }

      public List<string> Fonts { get; set; } = new List<string>();

      public DefaultsPageViewModel(ICommentConfiguration config)
      {
         this.config = config;

         using (var fonts = new InstalledFontCollection())
         {
            foreach (var font in fonts.Families.OrderBy(f => f.Name))
               Fonts.Add(font.Name);
         }
      }
   }
}
