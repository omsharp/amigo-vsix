using Configurations.Core.Comments;
using ProtoBuf;

namespace Configurations.Store.Comments.DTO
{
   [ProtoContract]
   class DefaultStyleDTO
   {
      [ProtoMember(1)]
      public string Font { get; set; }

      [ProtoMember(2, DataFormat = DataFormat.FixedSize)]
      public int Foreground { get; set; }

      [ProtoMember(3, DataFormat = DataFormat.FixedSize)]
      public int Background { get; set; }

      [ProtoMember(4)]
      public bool UseVSBackground { get; set; }

      [ProtoMember(5, DataFormat = DataFormat.FixedSize)]
      public int VSBackground { get; set; }

      [ProtoMember(6)]
      public double Opacity { get; set; }

      [ProtoMember(7)]
      public double Size { get; set; }

      [ProtoMember(8)]
      public bool Italic { get; set; }

      [ProtoMember(9)]
      public bool Bold { get; set; }

      [ProtoMember(10)]
      public bool Underline { get; set; }

      [ProtoMember(11)]
      public bool Strikethrough { get; set; }

      public DefaultStyleDTO() { /*! Protobuff-net needs a parameterless ctor */ }

      public DefaultStyleDTO(DefaultStyle source)
      {
         Font = source.Font;
         Foreground = source.Foreground.ToInt();
         Background = source.OriginalBackground.ToInt();
         UseVSBackground = source.UseVSBackground;
         VSBackground = source.VSBackground.ToInt();
         Opacity = source.Opacity;
         Size = source.Size;
         Italic = source.Italic;
         Bold = source.Bold;
         Underline = source.Underline;
         Strikethrough = source.Strikethrough;
      }

      public void Set(DefaultStyle target)
      {
         target.Font = Font;
         target.Foreground = ColorEx.FromInt(Foreground);
         target.Background = ColorEx.FromInt(Background);
         target.VSBackground = ColorEx.FromInt(VSBackground);
         target.UseVSBackground = UseVSBackground;
         target.Opacity = Opacity;
         target.Size = Size;
         target.Italic = Italic;
         target.Bold = Bold;
         target.Underline = Underline;
         target.Strikethrough = Strikethrough;
      }
   }
}
