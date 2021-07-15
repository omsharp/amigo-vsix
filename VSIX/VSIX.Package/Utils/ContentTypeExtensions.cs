using Microsoft.VisualStudio.Utilities;

namespace VSIX.Package.Utils
{
   public static class ContentTypeExtensions
   {
      public static string GetCommentDelimiter(this IContentType contentType)
      {
         if (contentType.IsOfType("CSharp")
          || contentType.IsOfType("C/C++")
          || contentType.IsOfType("F#")
          || contentType.IsOfType("JScript")
          || contentType.IsOfType("TypeScript"))
         {
            return "//";
         }
         else if (contentType.IsOfType("Basic"))
         {
            return "'";
         }

         else if (contentType.TypeName.ToLower().Contains("xaml")
               || contentType.TypeName.ToLower().Contains("html"))
         {
            return "<!--";
         }

         return string.Empty;
      }
   }
}
