using System;

namespace Configurations.Core
{
   public interface IConfiguration
   {
      void SaveChanges(string path);
      void Load(string path);

      event EventHandler ChangesSaved;
   }
}
