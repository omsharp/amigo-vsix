using System;
using System.ComponentModel;

namespace Configurations.Core.Comments
{
   public enum StyleApplication
   {
      [Description("All")]
      All,

      [Description("Comment Body")]
      BodyOnly,

      [Description("Token Only")]
      TokenOnly
   }

   public enum CapitalizationType
   {
      None,
      All,
      Initials
   }

   public class Classification
   {
      public string Key { get; set; }
      public string Name { get; set; } = string.Empty;
      public string Token { get; set; } = string.Empty;
      public CustomStyle Style { get; set; }

      //Todo Capitalization option is obsolete
      public CapitalizationType Capitalization { get; set; }

      //Todo Style Application options is obsolete
      public StyleApplication Application { get; set; }

      public bool RaiseWarning { get; set; }

      public Classification()
      {
         Key = System.Guid.NewGuid().ToString();
      }
   }
}
