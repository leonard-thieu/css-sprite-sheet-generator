using System;
using System.Collections.ObjectModel;
using System.Drawing;
using Mapper;

namespace CssSpriteSheetGenerator.Models
{
    /// <summary>
    /// Represents the background image used as the sprite sheet and the sprites on the sprite sheet.
    /// </summary>
    public interface ISpriteSheet : ISpriteBase, IImageInfo, IDisposable
    {
        /// <summary>
        /// The image that serves as the sprite sheet.
        /// </summary>
        Bitmap Image { get; }

        /// <summary>
        /// The path to <see cref="Image" />.
        /// </summary>
        string ImageFile { get; set; }

        /// <summary>
        /// The sprites contained within this sprite sheet.
        /// </summary>
        ObservableCollection<Sprite> Sprites { get; }

        /// <summary>
        /// Indicates (for certain display scenarios) if this sprite sheet is expanded.
        /// </summary>
        bool IsExpanded { get; set; }

        /// <summary>
        /// The bounds of this element.
        /// </summary>
        Rectangle Bounds { get; }
    }
}
