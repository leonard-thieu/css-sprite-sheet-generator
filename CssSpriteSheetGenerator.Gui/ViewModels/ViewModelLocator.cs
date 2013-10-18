using System.Diagnostics.CodeAnalysis;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace CssSpriteSheetGenerator.Gui.ViewModels
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                if (!SimpleIoc.Default.IsRegistered<IMainWindowViewModel>())
                    SimpleIoc.Default.Register<IMainWindowViewModel, DesignMainWindowViewModel>();
                if (!SimpleIoc.Default.IsRegistered<ISpriteSheetViewModel>())
                    SimpleIoc.Default.Register<ISpriteSheetViewModel, DesignSpriteSheetViewModel>();
            }
            else
            {
                SimpleIoc.Default.Register<IMainWindowViewModel, MainWindowViewModel>();
                SimpleIoc.Default.Register<ISpriteSheetViewModel, SpriteSheetViewModel>();
            }
        }

        /// <summary>
        /// Returns a <see cref="IMainWindowViewModel" /> instance.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public IMainWindowViewModel MainWindowViewModel { get { return ServiceLocator.Current.GetInstance<IMainWindowViewModel>(); } }

        /// <summary>
        /// Returns a <see cref="ISpriteSheetViewModel" /> instance.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public ISpriteSheetViewModel SpriteSheetViewModel { get { return ServiceLocator.Current.GetInstance<ISpriteSheetViewModel>(); } }
    }
}