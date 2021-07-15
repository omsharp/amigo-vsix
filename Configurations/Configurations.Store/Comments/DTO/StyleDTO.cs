using Configurations.Core.Comments;
using ProtoBuf;
using System.Windows.Media;

namespace Configurations.Store.Comments.DTO
{
   [ProtoContract]
   class StyleDTO
   {
      // Original properties
      [ProtoMember(1)]
      public string Name { get; set; }

      [ProtoMember(2)]
      public string Font { get; set; }

      [ProtoMember(3, DataFormat = DataFormat.FixedSize)]
      public int Foreground { get; set; }

      [ProtoMember(4, DataFormat = DataFormat.FixedSize)]
      public int Background { get; set; }

      [ProtoMember(5)]
      public double Opacity { get; set; }

      [ProtoMember(6)]
      public double Size { get; set; }

      [ProtoMember(7)]
      public bool Italic { get; set; }

      [ProtoMember(8)]
      public bool Bold { get; set; }

      [ProtoMember(9)]
      public bool Underline { get; set; }

      [ProtoMember(10)]
      public bool Strikethrough { get; set; }

      // Use default flags
      [ProtoMember(11)]
      public bool DefFont { get; set; }

      [ProtoMember(12)]
      public bool DefForeground { get; set; }

      [ProtoMember(13)]
      public bool DefBackground { get; set; }

      [ProtoMember(14)]
      public bool DefOpacity { get; set; }

      [ProtoMember(15)]
      public bool DefSize { get; set; }

      [ProtoMember(16)]
      public bool DefItalic { get; set; }

      [ProtoMember(17)]
      public bool DefBold { get; set; }

      [ProtoMember(18)]
      public bool DefUnderline { get; set; }

      [ProtoMember(19)]
      public bool DefStrikethrough { get; set; }


      public StyleDTO()
      { }

      public StyleDTO(CustomStyle source)
      {
         Name = source.Name;

         Font = source.OriginalFont;
         DefFont = source.UseDefaultFont;

         Foreground = source.OriginalForeground.ToInt();
         DefForeground = source.UseDefaultForeground;

         Background = source.OriginalBackground.ToInt();
         DefBackground = source.UseDefaultBackground;

         Opacity = source.OriginalOpacity;
         DefOpacity = source.UseDefaultOpacity;

         Size = source.OriginalSize;
         DefSize = source.UseDefaultSize;

         Italic = source.OriginalItalic;
         DefItalic = source.UseDefaultItalic;

         Bold = source.OriginalBold;
         DefBold = source.UseDefaultBold;

         Underline = source.OriginalUnderline;
         DefUnderline = source.UseDefaultUnderline;

         Strikethrough = source.OriginalStrikethrough;
         DefStrikethrough = source.UseDefaultStrikethrough;
      }

      public CustomStyle ToStyle()
      {
         return new CustomStyle
         {
            Name = Name,

            Font = Font,
            UseDefaultFont = DefFont,

            Foreground = ColorEx.FromInt(Foreground),
            UseDefaultForeground = DefForeground,

            Background = ColorEx.FromInt(Background),
            UseDefaultBackground = DefBackground,

            Opacity = Opacity,
            UseDefaultOpacity = DefOpacity,

            Size = Size,
            UseDefaultSize = DefSize,

            Italic = Italic,
            UseDefaultItalic = DefItalic,

            Bold = Bold,
            UseDefaultBold = DefBold,

            Underline = Underline,
            UseDefaultUnderline = DefUnderline,

            Strikethrough = Strikethrough,
            UseDefaultStrikethrough = DefStrikethrough,
         };
      }
   }
}
