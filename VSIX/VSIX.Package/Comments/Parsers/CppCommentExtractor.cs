using Configurations.Core.Comments;
using System;
using System.Collections.Generic;

namespace VSIX.Package.Comments.Parsers
{
   public class CppCommentExtractor : IBookmarkExtractor
   {
      readonly ICommentConfiguration config;
      public CppCommentExtractor(ICommentConfiguration config)
          { this.config = config; }

      public List<CommentExtract> Extract(string[] lines)
      {
         var result = new List<CommentExtract>();

         for (var l = 0; l < lines.Length; l++)
         {
            var charIndx = 0;
            // This is the main character scanning loop.
            // Loop through characters of each line, until the one before last.
            while (charIndx < lines[l].Length - 1)
            {
               // If a verbatim string (@") is found,
               // search for it's closing (").
               if (lines[l][charIndx] == 'R' && lines[l][charIndx + 1] == '"')
               {
                  var delStart = charIndx + 2;
                  var delEnd = lines[l].IndexOf("(", charIndx);

                  // if there is no "(" in this line, then move on to the next character.
                  if (delEnd < 0)
                  {
                     charIndx++;
                     continue;
                  }

                  var delimiter = lines[l].Substring(delStart, delEnd - delStart);

                  // if delimiter contains "\" or ")" or " ", then it's invalid.
                  // Move on to the next character, starting after delimiter.
                  if (delimiter.Contains("\\") || delimiter.Contains(")"))
                  {
                     charIndx = delEnd + 1;
                     continue;
                  }


                  // Closer of the raw string literal
                  var closer = string.IsNullOrEmpty(delimiter)
                             ? $")\""
                             : $"){delimiter}\"";

                  // Search for the closer in lines, starting from current line.
                  while (l < lines.Length)
                  {
                     var closerIndex = lines[l].IndexOf(closer, charIndx);

                     if (closerIndex > -1)
                     {
                        charIndx = closerIndex + closer.Length - 1;
                        break;
                     }

                     // If a closing (") is not found yet.
                     // Move line index to next line.
                     // Set character index to first character.
                     // Continue searching for the verbatim's closing.
                     l++;
                     charIndx = 0;
                  }

                  // If line index >= total number of lines.
                  // then document is done, break the main character scanning loop.
                  if (l >= lines.Length)
                     break;
               }
               // Handle normal string literals
               else if (lines[l][charIndx] == '"')
               {
                  // Starting scanning after the opening (")
                  charIndx++;

                  // Search for the closing (").
                  // If not found, then the end of the line is reached
                  while (charIndx < lines[l].Length)
                  {
                     if (lines[l][charIndx] == '\\')
                     {
                        charIndx += 2;
                        continue;
                     }
                     else if (lines[l][charIndx] == '"')
                     {
                        break;
                     }

                     charIndx++;
                  }
               }
               // Check if the current character is a start of a comment
               else if (lines[l][charIndx] == '/' && lines[l][charIndx + 1] == '/')
               {
                  // Get the comment starting after (//) and build the comment extract.
                  // And break the main character loop to scan next line.
                  var text = lines[l].Substring(charIndx + 2).TrimStart();
                  var line = l + 1;

                  foreach (var cls in config.Classifications)
                  {
                     if (text.StartsWith($"{cls.Token} ", StringComparison.OrdinalIgnoreCase))
                     {
                        var start = text.IndexOf(" ");
                        result.Add(new CommentExtract(line, charIndx, cls, text.Substring(start).Trim()));
                     }
                  }

                  break;
               }

               charIndx++;
            }
         }

         return result;
      }
   }
}
