namespace VSIX.Package.Comments.UI
{
   using System.Windows;
   using System.Windows.Controls;
   using System.Windows.Input;
   using VSIX.ConfigurationsService;
   using VSIX.Package.Comments.UI.ViewModels;

    public partial class CommentsListToolWindowControl : UserControl
   {
      public CommentsListToolWindowControl()
      {
         InitializeComponent();

         DataContext = ViewModelLocator.Instance.BookmarksListViewModel; //new BookmarksPaneViewModel(ConfigService.Current.CommentConfiguration);
      }

      private void InnerListView_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
      {
         // Bubbles the mouse wheel event from inner lists to the main list view.
         if (!e.Handled)
         {
            e.Handled = true;
            var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
            {
               RoutedEvent = MouseWheelEvent,
               Source = sender
            };

            (((Control)sender).Parent as UIElement)?.RaiseEvent(eventArg);
         }
      }
   }
}