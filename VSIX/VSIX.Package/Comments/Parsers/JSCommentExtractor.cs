using Configurations.Core.Comments;
using System;
using System.Collections.Generic;

namespace VSIX.Package.Comments.Parsers
{
   public class JSCommentExtractor : IBookmarkExtractor
   {
      readonly ICommentConfiguration config;
      public JSCommentExtractor(ICommentConfiguration config)
       { this.config = config; }

      public List<CommentExtract> Extract(string[] lines)
      {
         var result = new List<CommentExtract>();

         for (var l = 0; l < lines.Length; l++)
         {
            var charIndx = 0;
            // This is the main character scanning loop.
            // Loop through characters of each line, until the one before last.
            while (charIndx < lines[l].Length)
            {
               // Handle Template Literals
               if (lines[l][charIndx] == '`')
               {
                  // Start scanning at the next character index.
                  charIndx++;

                  var endFound = false;

                  while (l < lines.Length)
                  {
                     while (charIndx < lines[l].Length)
                     {

                        if (lines[l][charIndx] == '\\')
                        {
                           charIndx += 2;
                           continue;
                        }
                        else if (lines[l][charIndx] == '`')
                        {
                           endFound = true;
                           break;
                        }

                        charIndx++;
                     }

                     if (endFound) break;

                     // No closing tick found
                     // Move to the next line, and set character index to the start.
                     l++;
                     charIndx = 0;
                  }

                  // If line index >= total number of lines.
                  // then document is done, break the main character scanning loop.
                  if (l >= lines.Length)
                     break;
               }
               // Handle strings with (") or (')
               else if (lines[l][charIndx] == '\'' || lines[l][charIndx] == '"')
               {
                  // This might be (") or (')
                  var quote = lines[l][charIndx];

                  // Starting scanning after the opening (") or (')
                  charIndx++;

                  // Search for the closing (") or (').
                  // If not found, then the end of the line is reached.
                  while (charIndx < lines[l].Length)
                  {
                     // Check if it's the end of the line
                     if (charIndx == lines[l].Length - 1)
                     {
                        // If current line ends with (\) (multiline string) then advance line index,
                        // reset the character index,
                        // and continue scanning.
                        if (lines[l][charIndx] == '\\' && lines[l][charIndx - 1] != '\\')
                        {
                           l++;
                           charIndx = 0;
                           continue;
                        }
                     }
                     else // Not the end of the line
                     {
                        if (lines[l][charIndx] == '\\')
                        {
                           charIndx += 2;
                           continue;
                        }
                        else if (lines[l][charIndx] == quote)
                        {
                           break;
                        }
                     }

                     charIndx++;
                  }
               }
               // Check if the current character is a start of a comment
               else if (lines[l][charIndx] == '/')
               {
                  // Make sure it's not the last character in the line
                  // and that it's a double slash.
                  if (charIndx < lines[l].Length - 1 && lines[l][charIndx + 1] == '/')
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
               }

               charIndx++;
            }
         }

         return result;
      }
   }
}
