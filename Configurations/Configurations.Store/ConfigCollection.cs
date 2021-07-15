using Configurations.Core;
using Configurations.Core.Comments;
using System.IO;
using Configurations.Store.Comments;

namespace Configurations.Store
{
   public class ConfigCollection : IConfigurationCollection
   {
      readonly string commentsConfigPath;
      readonly static ICommentConfiguration commentConfig
         = new CommentConfig();

      public string ConfigDirectory { get; }

      public ICommentConfiguration CommentConfiguration
         => commentConfig;

      public ConfigCollection()
         : this("config")
      { }

      public ConfigCollection(string configDirectory)
      {
         ConfigDirectory = configDirectory;

         commentsConfigPath = $@"{ConfigDirectory}\comments.bin";
      }

      public void SaveAllChanges()
      {
         // Create the folder if it's not existing
         Directory.CreateDirectory(ConfigDirectory);

         commentConfig.SaveChanges(commentsConfigPath);
      }

      public void LoadAll()
      {
         commentConfig.Load(commentsConfigPath);
      }
   }
}
