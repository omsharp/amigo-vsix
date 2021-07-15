using Configurations.Core.Comments;
using Configurations.UI.Dialogs;
using Configurations.UI.ViewModels.Dialogs;
using Mvvm.Core;
using Mvvm.Core.Command;
using PropertyChanged;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using CommentConfigs = Configurations.Core.Comments;

namespace Configurations.UI.ViewModels.Comments
{
   public class StyleViewModel : ViewModelBase
   {
      readonly CustomStyle style;

      public StylesPageViewModel Owner { get; }

      public string Name
      {
         get => style.Name;
         set => style.Name = value;
      }

      [AlsoNotifyFor(nameof(Font))]
      public bool UseDefaultFont { get => style.UseDefaultFont; set => style.UseDefaultFont = value; }
      public string Font
      {
         get => style.Font;
         set => style.Font = value;
      }

      [AlsoNotifyFor(nameof(Foreground))]
      public bool UseDefaultForeground { get => style.UseDefaultForeground; set => style.UseDefaultForeground = value; }
      public Color Foreground
      {
         get => style.Foreground;
         set => style.Foreground = value;
      }

      [AlsoNotifyFor(nameof(Background))]
      public bool UseDefaultBackground { get => style.UseDefaultBackground; set => style.UseDefaultBackground = value; }
      public Color Background
      {
         get => style.Background;
         set => style.Background = value;
      }

      [AlsoNotifyFor(nameof(Opacity))]
      public bool UseDefaultOpacity { get => style.UseDefaultOpacity; set => style.UseDefaultOpacity = value; }
      public double Opacity
      {
         get => style.Opacity;
         set => style.Opacity = value;
      }

      [AlsoNotifyFor(nameof(Size))]
      public bool UseDefaultSize { get => style.UseDefaultSize; set => style.UseDefaultSize = value; }
      public double Size
      {
         get => style.Size;
         set => style.Size = value;
      }

      [AlsoNotifyFor(nameof(Italic))]
      public bool UseDefaultItalic { get => style.UseDefaultItalic; set => style.UseDefaultItalic = value; }
      public bool Italic
      {
         get => style.Italic;
         set => style.Italic = value;
      }

      [AlsoNotifyFor(nameof(Bold))]
      public bool UseDefaultBold { get => style.UseDefaultBold; set => style.UseDefaultBold = value; }
      public bool Bold
      {
         get => style.Bold;
         set => style.Bold = value;
      }

      [AlsoNotifyFor(nameof(Underline))]
      public bool UseDefaultUnderline { get => style.UseDefaultUnderline; set => style.UseDefaultUnderline = value; }
      public bool Underline
      {
         get => style.Underline;
         set => style.Underline = value;
      }

      [AlsoNotifyFor(nameof(Strikethrough))]
      public bool UseDefaultStrikethrough { get => style.UseDefaultStrikethrough; set => style.UseDefaultStrikethrough = value; }
      public bool Strikethrough
      {
         get => style.Strikethrough;
         set => style.Strikethrough = value;
      }

      [DependsOn(nameof(Italic))]
      public FontStyle FontStyle => Italic ? FontStyles.Italic : FontStyles.Normal;

      [DependsOn(nameof(Bold))]
      public FontWeight FontWeight => Bold ? FontWeights.Bold : FontWeights.Normal;

      [DependsOn(nameof(Underline), nameof(Strikethrough))]
      public TextDecorationCollection TextDecoration
      {
         get
         {
            var decorations = new TextDecorationCollection();

            if (Underline)
               decorations.Add(TextDecorations.Underline);

            if (Strikethrough)
               decorations.Add(TextDecorations.Strikethrough);

            return decorations;
         }
      }


      public RelayCommand DeleteCommand { get; }
      public RelayCommand RenameCommand { get; }

      public StyleViewModel(CommentConfigs.CustomStyle source, StylesPageViewModel owner)
      {
         style = source;
         Owner = owner;

         // Commands
         DeleteCommand = new RelayCommand(Remove);
         RenameCommand = new RelayCommand(Rename);
      }

      private void Rename()
      {
         var oldName = Name;
         var dialogVm = new StyleNameDialogViewModel(this, $"Rename Style [{Name}]", DialogType.Edit);
         var isOk = Owner.DialogService.OpenDialog(dialogVm);

         if (!isOk)
            Name = oldName;
      }

      private void Remove()
      {
         var q = Owner.Config.Classifications.Where(c => c.Style.Name == Name);
         if (q.Any())
         {
            var msg = $"This style ({Name}) is attached to a classification ({q.First().Name})!\n\n"
                     + "You can't delete an attached style.";

            var infoVm = new NotificationDialogViewModel("Inappropriate Action", msg, NotificationType.Explanation);
            Owner.DialogService.OpenDialog(infoVm);

            return;
         }

         var message = $"The ({Name}) style will be permanently deleted.";
         var viewModel = new ConfirmationDialogViewModel("Delete", message);
         var isOk = Owner.DialogService.OpenDialog(viewModel);
         if (isOk)
         {
            Owner.Styles.Remove(this);
            Owner.Config.Styles.Remove(Owner.Config.Styles.Single(s => s.Name == Name));
         }
      }
   }
}
