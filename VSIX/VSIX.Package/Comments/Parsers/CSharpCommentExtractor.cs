using Configurations.Core.Comments;
using System;
using System.Collections.Generic;

namespace VSIX.Package.Comments.Parsers
{
   public class CSharpCommentExtractor : IBookmarkExtractor
   {
      readonly ICommentConfiguration config;
      public CSharpCommentExtractor(ICommentConfiguration config)
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
               if (lines[l][charIndx] == '@' && lines[l][charIndx + 1] == '"')
               {

                  charIndx += 2; // Set character index after (@")
                  var endFound = false;

                  // Search for the closing (") of the verbatim string
                  // Looping though lines, starting from current line.
                  while (l < lines.Length)
                  {
                     // Loop through the characters of current line
                     while (charIndx < lines[l].Length)
                     {
                        if (lines[l][charIndx] == '"')
                        {
                           // If escaped with (""), move character indexer +2 and continue.
                           if (charIndx < lines[l].Length - 1 && lines[l][charIndx + 1] == '"')
                           {
                              charIndx += 2;
                              continue;
                           }
                           else
                           {
                              // This is the closing quote.
                              // Break verbatim character scanning loop
                              endFound = true;
                              break;
                           }
                        }

                        charIndx++;
                     }

                     // If closing (") is found, then break verbatim line iteration.
                     // The main character scanning loop will continue at charIndx++.
                     if (endFound) break;

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