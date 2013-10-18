using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using Mapper;

namespace CssSpriteSheetGenerator.Models
{
    /// <summary>
    /// The main container class for generating a sprite sheet. Creates the final,
    /// composite image and the CSS rules of the sprite sheet. This class can also
    /// serialize the sprite sheet model to disk for later modification or reuse.
    /// </summary>
    public interface ISpriteSheetGenerator : IDisposable, INotifyPropertyChanged, IExtensibleDataObject
    {
        /// <summary>
        /// Adds a sprite to a sprite sheet.
        /// </summary>
        /// <param name="parent">The sprite sheet to add the sprite to.</param>
        /// <param name="sprite">The sprite to be added.</param>
        /// <returns>The sprite that was added.</returns>
        Sprite AddSprite(SpriteSheet parent, Sprite sprite);

        /// <summary>
        /// Adds a sprite to a sprite sheet.
        /// </summary>
        /// <param name="parent">The sprite sheet to add the sprite to.</param>
        /// <param name="className">The class name of the sprite.</param>
        /// <param name="x">The x-coordinate of the sprite.</param>
        /// <param name="y">The y-coordinate of the sprite.</param>
        /// <param name="width">The width of the sprite.</param>
        /// <param name="height">The height of the sprite.</param>
        /// <returns>The sprite that was added to <paramref name="parent" />.</returns>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "y"), SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "x")]
        Sprite AddSprite(SpriteSheet parent, string className, int x, int y, int width, int height);

        /// <summary>
        /// Adds a sprite to a sprite sheet.
        /// </summary>
        /// <param name="className">The class name of the sprite.</param>
        /// <param name="bounds">The bounds of the sprite.</param>
        /// <returns>The sprite that was added to a child sprite sheet.</returns>
        Sprite AddSprite(string className, Rectangle bounds);

        /// <summary>
        /// Adds a sprite sheet to this container sprite sheet.
        /// </summary>
        /// <param name="spriteSheet">The sprite sheet to add.</param>
        /// <returns>The sprite sheet that was added to this container sprite sheet.</returns>
        SpriteSheet AddSpriteSheet(SpriteSheet spriteSheet);

        /// <summary>
        /// Adds a sprite sheet to this container sprite sheet.
        /// </summary>
        /// <param name="bitmap">The background image.</param>
        /// <param name="point">The point that the sprite sheet is located at.</param>
        /// <returns>The sprite sheet added to this container sprite sheet.</returns>
        SpriteSheet AddSpriteSheet(Bitmap bitmap, Point point);

        /// <summary>
        /// Adds a sprite sheet to this container sprite sheet.
        /// </summary>
        /// <param name="bitmap">The background image.</param>
        /// <param name="x">The x-coordinate of the sprite sheet.</param>
        /// <param name="y">The y-coordinate of the sprite sheet.</param>
        /// <returns>The sprite sheet added to this container sprite sheet.</returns>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "x")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "y")]
        SpriteSheet AddSpriteSheet(Bitmap bitmap, int x, int y);

        /// <summary>
        /// Adds a sprite sheet to this container sprite sheet.
        /// </summary>
        /// <param name="fileName">The path to the image that will serve as the background image.</param>
        /// <param name="point">The point that the sprite sheet is located at.</param>
        /// <returns>The sprite sheet added to this container sprite sheet.</returns>
        SpriteSheet AddSpriteSheet(string fileName, Point point);

        /// <summary>
        /// Adds a sprite sheet to this container sprite sheet.
        /// </summary>
        /// <param name="fileName">The path to the image that will serve as the background image.</param>
        /// <param name="x">The x-coordinate of the sprite sheet.</param>
        /// <param name="y">The y-coordinate of the sprite sheet.</param>
        /// <returns>The sprite sheet added to this container sprite sheet.</returns>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "x")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "y")]
        SpriteSheet AddSpriteSheet(string fileName, int x, int y);

        /// <summary>
        /// Triggers an arrange.
        /// </summary>
        void Build();

        /// <summary>
        /// Specifies how sprites are arranged.
        /// </summary>
        Arrange BuildMethod { get; set; }

        /// <summary>
        /// The CSS output of the sprite sheet.
        /// </summary>
        string Css { get; set; }

        /// <summary>
        /// Returns the <see cref="Sprite" /> or <see cref="SpriteSheet" /> that matches <paramref name="className" />.
        /// </summary>
        /// <param name="className">The class name to search for.</param>
        /// <returns>
        /// The <see cref="Sprite" /> or <see cref="SpriteSheet" /> that matches <paramref name="className" />.
        /// </returns>
        T Find<T>(string className) where T : SpriteBase;

        /// <summary>
        /// The number of pixels that separates each sprite on the horizontal axis.
        /// </summary>
        int HorizontalOffset { get; set; }

        /// <summary>
        /// The sprite mapper.
        /// </summary>
        IMapper<SpriteMapper> Mapper { get; set; }

        /// <summary>
        /// Removes a sprite from a sprite sheet.
        /// </summary>
        /// <param name="parent">The sprite sheet to remove the sprite from.</param>
        /// <param name="sprite">The sprite to remove.</param>
        void RemoveSprite(SpriteSheet parent, Sprite sprite);

        /// <summary>
        /// Removes a sprite from a sprite sheet.
        /// </summary>
        /// <param name="className">The class name of the sprite to remove.</param>
        void RemoveSprite(string className);

        /// <summary>
        /// Removes a sprite sheet from this container sprite sheet.
        /// </summary>
        /// <param name="className">The CSS class that represents the sprite sheet.</param>
        void RemoveSpriteSheet(string className);

        /// <summary>
        /// Serializes the sprite sheet model to disk.
        /// </summary>
        /// <param name="stream">The stream to serialize to.</param>
        void Save(Stream stream);

        /// <summary>
        /// Serializes the sprite sheet model to disk.
        /// </summary>
        /// <param name="fileName">The file name to save the sprite sheet model as.</param>
        void Save(string fileName);

        /// <summary>
        /// Saves the CSS of this sprite sheet.
        /// </summary>
        /// <param name="fileName">The file name to save the CSS as.</param>
        void SaveCss(string fileName);

        /// <summary>
        /// Saves the composite background image of this sprite sheet.
        /// </summary>
        /// <param name="fileStream">The file stream to save the image to.</param>
        void SaveImage(Stream fileStream);

        /// <summary>
        /// Saves the composite background image of this sprite sheet.
        /// </summary>
        /// <param name="fileName">The file name to save the image as.</param>
        void SaveImage(string fileName);

        /// <summary>
        /// The flattened collection of all the sprites in <see cref="SpriteSheets" />.
        /// </summary>
        ObservableCollection<Sprite> Sprites { get; }

        /// <summary>
        /// The child sprite sheets of this container sprite sheet.
        /// </summary>
        ObservableCollection<SpriteSheet> SpriteSheets { get; }

        /// <summary>
        /// The number of pixels that separates each sprite on the vertical axis.
        /// </summary>
        int VerticalOffset { get; set; }
    }
}
