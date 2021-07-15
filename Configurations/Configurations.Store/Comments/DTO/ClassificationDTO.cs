using Configurations.Core.Comments;
using ProtoBuf;
using System;

namespace Configurations.Store.Comments.DTO
{
   [ProtoContract]
   class ClassificationDTO
   {
      [ProtoMember(1)]
      public string Name { get; set; } = string.Empty;

      [ProtoMember(2)]
      public string Token { get; set; } = string.Empty;

      [ProtoMember(3)]
      public string Style { get; set; }

      [ProtoMember(4)]
      public CapitalizationType Capitalization { get; set; }

      [ProtoMember(5)]
      public StyleApplication Application { get; set; }

      [ProtoMember(6)]
      public string Key { get; set; }

      [ProtoMember(7)]
      public bool RaiseWarning { get; set; }

      public ClassificationDTO()
      { }

      public ClassificationDTO(Classification source)
      {
         Key = source.Key;
         Name = source.Name;
         Token = source.Token;
         Style = source.Style.Name;
         Capitalization = source.Capitalization;
         Application = source.Application;
         RaiseWarning = source.RaiseWarning;
      }
   }
}
