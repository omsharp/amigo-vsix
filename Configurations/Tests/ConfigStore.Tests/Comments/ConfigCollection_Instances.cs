using Configurations.Store;
using Xunit;

namespace ConfigStore.Tests.Comments
{
   public class ConfigCollection_Instances
   {
      [Fact(DisplayName = "Single CommentConfiguration instance")]
      public void Test()
      {
         var c1 = new ConfigCollection();
         var cc = c1.CommentConfiguration;

         var c2 = new ConfigCollection();
         var c3 = new ConfigCollection();

         Assert.Same(cc, c2.CommentConfiguration);
         Assert.Same(cc, c3.CommentConfiguration);
      }

   }
}
