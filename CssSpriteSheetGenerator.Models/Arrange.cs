namespace CssSpriteSheetGenerator.Models
{
    /// <summary>
    /// Determines how sprites should be arranged on a sprite sheet.
    /// </summary>
    public enum Arrange
    {
        /// <summary>
        /// Sprites are arranged horizontally.
        /// </summary>
        Horizontal,
        /// <summary>
        /// Sprites are arranged vertically.
        /// </summary>
        Vertical,
        /// <summary>
        /// Sprites are arranged optimally using a rectangle packer algorithm.
        /// </summary>
        Optimal
    }
}
