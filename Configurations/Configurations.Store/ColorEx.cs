using System.Windows.Media;

namespace Configurations.Store
{
   public static class ColorEx
   {
      public static int ToInt(this Color color)
         => color.A << 24 | color.R << 16 | color.G << 8 | color.B;

      public static Color FromInt(int n)
         => Color.FromArgb((byte)(n >> 24), (byte)(n >> 16), (byte)(n >> 8), (byte)n);
   }
}
