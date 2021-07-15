using System.Collections.ObjectModel;
using System.Windows.Media;

namespace Configurations.Core.Comments
{
   public interface ICommentConfiguration : IConfiguration
   {
      DefaultStyle Defaults { get; }

      ObservableCollection<Classification> Classifications { get; }

      ObservableCollection<CustomStyle> Styles { get; }
   }
}
