namespace CssSpriteSheetGenerator.Models
{
    /// <summary>
    /// Represents a sprite on a sprite sheet.
    /// </summary>
    public interface ISprite : ISpriteBase
    {
        /// <summary>
        /// The parent that this object is contained in.
        /// </summary>
        SpriteSheet Parent { get; set; }

        /// <summary>
        /// The height of the sprite. Represents the value for the CSS property height.
        /// </summary>
        int Height { get; set; }

        /// <summary>
        /// The width of the sprite. Represents the value for the CSS property width.
        /// </summary>
        int Width { get; set; }
    }
}
