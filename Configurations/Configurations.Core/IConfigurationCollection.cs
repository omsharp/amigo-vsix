using Configurations.Core.Comments;

namespace Configurations.Core
{
   public interface IConfigurationCollection
   {
      ICommentConfiguration CommentConfiguration { get; }
      void SaveAllChanges();
      void LoadAll();
   }
}
