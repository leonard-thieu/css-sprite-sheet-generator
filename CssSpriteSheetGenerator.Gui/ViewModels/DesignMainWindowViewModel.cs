using System.Diagnostics.CodeAnalysis;
using GalaSoft.MvvmLight;

namespace CssSpriteSheetGenerator.Gui.ViewModels
{
#pragma warning disable 1591
    [ExcludeFromCodeCoverage]
    public sealed class DesignMainWindowViewModel : ViewModelBase, IMainWindowViewModel
    {
        public ISpriteSheetViewModel SpriteSheetViewModel { get; set; }

        public DesignMainWindowViewModel(ISpriteSheetViewModel spriteSheetViewModel)
        {
            SpriteSheetViewModel = spriteSheetViewModel;
        }
    }
#pragma warning restore 1591
}
