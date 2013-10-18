using System;
using System.Drawing;
using System.IO;
using Mapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CssSpriteSheetGenerator.Models.Tests
{
    [TestClass]
    public class SpriteSheetGeneratorTests
    {
        private SpriteSheetGenerator spriteSheetGenerator;

        [TestInitialize]
        public void Initialize()
        {
            spriteSheetGenerator = new SpriteSheetGenerator();
        }

        [TestMethod]
        public void SaveImage_ProducesCorrectFlattenedImage()
        {
            // Arrange
            var spriteSheet = new SpriteSheet(new Bitmap(1, 1));
            spriteSheetGenerator.AddSpriteSheet(spriteSheet);

            // Act
            Bitmap actual;
            using (var ms = new MemoryStream())
            {
                spriteSheetGenerator.SaveImage(ms);
                actual = new Bitmap(ms);
            }

            // Assert
            Assert.AreEqual(1, actual.Width);
            Assert.AreEqual(1, actual.Height);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SaveImage_WithNoSpriteSheets_ThrowsException()
        {
            // Act/Assert
            Bitmap actual;
            using (var ms = new MemoryStream())
            {
                spriteSheetGenerator.SaveImage(ms);
                actual = new Bitmap(ms);
            }
        }

        [TestMethod]
        public void CanAddSprite()
        {
            // Arrange
            var spriteSheet = new SpriteSheet(new Bitmap(1, 1));
            spriteSheetGenerator.AddSpriteSheet(spriteSheet);

            // Act
            spriteSheetGenerator.AddSprite("SPRITE", new Rectangle(0, 0, 1, 1));

            // Assert
            var sprite = spriteSheet.Sprites[0];
            Assert.AreEqual("SPRITE", sprite.ClassName);
            Assert.AreEqual(0, sprite.X);
            Assert.AreEqual(0, sprite.Y);
            Assert.AreEqual(1, sprite.Width);
            Assert.AreEqual(1, sprite.Height);
        }

        [TestMethod]
        public void ChangingBuildMethod_UpdatesMapper()
        {
            // Act
            spriteSheetGenerator.BuildMethod = Arrange.Optimal;
            spriteSheetGenerator.BuildMethod = Arrange.Horizontal;

            // Assert
            Assert.IsInstanceOfType(spriteSheetGenerator.Mapper, typeof(IMapper<SpriteMapper>));
        }

        [TestMethod]
        public void CanBuild()
        {
            // Arrange
            var spriteSheet = new SpriteSheet(new Bitmap(1, 1)) { X = 10, Y = 10 };
            spriteSheetGenerator.AddSpriteSheet(spriteSheet);

            // Act
            spriteSheetGenerator.BuildMethod = Arrange.Horizontal;
            spriteSheetGenerator.Build();

            // Assert
            Assert.AreEqual(0, spriteSheet.X);
            Assert.AreEqual(0, spriteSheet.Y);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddingSprite_WithNullParent_ThrowsException()
        {
            // Act/Assert
            spriteSheetGenerator.AddSprite(null, "", 0, 0, 0, 0);
        }

        [TestMethod]
        public void SettingCss_ToSameValue_DoesNothing()
        {
            // Act
            spriteSheetGenerator.Css = null;

            // Assert
            Assert.AreEqual(null, spriteSheetGenerator.Css);
        }

        [TestMethod]
        public void RemoveSpriteSheetTest()
        {
            // Arrange
            var className = "SHEET";
            var spriteSheet = new SpriteSheet((Bitmap)null) { ClassName = className };
            spriteSheetGenerator.AddSpriteSheet(spriteSheet);

            // Act
            spriteSheetGenerator.RemoveSpriteSheet(className);

            // Assert
            Assert.AreEqual(0, spriteSheetGenerator.SpriteSheets.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RemoveSpriteSheet_NoMatchingClassName_ThrowsException()
        {
            spriteSheetGenerator.RemoveSpriteSheet("SHEET");
        }

        [TestMethod]
        public void FindTest1()
        {
            var spriteClassName = "SPRITE";
            var spriteSheetClassName = "SPRITESHEET";
            var spriteSheet = new SpriteSheet((Bitmap)null) { ClassName = spriteSheetClassName };
            spriteSheetGenerator.AddSpriteSheet(spriteSheet);
            var expected = spriteSheetGenerator.AddSprite(spriteSheet, spriteClassName, 0, 0, 1, 1);

            var actual = spriteSheetGenerator.Find<Sprite>(spriteClassName);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FindTest2()
        {
            var className = "SPRITESHEET";
            var spriteSheet = new SpriteSheet((Bitmap)null) { ClassName = className };
            spriteSheetGenerator.AddSpriteSheet(spriteSheet);

            var actual = spriteSheetGenerator.Find<SpriteSheet>(className);

            Assert.AreEqual(spriteSheet, actual);
        }

        [TestMethod]
        public void RemoveSpriteTest()
        {
            var spriteSheet = new SpriteSheet((Bitmap)null);
            spriteSheetGenerator.AddSpriteSheet(spriteSheet);
            var sprite = spriteSheetGenerator.AddSprite("SPRITE", new Rectangle(0, 0, 1, 1));

            spriteSheetGenerator.RemoveSprite(spriteSheet, sprite);

            Assert.AreEqual(0, spriteSheet.Sprites.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RemoveSprite_WithNullParentParameter_ThrowsException()
        {
            spriteSheetGenerator.RemoveSprite(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RemoveSprite_WithNullSpriteParameter_ThrowsException()
        {
            var spriteSheet = new SpriteSheet((Bitmap)null);
            spriteSheetGenerator.RemoveSprite(spriteSheet, null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RemoveSprite_SpriteIsNotInCollection_ThrowsException()
        {
            var spriteSheet = new SpriteSheet((Bitmap)null);
            var sprite = new Sprite("", 0, 0, 1, 1);
            spriteSheetGenerator.RemoveSprite(spriteSheet, sprite);
        }

        [TestMethod]
        public void Build_WithHorizontalOffset_BuildsOffsetSpriteSheet()
        {
            var spriteSheet1 = spriteSheetGenerator.AddSpriteSheet("Close.png", new Point());
            var spriteSheet2 = spriteSheetGenerator.AddSpriteSheet("Close.png", new Point());
            spriteSheetGenerator.HorizontalOffset = 50;
            spriteSheetGenerator.VerticalOffset = 50;
            spriteSheetGenerator.BuildMethod = Arrange.Vertical;

            spriteSheetGenerator.Build();

            Assert.AreEqual(spriteSheet1.Image.Height + spriteSheetGenerator.VerticalOffset, spriteSheet2.Y);
        }
    }
}
