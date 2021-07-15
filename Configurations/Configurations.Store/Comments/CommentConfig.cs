using System;
using System.Collections.ObjectModel;
using ProtoBuf;
using System.IO;
using Configurations.Core.Comments;
using Configurations.Store.Comments.DTO;

namespace Configurations.Store.Comments
{
   public class CommentConfig : ICommentConfiguration
   {
      public DefaultStyle Defaults => DefaultStyle.Instance;

      public ObservableCollection<CustomStyle> Styles { get; private set; }

      public ObservableCollection<Classification> Classifications { get; private set; }

      public event EventHandler ChangesSaved;

      internal CommentConfig()
      {
         Styles = new ObservableCollection<CustomStyle>();
         Classifications = new ObservableCollection<Classification>();
      }

      public void Load(string filePath)
      {
         if (File.Exists(filePath))
         {
            CommentConfigDTO configDto;

            using (var file = File.OpenRead(filePath))
               configDto = Serializer.Deserialize<CommentConfigDTO>(file);

            configDto.Set(this);
         }
      }

      public void SaveChanges(string filePath)
      {
         using (var file = File.Create(filePath))
            Serializer.Serialize(file, new CommentConfigDTO(this));

         ChangesSaved?.Invoke(this, EventArgs.Empty);
      }
   }
}
