namespace VSIX.Package.Utils
{
   public static class IntExtensions
   {
      public static bool IsEven(this int i) => i % 2 == 0;
      public static bool IsOdd(this int i) => i % 2 != 0;
      public static bool IsEvenAndNotZero(this int i) => IsEven(i) && i != 0;
   }
}