using System;
using System.Collections.ObjectModel;
using Mapper;

namespace CssSpriteSheetGenerator.Models
{
    /// <summary>
    /// Contains the data for and the results from processing for an <see cref="IMapper{T}" />.
    /// </summary>
    public class SpriteMapper : Mapper.ISprite
    {
        private Collection<IMappedImageInfo> _MappedImages;

        /// <summary>
        /// Holds the locations of all the individual images (treated as rectangles) within 
        /// the sprite sheet (treated as the enclosing rectangle).
        /// </summary>
        public Collection<IMappedImageInfo> MappedImages
        {
            get { return _MappedImages ?? (_MappedImages = new Collection<IMappedImageInfo>()); }
        }

        /// <summary>
        /// The width in pixels of the sprite sheet.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// The height in pixels of the sprite sheet.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// The area of the sprite sheet.
        /// </summary>
        public int Area { get { return Width * Height; } }

        /// <summary>
        /// Adds an image to <see cref="MappedImages" />, and updates <see cref="Width" /> 
        /// and <see cref="Height" />.
        /// </summary>
        /// <param name="mappedImage">The image to add.</param>
        /// <exception cref="ArgumentNullException"><paramref name="mappedImage" /> cannot be null.</exception>
        public void AddMappedImage(IMappedImageInfo mappedImage)
        {
            if (mappedImage == null)
                throw new ArgumentNullException("mappedImage");

            MappedImages.Add(mappedImage);

            var newImage = mappedImage.ImageInfo;

            var maxX = mappedImage.X + newImage.Width;
            var maxY = mappedImage.Y + newImage.Height;

            if (maxX > Width)
                Width = maxX;
            if (maxY > Height)
                Height = maxY;

            var spriteSheet = mappedImage.ImageInfo as SpriteSheet;
            if (spriteSheet != null)
            {
                spriteSheet.X = mappedImage.X;
                spriteSheet.Y = mappedImage.Y;
            }
        }
    }
}
