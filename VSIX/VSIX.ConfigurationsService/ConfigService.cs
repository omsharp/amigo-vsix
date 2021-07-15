using Configurations.Core;
using Configurations.Store;
using System;

namespace VSIX.ConfigurationsService
{
   public class ConfigService
   {
      private static IConfigurationCollection configs;

      public static IConfigurationCollection Current
      {
         get
         {
            if (configs == null)
            {
               configs = new ConfigCollection($@"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\AmigoExt\configs");
               configs.LoadAll();
            }

            return configs;
         }
      }
   }
}
