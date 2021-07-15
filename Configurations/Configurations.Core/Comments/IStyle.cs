using System.Windows.Media;

namespace Configurations.Core.Comments
{
   public interface IStyle
   {
      string Font { get; set; }
      Color Foreground { get; set; }
      Color Background { get; set; }
      double Opacity { get; set; }
      double Size { get; set; }
      bool Italic { get; set; }
      bool Bold { get; set; }
      bool Underline { get; set; }
      bool Strikethrough { get; set; }
   }
}