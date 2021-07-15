using Configurations.Store;
using Configurations.Core.Comments;
using System.IO;
using Xunit;

namespace ConfigStore.Tests.Comments
{
   public class ConfigStore_Deserialize
   {
      [Fact(DisplayName = "Keep Style Reference in Classification")]
      public void ConfigStore()
      {
         var configDir = @"config";
         var config = new ConfigCollection(configDir);
         config.CommentConfiguration.Styles.Add(new CustomStyle { Name = "Style-1" });
         config.CommentConfiguration.Classifications.Add(
            new Classification
            {
               Name = "Csl-1",
               Token = "c",
               Style = config.CommentConfiguration.Styles[0]
            });

         if (Directory.Exists(configDir))
            foreach (var file in Directory.GetFiles(configDir))
               File.Delete(file);

         config.SaveAllChanges();
         config.LoadAll();

         Assert.Same(
            config.CommentConfiguration.Styles[0],
            config.CommentConfiguration.Classifications[0].Style);
      }
   }
}
