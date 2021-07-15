using System.Windows.Media;

namespace Configurations.Core.Comments
{
   public class DefaultStyle : IStyle
   {
      private static DefaultStyle instance;
      private static readonly object lockObject = new object();

      #region Original Attributes
      public Color OriginalBackground { get; private set; } = Colors.White;
      #endregion

      public string Font { get; set; } = "Arial";

      public Color Foreground { get; set; } = Color.FromRgb(52, 238, 52);

      public Color VSBackground { get; set; } = Color.FromRgb(22, 22, 22);

      public bool UseVSBackground { get; set; } = true;

      public Color Background
      {
         get => UseVSBackground ? Colors.Transparent : OriginalBackground;
         set => OriginalBackground = value;
      }

      public double Opacity { get; set; } = 0.8;

      public double Size { get; set; } = 20;

      public bool Italic { get; set; } = true;

      public bool Bold { get; set; } = false;

      public bool Underline { get; set; } = false;

      public bool Strikethrough { get; set; } = false;

      public static DefaultStyle Instance
      {
         get
         {
            if (instance == null)
            {
               lock (lockObject)
               {
                  if (instance == null)
                     instance = new DefaultStyle();
               }
            }

            return instance;
         }
      }

      private DefaultStyle() { }
   }
}
