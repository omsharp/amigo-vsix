using System.Collections.Generic;
using System.Configuration;

namespace VSIX.Package.Utils
{
   public static class StringExtensions
   {
      static public int[] IndicesOf(this string source, string target, bool caseSensitive = false)
      {
         var indices = new List<int>();

         var tgt = caseSensitive ? target : target.ToLower();
         var temp = caseSensitive ? source : source.ToLower();
         var currIndex = temp.IndexOf(tgt);

         while (currIndex > -1)
         {
            indices.Add(currIndex);
            currIndex = temp.IndexOf(tgt, currIndex + target.Length);
         }

         return indices.ToArray();
      }

      static public int CountOf(this string source, string target, bool caseSensitive = false)
      {
         var count = 0; 

         var tgt = caseSensitive ? target : target.ToLower();
         var temp = caseSensitive ? source : source.ToLower();
         var currIndex = temp.IndexOf(tgt);

         while (currIndex > -1)
         {
            count++;
            currIndex = temp.IndexOf(tgt, currIndex + target.Length);
         }

         return count;
      }

      /// <summary>
      /// Returns the first non empty character starting from the given position.
      /// Returns -1 if string is empty.
      /// </summary>
      static public int IndexOfFirstChar(this string source, int start = 0)
      {
         var index = start;
         while (index < source.Length)
         {
            if (source[index] != ' ')
               return index;

            index++;
         }

         return -1;
      }
   }
}