using System.ComponentModel;

namespace CssSpriteSheetGenerator.Gui.ViewModels
{
    /// <summary>
    /// Represents the view model used by MainWindow.xaml.
    /// </summary>
    public interface IMainWindowViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The child view model that handles interaction with <see cref="CssSpriteSheetGenerator.Models.SpriteSheetGenerator" />.
        /// </summary>
        ISpriteSheetViewModel SpriteSheetViewModel { get; set; }
    }
}
