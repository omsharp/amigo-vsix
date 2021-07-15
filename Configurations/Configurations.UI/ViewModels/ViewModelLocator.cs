using Mvvm.Core.Ioc;
using CommonServiceLocator;
using Configurations.UI.Dialogs;
using Configurations.UI.ViewModels.Comments;
using VSIX.ConfigurationsService;

namespace Configurations.UI.ViewModels
{
   public class ViewModelLocator
   {
      static readonly object locker = new object();
      static ViewModelLocator instance;

      public static ViewModelLocator Instance
      {
         get
         {
            if (instance == null)
            {
               lock (locker)
               {
                  if (instance == null)
                     instance = new ViewModelLocator();
               }
            }

            return instance;
         }
      }

      ////=> Application.Current.Resources["Locator"] as ViewModelLocator;

      public MainViewModel Main
         => ServiceLocator.Current.GetInstance<MainViewModel>();

      public DefaultsPageViewModel Defaults
         => ServiceLocator.Current.GetInstance<DefaultsPageViewModel>();

      public StylesPageViewModel Styles
         => ServiceLocator.Current.GetInstance<StylesPageViewModel>();

      public ClassificationsPageViewModel Classifications
         => ServiceLocator.Current.GetInstance<ClassificationsPageViewModel>();

      private ViewModelLocator()
      {
         ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

         ////if (ViewModelBase.IsInDesignModeStatic)
         ////{
         ////   //! Create design time view services and models
         ////}
         ////else
         {
            //! ViewModels
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<DefaultsPageViewModel>();
            SimpleIoc.Default.Register<StylesPageViewModel>();
            SimpleIoc.Default.Register<ClassificationsPageViewModel>();

            //! Interfaces factories
            SimpleIoc.Default.Register(() => ConfigService.Current);
            SimpleIoc.Default.Register(() => ConfigService.Current.CommentConfiguration);
            SimpleIoc.Default.Register<IDialogService>(() => new DialogService(MainWindow.CurrentInstance));


            //  MakeTestData(); // TODO Remove this method
         }
      }

      public static void Cleanup()
      {
         instance = null;
         SimpleIoc.Default.Reset();
      }


      // TODO Remove this method when done
      //public void MakeTestData()
      //{
      //   var note = new CommentConfigs.Style { Name = "Note" };
      //   note.Font = "Consolas";
      //   note.Foreground = Colors.Aqua;
      //   note.Background = Color.FromRgb(24, 24, 24);
      //   note.UseDefaultBackground = true;
      //   note.Opacity.UseDefault = true;
      //   note.Size.UseDefault = true;
      //   note.Italic = true;
      //   note.Bold = false;
      //   note.Underline = true;
      //   note.Strikethrough = false;
      //   Config.CommentConfiguration.Styles.Add(note);

      //   var task = new CommentConfigs.Style { Name = "Task" };
      //   task.Font = "Aller";
      //   task.Foreground = Colors.Yellow;
      //   task.Background = Colors.Gray;
      //   task.Background.UseDefault = false;
      //   task.Opacity.UseDefault = true;
      //   task.Size.UseDefault = true;
      //   task.Italic = true;
      //   task.Bold = true;
      //   task.Bold.UseDefault = true;
      //   task.Underline = true;
      //   task.Strikethrough = false;
      //   Config.CommentConfiguration.Styles.Add(task);

      //   var bug = new CommentConfigs.Style { Name = "Bug" };
      //   bug.Font = "Days";
      //   bug.Font.UseDefault = true;
      //   bug.Foreground = Color.FromRgb(227, 45, 32);
      //   bug.Background.UseDefault = false;
      //   bug.Opacity.UseDefault = true;
      //   bug.Size.UseDefault = true;
      //   bug.Italic = false;
      //   bug.Bold = true;
      //   bug.Underline = true;
      //   bug.Strikethrough = false;
      //   Config.CommentConfiguration.Styles.Add(bug);

      //   var allDefault = new CommentConfigs.Style { Name = "All-Default" };
      //   allDefault.Font.UseDefault = true;
      //   allDefault.Foreground.UseDefault = true;
      //   allDefault.Background.UseDefault = true;
      //   allDefault.Opacity.UseDefault = true;
      //   allDefault.Size.UseDefault = true;
      //   allDefault.Italic.UseDefault = true;
      //   allDefault.Bold.UseDefault = true;
      //   allDefault.Underline.UseDefault = true;
      //   allDefault.Strikethrough.UseDefault = true;
      //   Config.CommentConfiguration.Styles.Add(allDefault);

      //   var allOn = new CommentConfigs.Style { Name = "All-On" };
      //   allOn.Font = "Calibri";
      //   allOn.Foreground = Colors.LimeGreen;
      //   allOn.Background = Colors.Purple;
      //   allOn.Opacity = 0.5;
      //   allOn.Size = 18;
      //   allOn.Italic = true;
      //   allOn.Bold = true;
      //   allOn.Underline = true;
      //   allOn.Strikethrough = true;
      //   Config.CommentConfiguration.Styles.Add(allOn);

      //   var full = new CommentConfigs.Style { Name = "JEKKDIJEHHGIDKDM" };
      //   full.Font = "Calibri";
      //   full.Foreground = Colors.LimeGreen;
      //   full.Background = Colors.Purple;
      //   full.Opacity = 0.5;
      //   full.Size = 18;
      //   full.Italic = true;
      //   full.Bold = true;
      //   full.Underline = true;
      //   full.Strikethrough = true;
      //   Config.CommentConfiguration.Styles.Add(full);


      //   //! Classifications
      //   Config.CommentConfiguration.Classifications.Add(
      //      new Classification
      //      {
      //         Name = "Note",
      //         Token = "!",
      //         Style = note,
      //         Application = StyleApplication.BodyOnly,
      //         Capitalization = CapitalizationType.Initials
      //      });

      //   Config.CommentConfiguration.Classifications.Add(
      //      new Classification
      //      {
      //         Name = "Bug",
      //         Token = "?",
      //         Style = bug,
      //         Application = StyleApplication.TokenOnly,
      //         Capitalization = CapitalizationType.All
      //      });

      //   Config.CommentConfiguration.Classifications.Add(
      //      new Classification
      //      {
      //         Name = "Task",
      //         Token = "TODO",
      //         Style = task,
      //         Application = StyleApplication.BodyOnly,
      //         Capitalization = CapitalizationType.None
      //      });

      //   Config.CommentConfiguration.Classifications.Add(
      //     new Classification
      //     {
      //        Name = "All-On",
      //        Token = "all",
      //        Style = allOn,
      //        Application = StyleApplication.All,
      //        Capitalization = CapitalizationType.All
      //     });

      //   Config.CommentConfiguration.Classifications.Add(
      //    new Classification
      //    {
      //       Name = "Final",
      //       Token = "fin",
      //       Style = bug,
      //       Application = StyleApplication.All,
      //       Capitalization = CapitalizationType.All
      //    });
      //}

   }
}