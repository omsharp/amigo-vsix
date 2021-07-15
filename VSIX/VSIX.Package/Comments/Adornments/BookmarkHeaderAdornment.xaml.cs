using System.Windows.Controls;
using System.Windows.Media;

namespace VSIX.Package.Comments.Adornments
{
   public partial class BookmarkHeaderAdornment : UserControl
   {
      public BookmarkHeaderAdornment(BookmarkHeaderTag tag)
      {
         InitializeComponent();
         Update(tag);
      }

      public void Update(BookmarkHeaderTag tag)
      {
         AdornmentBorder.BorderBrush = new SolidColorBrush(tag.Classification.Style.Foreground);

         AdornmentHeader.Text = tag.Classification.Name;
         AdornmentHeader.FontSize = tag.Classification.Style.Size;
         AdornmentHeader.FontFamily = new FontFamily(tag.Classification.Style.Font);
         AdornmentHeader.Foreground = new SolidColorBrush(tag.Classification.Style.Foreground);
      }
   }
}
