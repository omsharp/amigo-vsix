using Configurations.Core.Comments;
using Configurations.UI.Dialogs;
using Mvvm.Core;
using Mvvm.Core.Command;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Media;

namespace Configurations.UI.ViewModels.Comments
{
   public class StylesPageViewModel : ViewModelBase, IPageViewModel
   {
      public ICommentConfiguration Config { get; }
      public IDialogService DialogService { get; }

      public string Title => "Styles";

      public ObservableCollection<StyleViewModel> Styles { get; }
         = new ObservableCollection<StyleViewModel>();

      public StyleViewModel SelectedStyle { get; set; }

      public Color VSBackground => Config.Defaults.VSBackground;

      public List<string> Fonts { get; set; } = new List<string>();

      public RelayCommand AddCommand { get; }

      public StylesPageViewModel(ICommentConfiguration config, IDialogService dialogService)
      {
         Config = config;
         DialogService = dialogService;

         foreach (var style in Config.Styles)
            Styles.Add(new StyleViewModel(style, this));

         AddCommand = new RelayCommand(AddStyle);

         using (var fonts = new InstalledFontCollection())
         {
            foreach (var font in fonts.Families.OrderBy(f => f.Name))
               Fonts.Add(font.Name);
         }
      }

      private void AddStyle()
      {
         var style = new CustomStyle();
         var styleVm = new StyleViewModel(style, this);
         var dialogVm = new StyleNameDialogViewModel(styleVm, "New Style", DialogType.AddNew);

         var isOk = DialogService.OpenDialog(dialogVm);

         if (isOk)
         {
            Config.Styles.Add(style);
            Styles.Add(styleVm);
         }
      }

   }
}
