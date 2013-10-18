﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using Mapper;

namespace CssSpriteSheetGenerator.Models
{
    /// <summary>
    /// The main container class for generating a sprite sheet. Creates the final,
    /// composite image and the CSS rules of the sprite sheet. This class can also
    /// serialize the sprite sheet model to disk for later modification or reuse.
    /// </summary>
    [DataContract]
    public class SpriteSheetGenerator : ISpriteSheetGenerator
    {
        private ObservableCollection<SpriteSheet> _SpriteSheets;
        private ObservableCollection<Sprite> _Sprites;
        private ClassNameKeyedCollection _ClassNames;
        private KeyedCollection<string, SpriteBase> ClassNames { get { return _ClassNames ?? (_ClassNames = new ClassNameKeyedCollection()); } }

        #region Properties

        /// <summary>
        /// A <see cref="System.Runtime.Serialization.ExtensionDataObject" /> that contains 
        /// data that is not recognized as belonging to the data contract.
        /// </summary>
        public ExtensionDataObject ExtensionData { get; set; }

        /// <summary>
        /// Identifies the <see cref="BuildMethod" /> property.
        /// </summary>
        public const string BuildMethodPropertyName = "BuildMethod";
        private Arrange _BuildMethod;
        /// <summary>
        /// Specifies how sprites are arranged.
        /// </summary>
        public Arrange BuildMethod
        {
            get { return _BuildMethod; }
            set
            {
                _BuildMethod = value;
                OnPropertyChanged(BuildMethodPropertyName);
            }
        }

        /// <summary>
        /// The number of pixels that separates each sprite on the horizontal axis.
        /// </summary>
        public int HorizontalOffset { get; set; }

        /// <summary>
        /// The number of pixels that separates each sprite on the vertical axis.
        /// </summary>
        public int VerticalOffset { get; set; }

        /// <summary>
        /// The child sprite sheets of this container sprite sheet.
        /// </summary>
        [DataMember(Order = 0)]
        public ObservableCollection<SpriteSheet> SpriteSheets
        {
            get { return _SpriteSheets ?? (_SpriteSheets = new ObservableCollection<SpriteSheet>()); }
        }

        /// <summary>
        /// The flattened collection of all the sprites in <see cref="SpriteSheets" />.
        /// </summary>
        public ObservableCollection<Sprite> Sprites
        {
            get { return _Sprites ?? (_Sprites = new ObservableCollection<Sprite>()); }
        }

        /// <summary>
        /// Identifies the <see cref="Mapper" /> property.
        /// </summary>
        public const string MapperPropertyName = "Mapper";
        private IMapper<SpriteMapper> _Mapper;
        /// <summary>
        /// The sprite mapper.
        /// </summary>
        public IMapper<SpriteMapper> Mapper
        {
            get
            {
                if (_Mapper == null)
                    UpdateMapper();
                return _Mapper;
            }
            set
            {
                _Mapper = value;
                OnPropertyChanged(MapperPropertyName);
            }
        }

        /// <summary>
        /// Identifies the <see cref="Css" /> property.
        /// </summary>
        public const string CssPropertyName = "Css";
        private string _Css;
        /// <summary>
        /// The CSS output of the sprite sheet.
        /// </summary>
        public string Css
        {
            get { return _Css; }
            set
            {
                if (_Css == value)
                    return;
                _Css = value;
                OnPropertyChanged(CssPropertyName);
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteSheetGenerator" /> class.
        /// </summary>
        public SpriteSheetGenerator()
        {
            Initialize();
        }

        #endregion

        #region Public Methods

        #region SpriteSheet Methods

        #region AddSpriteSheet Overloads

        /// <summary>
        /// Adds a sprite sheet to this container sprite sheet.
        /// </summary>
        /// <param name="fileName">The path to the image that will serve as the background image.</param>
        /// <param name="point">The point that the sprite sheet is located at.</param>
        /// <returns>The sprite sheet added to this container sprite sheet.</returns>
        public SpriteSheet AddSpriteSheet(string fileName, Point point)
        {
            return AddSpriteSheet(fileName, point.X, point.Y);
        }

        /// <summary>
        /// Adds a sprite sheet to this container sprite sheet.
        /// </summary>
        /// <param name="fileName">The path to the image that will serve as the background image.</param>
        /// <param name="x">The x-coordinate of the sprite sheet.</param>
        /// <param name="y">The y-coordinate of the sprite sheet.</param>
        /// <returns>The sprite sheet added to this container sprite sheet.</returns>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "x")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "y")]
        public SpriteSheet AddSpriteSheet(string fileName, int x, int y)
        {
            var spriteSheet = new SpriteSheet(fileName)
            {
                ClassName = Path.GetFileName(fileName),
                X = x,
                Y = y
            };

            return AddSpriteSheet(spriteSheet);
        }

        /// <summary>
        /// Adds a sprite sheet to this container sprite sheet.
        /// </summary>
        /// <param name="bitmap">The background image.</param>
        /// <param name="point">The point that the sprite sheet is located at.</param>
        /// <returns>The sprite sheet added to this container sprite sheet.</returns>
        public SpriteSheet AddSpriteSheet(Bitmap bitmap, Point point)
        {
            return AddSpriteSheet(bitmap, point.X, point.Y);
        }

        /// <summary>
        /// Adds a sprite sheet to this container sprite sheet.
        /// </summary>
        /// <param name="bitmap">The background image.</param>
        /// <param name="x">The x-coordinate of the sprite sheet.</param>
        /// <param name="y">The y-coordinate of the sprite sheet.</param>
        /// <returns>The sprite sheet added to this container sprite sheet.</returns>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "x")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "y")]
        public SpriteSheet AddSpriteSheet(Bitmap bitmap, int x, int y)
        {
            var spriteSheet = new SpriteSheet(bitmap)
            {
                ClassName = "cssg-{0}".Invariant(Helper.GenerateRandomString()),
                X = x,
                Y = y
            };

            return AddSpriteSheet(spriteSheet);
        }

        /// <summary>
        /// Adds a sprite sheet to this container sprite sheet.
        /// </summary>
        /// <param name="spriteSheet">The sprite sheet to add.</param>
        /// <returns>The sprite sheet that was added to this container sprite sheet.</returns>
        public SpriteSheet AddSpriteSheet(SpriteSheet spriteSheet)
        {
            ClassNames.Add(spriteSheet);
            SpriteSheets.Add(spriteSheet);

            return spriteSheet;
        }

        #endregion

        /// <summary>
        /// Removes a sprite sheet from this container sprite sheet.
        /// </summary>
        /// <param name="className">The CSS class that represents the sprite sheet.</param>
        /// <exception cref="InvalidOperationException">A sprite sheet with the class name <paramref name="className" /> does not exist in this container sprite sheet.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="className" /> is not a sprite sheet.</exception>
        public void RemoveSpriteSheet(string className)
        {
            if (ClassNames.Contains(className))
            {
                var spriteSheet = ClassNames[className] as SpriteSheet;
                if (spriteSheet == null)
                    throw new InvalidOperationException(
                        string.Format(
                            CultureInfo.CurrentUICulture,
                            "'{0}' is not a sprite sheet.",
                            className));

                ClassNames.Remove(spriteSheet);
                SpriteSheets.Remove(spriteSheet);
            }
            else
                throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.CurrentUICulture,
                        "A sprite sheet with the class name '{0}' does not exist in this container sprite sheet.",
                        className));
        }

        #endregion

        #region Sprite Methods

        #region AddSprite Overloads

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
        /// <exception cref="ArgumentNullException"><paramref name="parent" /> cannot be null.</exception>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "x")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "y")]
        public Sprite AddSprite(SpriteSheet parent, string className, int x, int y, int width, int height)
        {
            if (parent == null)
                throw new ArgumentNullException("parent");

            var positionX = x - parent.X;
            var positionY = y - parent.Y;
            var sprite = new Sprite(className, positionX, positionY, width, height);

            return AddSprite(parent, sprite);
        }

        /// <summary>
        /// Adds a sprite to a sprite sheet.
        /// </summary>
        /// <param name="className">The class name of the sprite.</param>
        /// <param name="bounds">The bounds of the sprite.</param>
        /// <returns>The sprite that was added to a child sprite sheet.</returns>
        /// <exception cref="ArgumentException">The sprite is not fully enclose by a sprite sheet.</exception>
        public Sprite AddSprite(string className, Rectangle bounds)
        {
            foreach (var spriteSheet in SpriteSheets)
                if (spriteSheet.Bounds.Contains(bounds))
                    return AddSprite(spriteSheet, className, bounds.X, bounds.Y, bounds.Width, bounds.Height);

            throw new ArgumentException(
                "The sprite is not fully enclose by a sprite sheet.",
                "bounds");
        }

        /// <summary>
        /// Adds a sprite to a sprite sheet.
        /// </summary>
        /// <param name="parent">The sprite sheet to add the sprite to.</param>
        /// <param name="sprite">The sprite to be added.</param>
        /// <returns>The sprite that was added.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="parent" /> cannot be null.</exception>
        public Sprite AddSprite(SpriteSheet parent, Sprite sprite)
        {
            if (parent == null)
                throw new ArgumentNullException("parent");

            ClassNames.Add(sprite);
            parent.Sprites.Add(sprite);

            return sprite;
        }

        #endregion

        #region RemoveSprite Overloads

        /// <summary>
        /// Removes a sprite from a sprite sheet.
        /// </summary>
        /// <param name="className">The class name of the sprite to remove.</param>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">An element with the specified key does not exist in the collection.</exception>
        public void RemoveSprite(string className)
        {
            var sprite = Find<Sprite>(className);
            if (sprite != null)
                RemoveSprite(sprite.Parent, sprite);
        }

        /// <summary>
        /// Removes a sprite from a sprite sheet.
        /// </summary>
        /// <param name="parent">The sprite sheet to remove the sprite from.</param>
        /// <param name="sprite">The sprite to remove.</param>
        /// <exception cref="ArgumentNullException"><paramref name="parent" /> cannot be null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="sprite" /> cannot be null.</exception>
        /// <exception cref="InvalidOperationException">The sprite ('<paramref name="sprite" />') is not within this child sprite sheet.</exception>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public void RemoveSprite(SpriteSheet parent, Sprite sprite)
        {
            if (parent == null)
                throw new ArgumentNullException("parent");
            if (sprite == null)
                throw new ArgumentNullException("sprite");

            if (parent.Sprites.Contains(sprite))
            {
                ClassNames.Remove(sprite);
                parent.Sprites.Remove(sprite);
            }
            else
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentUICulture, "The sprite ('{0}') is not within this child sprite sheet.", sprite.ClassName));
        }

        #endregion

        #endregion

        /// <summary>
        /// Saves the composite background image of this sprite sheet.
        /// </summary>
        /// <param name="fileName">The file name to save the image as.</param>
        /// <exception cref="ArgumentOutOfRangeException">width or height cannot be equal to 0.</exception>
        public void SaveImage(string fileName)
        {
            var fileStream = File.Create(fileName);

            try
            {
                SaveImage(fileStream);
            }
            catch
            {
                File.Delete(fileName);
                throw;
            }
            finally
            {
                fileStream.Dispose();
            }
        }

        /// <summary>
        /// Saves the composite background image of this sprite sheet.
        /// </summary>
        /// <param name="fileStream">The file stream to save the image to.</param>
        /// <exception cref="ArgumentOutOfRangeException">width or height cannot be equal to 0.</exception>
        public void SaveImage(Stream fileStream)
        {
            using (var flattenedImage = GetFlattenedImage())
                flattenedImage.Save(fileStream, ImageFormat.Png);
        }

        /// <summary>
        /// Saves the CSS of this sprite sheet.
        /// </summary>
        /// <param name="fileName">The file name to save the CSS as.</param>
        public void SaveCss(string fileName)
        {
            using (var sw = new StreamWriter(fileName))
                sw.Write(Css);
        }

        /// <summary>
        /// Serializes the sprite sheet model to disk.
        /// </summary>
        /// <param name="fileName">The file name to save the sprite sheet model as.</param>
        /// <exception cref="SerializationException">There is a problem with the instance being serialized.</exception>
        public void Save(string fileName)
        {
            using (var memoryStream = new MemoryStream())
            {
                Save(memoryStream);

                // Create the file only if serialization is successful
                using (var fileStream = File.Create(fileName))
                    memoryStream.WriteTo(fileStream);
            }
        }

        /// <summary>
        /// Serializes the sprite sheet model to disk.
        /// </summary>
        /// <param name="stream">The stream to serialize to.</param>
        /// <exception cref="SerializationException">There is a problem with the instance being serialized.</exception>
        public void Save(Stream stream)
        {
            var knownTypes = new Type[] { typeof(SpriteSheet), typeof(Sprite) };
            var serializer = new DataContractSerializer(typeof(SpriteSheetGenerator), knownTypes);
            var xmlWriter = XmlWriter.Create(stream, new XmlWriterSettings { Indent = true, IndentChars = "    " });

            serializer.WriteObject(xmlWriter, this);
            xmlWriter.Flush(); // Must flush or else the entire object does not get written
        }

        /// <summary>
        /// Deserializes a sprite sheet model from a file on disk.
        /// </summary>
        /// <param name="fileName">The filename to load the sprite sheet model from.</param>
        /// <returns>The sprite sheet model.</returns>
        /// <exception cref="InvalidOperationException">
        /// Height or width values differ on one of the images. This is likely indicative 
        /// that the image has been modified since this was saved.
        /// </exception>
        public static SpriteSheetGenerator Load(string fileName)
        {
            FileStream fileStream = null;

            try
            {
                fileStream = File.OpenRead(fileName);
                var serializer = new DataContractSerializer(typeof(SpriteSheetGenerator));
                var spriteSheetGenerator = serializer.ReadObject(fileStream) as SpriteSheetGenerator;

                return spriteSheetGenerator;
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(
                    string.Format(CultureInfo.CurrentUICulture, "File could not be found. \r\nFile name: '{0}' ", fileName),
                    "fileName", ex);
            }
            finally
            {
                if (fileStream != null)
                    fileStream.Dispose();
            }
        }

        /// <summary>
        /// Triggers an arrange.
        /// </summary>
        public void Build()
        {
            // Add the padding
            foreach (var spriteSheet in SpriteSheets)
            {
                spriteSheet.Width = spriteSheet.Image.Width + HorizontalOffset;
                spriteSheet.Height = spriteSheet.Image.Height + VerticalOffset;
            }

            // Arrange
            var mapping = Mapper.Mapping(SpriteSheets);

            // Update sprite sheets
            foreach (var mappedImage in mapping.MappedImages)
            {
                var spriteSheet = mappedImage.ImageInfo as SpriteSheet;
                spriteSheet.X = mappedImage.X;
                spriteSheet.Y = mappedImage.Y;
            }
        }

        /// <summary>
        /// Returns the <see cref="Sprite" /> or <see cref="SpriteSheet" /> that matches <paramref name="className" />.
        /// </summary>
        /// <param name="className">The class name to search for.</param>
        /// <returns>
        /// The <see cref="Sprite" /> or <see cref="SpriteSheet" /> that matches <paramref name="className" />.
        /// </returns>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">An element with the specified key does not exist in the collection.</exception>
        public T Find<T>(string className) where T : SpriteBase
        {
            return ClassNames[className] as T;
        }

        /// <summary>
        /// Disposes of background images.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes of background images.
        /// </summary>
        /// <param name="disposing">
        /// If true, disposes of managed and unmanaged resources. If false, only disposes of
        /// unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            // Need a separate array to iterate from
            var spriteSheets = SpriteSheets.ToArray();

            foreach (var spriteSheet in spriteSheets)
                spriteSheet.Dispose();
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Raises the <see cref="PropertyChanged" /> event.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed value.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Private Methods

        // Performs initialization common to normal construction and deserialization
        private void Initialize()
        {
            SpriteSheets.CollectionChanged += new NotifyCollectionChangedEventHandler(OnSpriteSheetsCollectionChanged);
            PropertyChanged += new PropertyChangedEventHandler(OnSpriteSheetGeneratorPropertyChanged);
        }

        // Runs before deserialization
        [OnDeserializing]
        private void OnDeserializing(StreamingContext context)
        {
            Initialize();
        }

        #region Property/Collection Changed Callbacks

        // Adds/removes a NotifyCollectionChangedEventHandler when a sprite sheet is added/removed
        private void OnSpriteSheetsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (SpriteSheet spriteSheet in e.NewItems)
                        spriteSheet.Sprites.CollectionChanged += new NotifyCollectionChangedEventHandler(OnSpritesCollectionChanged);
                    break;
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Reset:
                    if (e.OldItems == null)
                        break;

                    foreach (SpriteSheet spriteSheet in e.OldItems)
                        spriteSheet.Sprites.CollectionChanged -= OnSpritesCollectionChanged;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        // Adds/removes a PropertyChangedEventHandler when a sprite is added/removed
        // Adds/removes a sprite to/from Sprites when a sprite is added/removed from a child sprite sheet's collection
        private void OnSpritesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (Sprite sprite in e.NewItems)
                    {
                        sprite.PropertyChanged += new PropertyChangedEventHandler(OnSpritePropertyChanged);
                        Sprites.Add(sprite);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Reset:
                    if (e.OldItems == null)
                        break;

                    foreach (Sprite sprite in e.OldItems)
                    {
                        sprite.PropertyChanged -= OnSpritePropertyChanged;
                        Sprites.Remove(sprite);
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }

            UpdateCss();
        }

        // When the build method changes, update the mapper
        private void OnSpriteSheetGeneratorPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == BuildMethodPropertyName)
                UpdateMapper();
        }

        // Updates the Css property when a change is made to a child sprite
        private void OnSpritePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateCss();
        }

        #endregion

        #region Property Updating Methods

        // Gets the flattened image of all the sprite sheets
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        private Bitmap GetFlattenedImage()
        {
            int width = 0;
            int height = 0;

            foreach (var spriteSheet in SpriteSheets)
            {
                if (spriteSheet.X + spriteSheet.Image.Width > width)
                    width = spriteSheet.X + spriteSheet.Image.Width;
                if (spriteSheet.Y + spriteSheet.Image.Height > height)
                    height = spriteSheet.Y + spriteSheet.Image.Height;
            }

            Bitmap flattenedImage;
            try
            {
                flattenedImage = new Bitmap(width, height);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentOutOfRangeException("width or height cannot be equal to 0.", ex);
            }

            using (var g = Graphics.FromImage(flattenedImage))
            {
                g.CompositingMode = CompositingMode.SourceCopy;
                foreach (var spriteSheet in SpriteSheets)
                    g.DrawImage(spriteSheet.Image, spriteSheet.X, spriteSheet.Y);

                return flattenedImage;
            }
        }

        // Updates the Css property
        private void UpdateCss()
        {
            var cssRules = new List<CssRule>();

            foreach (var spriteSheet in SpriteSheets)
                foreach (var sprite in spriteSheet.Sprites)
                {
                    var cssRule = new CssRule();

                    cssRule.Selectors.Add(sprite.ClassName);

                    var actualX = spriteSheet.X + sprite.X;
                    var actualY = spriteSheet.Y + sprite.Y;

                    cssRule.Declarations.Add("background-position", string.Format(CultureInfo.InvariantCulture, "{0}px {1}px", actualX, actualY));
                    cssRule.Declarations.Add("width", string.Format(CultureInfo.InvariantCulture, "{0}px", sprite.Width));
                    cssRule.Declarations.Add("height", string.Format(CultureInfo.InvariantCulture, "{0}px", sprite.Height));

                    cssRules.Add(cssRule);
                }

            var css = new StringBuilder();

            for (int i = 0; i < cssRules.Count; i++)
            {
                css.Append(cssRules[i]);
                if (i != cssRules.Count - 1)
                    css.AppendLine();
            }

            if (!string.IsNullOrEmpty(css.ToString()))
                Css = css.ToString();
            else
                Css = null;
        }

        // Sets the appropriate mapper based on the build method
        private void UpdateMapper()
        {
            switch (BuildMethod)
            {
                case Arrange.Horizontal:
                    Mapper = new MapperHorizontalOnly<SpriteMapper>();
                    break;
                case Arrange.Vertical:
                    Mapper = new MapperVerticalOnly<SpriteMapper>();
                    break;
                case Arrange.Optimal:
                    Mapper = new MapperOptimalEfficiency<SpriteMapper>(new Canvas());
                    break;
            }
        }

        #endregion

        #endregion
    }
}
