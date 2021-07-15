using Microsoft.VisualStudio.Text.Classification;
using System;
using VSIX.ConfigurationsService;

namespace VSIX.Package.Utils
{
   public static class ClassificationTypeRegistryServiceExtensions
   {
      public static void SyncWithConfigs(this IClassificationTypeRegistryService service)
      {
         foreach (var cls in ConfigService.Current.CommentConfiguration.Classifications)
         {
            if (service.GetClassificationType(cls.Key) == null)
               service.CreateClassificationType(cls.Key, Array.Empty<IClassificationType>());
         }
      }

   }
}
