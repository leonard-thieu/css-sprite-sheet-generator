﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace CssSpriteSheetGenerator.Models
{
    /// <summary>
    /// Represents a sprite on a sprite sheet.
    /// </summary>
    [DataContract]
    public sealed class Sprite : SpriteBase, ISprite
    {
        private WeakReference _Parent;
        /// <summary>
        /// The parent that this object is contained in.
        /// </summary>
        public SpriteSheet Parent
        {
            get { return _Parent.Target as SpriteSheet; }
            set { _Parent = new WeakReference(value); }
        }

        /// <summary>
        /// Identifies the <see cref="Width" /> property.
        /// </summary>
        public const string WidthPropertyName = "Width";
        private int _Width;
        /// <summary>
        /// The width of the sprite. Represents the value for the CSS property width.
        /// </summary>
        [DataMember(Order = 0)]
        public int Width
        {
            get { return _Width; }
            set
            {
                if (_Width == value)
                    return;
                _Width = value;
                OnPropertyChanged(WidthPropertyName);
            }
        }

        /// <summary>
        /// Identifies the <see cref="Height" /> property.
        /// </summary>
        public const string HeightPropertyName = "Height";
        private int _Height;
        /// <summary>
        /// The height of the sprite. Represents the value for the CSS property height.
        /// </summary>
        [DataMember(Order = 1)]
        public int Height
        {
            get { return _Height; }
            set
            {
                if (_Height == value)
                    return;
                _Height = value;
                OnPropertyChanged(HeightPropertyName);
            }
        }

        /// <summary>
        /// Initializes an instance of the <see cref="Sprite" /> class.
        /// </summary>
        /// <param name="className">The name to use for the CSS class.</param>
        /// <param name="x">The x-coordinate of the sprite.</param>
        /// <param name="y">The y-coordinate of the sprite.</param>
        /// <param name="width">The width in pixels of the sprite.</param>
        /// <param name="height">The height in pixels of the sprite.</param>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "x")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "y")]
        public Sprite(string className, int x, int y, int width, int height)
        {
            ClassName = className;
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }
}
