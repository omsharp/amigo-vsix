using System.Collections.Generic;
using System.Linq;
using Configurations.Core.Comments;
using Configurations.Store.Comments;
using ProtoBuf;

namespace Configurations.Store.Comments.DTO
{
   [ProtoContract]
   class CommentConfigDTO
   {
      [ProtoMember(1)]
      public DefaultStyleDTO DefaultsDto { get; set; }

      [ProtoMember(2)]
      public List<StyleDTO> StyleDtos { get; set; } = new List<StyleDTO>();

      [ProtoMember(3)]
      public List<ClassificationDTO> ClassificationDtos { get; set; } = new List<ClassificationDTO>();

      public CommentConfigDTO()
      { }

      public CommentConfigDTO(CommentConfig source)
      {
         DefaultsDto = new DefaultStyleDTO(source.Defaults);

         foreach (var style in source.Styles)
            StyleDtos.Add(new StyleDTO(style));

         foreach (var cls in source.Classifications)
            ClassificationDtos.Add(new ClassificationDTO(cls));
      }

      public void Set(CommentConfig target)
      {
         target.Styles.Clear();
         target.Classifications.Clear();

         DefaultsDto.Set(target.Defaults);

         foreach (var styleDto in StyleDtos)
            target.Styles.Add(styleDto.ToStyle());

         foreach (var clsDto in ClassificationDtos)
         {
            var style = target.Styles.SingleOrDefault(s => s.Name == clsDto.Style);

            target.Classifications.Add(
                  new Classification
                  {
                     Key = clsDto.Key,
                     Name = clsDto.Name,
                     Token = clsDto.Token,
                     Style = style,
                     Capitalization = clsDto.Capitalization,
                     Application = clsDto.Application,
                     RaiseWarning = clsDto.RaiseWarning
                  });
         }
      }
   }
}
