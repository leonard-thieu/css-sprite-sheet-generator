﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Runtime.Serialization;

namespace CssSpriteSheetGenerator.Models
{
    /// <summary>
    /// Represents the background image used as the sprite sheet and the sprites on the sprite sheet.
    /// </summary>
    [DataContract]
    public sealed class SpriteSheet : SpriteBase, ISpriteSheet
    {
        private ObservableCollection<Sprite> _Sprites;

        /// <summary>
        /// The image that serves as the sprite sheet.
        /// </summary>
        public Bitmap Image { get; private set; }

        /// <summary>
        /// Identifies the <see cref="ImageFile" /> property.
        /// </summary>
        public const string ImageFilePropertyName = "ImageFile";
        private string _ImageFile;
        /// <summary>
        /// The path to <see cref="Image" />.
        /// </summary>
        /// <exception cref="ArgumentException">The path is not of a legal form.</exception>
        [DataMember(Order = 0)]
        public string ImageFile
        {
            get { return _ImageFile; }
            set
            {
                _ImageFile = value;
                OnPropertyChanged(ImageFilePropertyName);
            }
        }

        /// <summary>
        /// The sprites contained within this sprite sheet.
        /// </summary>
        [DataMember(Order = 1)]
        public ObservableCollection<Sprite> Sprites
        {
            get { return _Sprites ?? (_Sprites = new SpriteCollection(this)); }
        }

        /// <summary>
        /// The width of the sprite sheet.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// The height of the sprite sheet.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Identifies the <see cref="IsExpanded" /> property.
        /// </summary>
        public const string IsExpandedPropertyName = "IsExpanded";
        private bool _IsExpanded;
        /// <summary>
        /// Indicates (for certain display scenarios) if this sprite sheet is expanded.
        /// </summary>
        public bool IsExpanded
        {
            get { return _IsExpanded; }
            set
            {
                if (_IsExpanded == value)
                    return;
                _IsExpanded = value;
                OnPropertyChanged(IsExpandedPropertyName);
            }
        }

        /// <summary>
        /// The bounds of this element.
        /// </summary>
        public Rectangle Bounds { get { return new Rectangle(X, Y, Image.Width, Image.Height); } }

        // Initializes defaults
        private SpriteSheet()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteSheet" /> class.
        /// </summary>
        /// <param name="fileName">The file name of the background image.</param>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public SpriteSheet(string fileName)
            : this()
        {
            ImageFile = fileName;
        }

        /// <summary>
        /// Initializes an instance of the <see cref="SpriteSheet" /> class.
        /// </summary>
        /// <param name="image">The background image.</param>
        public SpriteSheet(Bitmap image)
            : this()
        {
            ImageFile = null;
            if (image != null)
                Image = image;
        }

        /// <summary>
        /// Releases all resources used by this <see cref="SpriteSheet" />.
        /// </summary>
        public void Dispose()
        {
            if (Image != null)
                Image.Dispose();
        }

        // Runs before deserialization
        [OnDeserializing]
        private void OnDeserializing(StreamingContext context)
        {
            Initialize();
        }

        // Performs initialization common to normal construction and deserialization
        private void Initialize()
        {
            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == ImageFilePropertyName)
                    if (ImageFile != null)
                        Image = new Bitmap(ImageFile);
                    else
                        Image = new Bitmap(1, 1);
            };

            IsExpanded = true;
        }
    }
}
