using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Data;
using CssSpriteSheetGenerator.Models;
using GalaSoft.MvvmLight.Command;

namespace CssSpriteSheetGenerator.Gui.ViewModels
{
    /// <summary>
    /// Defines the view model for the manipulation of a sprite sheet.
    /// </summary>
    public interface ISpriteSheetViewModel : INotifyPropertyChanged
    {
        #region Properties

        /// <summary>
        /// Indicates if there are any images on the sprite sheet.
        /// </summary>
        bool IsEmpty { get; set; }

        /// <summary>
        /// The file name of the sprite sheet.
        /// </summary>
        string FileName { get; set; }

        /// <summary>
        /// Indicates if the sprite sheet generator has been modified since it was last saved or loaded.
        /// </summary>
        bool IsModified { get; set; }

        /// <summary>
        /// The members of <see cref="Arrange" />.
        /// </summary>
        IEnumerable<Arrange> BuildMethods { get; }

        /// <summary>
        /// Specifies how items align on the <see cref="Controls.DropTargetCanvas" />.
        /// </summary>
        Arrange BuildMethod { get; set; }

        /// <summary>
        /// The number of pixels that separates each sprite on the horizontal axis.
        /// </summary>
        int HorizontalOffset { get; }

        /// <summary>
        /// The number of pixels that separates each sprite on the vertical axis.
        /// </summary>
        int VerticalOffset { get; }

        /// <summary>
        /// The main class used to manipulate a sprite sheet.
        /// </summary>
        SpriteSheetGenerator SpriteSheetGenerator { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether the Rectangle Select tool is enabled or disabled.
        /// </summary>
        bool IsRectangleSelectToolEnabled { get; set; }

        /// <summary>
        /// A flattened collection of all the sprite sheets and sprites.
        /// </summary>
        CompositeCollection Children { get; }

        #endregion

        #region Commands

        /// <summary>
        /// Creates a new sprite sheet generator.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "New")]
        RelayCommand New { get; }

        /// <summary>
        /// Loads a previously saved state of a sprite sheet.
        /// </summary>
        RelayCommand Import { get; }

        /// <summary>
        /// Requests the view to open a save dialog and saves the state representing the sprite sheet.
        /// </summary>
        RelayCommand Export { get; }

        /// <summary>
        /// Requests the view to open a save dialog and saves the image representing the sprite sheet.
        /// </summary>
        RelayCommand SaveImage { get; }

        /// <summary>
        /// Requests the view to open a save dialog and saves the CSS representing the sprite sheet.
        /// </summary>
        RelayCommand SaveCss { get; }

        /// <summary>
        /// Enables or disables the Rectangle Select tool.
        /// </summary>
        RelayCommand ToggleRectangleSelectTool { get; }

        /// <summary>
        /// Performs a cut operation on the selected objects.
        /// </summary>
        RelayCommand Cut { get; }

        /// <summary>
        /// Builds the sprite sheet.
        /// </summary>
        RelayCommand Build { get; }

        /// <summary>
        /// Causes the debugger to break.
        /// </summary>
        RelayCommand DebugCommand { get; }

        /// <summary>
        /// Adds a sample image.
        /// </summary>
        RelayCommand AddSampleImageCommand { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a sprite sheet.
        /// </summary>
        /// <param name="fileName">The filename of the image to use as the background for this sprite sheet.</param>
        /// <param name="position">The position where the sprite sheet is to be added.</param>
        SpriteSheet AddSpriteSheet(string fileName, Point position);

        /// <summary>
        /// Removes a sprite sheet.
        /// </summary>
        /// <param name="className">The CSS class assigned to this sprite sheet.</param>
        void RemoveSpriteSheet(string className);

        /// <summary>
        /// Adds a sprite.
        /// </summary>
        /// <param name="className">The CSS class assigned to this sprite.</param>
        /// <param name="bounds">The location and bounds for this sprite.</param>
        void AddSprite(string className, Rect bounds);

        /// <summary>
        /// Removes a sprite.
        /// </summary>
        /// <param name="className">The CSS class assigned to this sprite.</param>
        void RemoveSprite(string className);

        #endregion
    }
}
