using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CssSpriteSheetGenerator.Models.Tests
{
    [TestClass]
    public class SpriteSheetTests
    {
        [TestMethod]
        public void CanAdd_Sprite()
        {
            // Arrange
            var spriteSheetCollection = new ObservableCollection<SpriteSheet>();
            var spriteSheet = new SpriteSheet(new Bitmap(205, 359));
            var sprite = new Sprite("", 5, 10, 100, 349);

            // Act
            spriteSheet.Sprites.Add(sprite);

            // Assert
            Assert.IsTrue(spriteSheet.Sprites.Contains(sprite));
        }

        [TestMethod]
        public void CanAdd_NonOverlappingSprite()
        {
            // Arrange
            var spriteSheetCollection = new ObservableCollection<SpriteSheet>();
            var spriteSheet = new SpriteSheet(new Bitmap(205, 359));
            var sprite1 = new Sprite("", 5, 10, 100, 349);
            var sprite2 = new Sprite("", 105, 10, 100, 349);

            // Act
            spriteSheet.Sprites.Add(sprite1);
            spriteSheet.Sprites.Add(sprite2);

            // Assert
            Assert.IsTrue(spriteSheet.Sprites.Contains(sprite2));
        }

        [TestMethod]
        public void CanRemove_Sprite()
        {
            // Arrange
            var spriteSheetCollection = new ObservableCollection<SpriteSheet>();
            var spriteSheet = new SpriteSheet(new Bitmap(205, 359));
            var sprite = new Sprite("", 5, 10, 100, 349);

            // Act
            spriteSheet.Sprites.Add(sprite);
            spriteSheet.Sprites.Remove(sprite);

            // Assert
            Assert.IsFalse(spriteSheet.Sprites.Contains(sprite));
        }

        [TestMethod]
        public void CanAdd_SpriteSheet()
        {
            // Arrange
            var spriteSheetCollection = new ObservableCollection<SpriteSheet>();
            var spriteSheet = new SpriteSheet(new Bitmap(205, 359));
            var sprite = new Sprite("", 5, 10, 100, 349);
            spriteSheet.Sprites.Add(sprite);

            var spriteSheetGenerator = new SpriteSheetGenerator();

            // Act
            spriteSheetGenerator.AddSpriteSheet(spriteSheet);

            // Assert
            Assert.IsTrue(spriteSheetGenerator.SpriteSheets.Contains(spriteSheet));
        }

        [TestMethod]
        public void Serialization_ProducesCorrectOutput()
        {
            // Arrange
            var spriteSheetGenerator = new SpriteSheetGenerator();
            string fileName = "Close.png";
            var spriteSheet = spriteSheetGenerator.AddSpriteSheet(fileName, 50, 50);
            var bounds = new Rectangle(50, 50, 16, 16);
            var sprite = spriteSheetGenerator.AddSprite("ExampleSprite", bounds);
            var spriteSheetClassName = spriteSheet.ClassName;
            var spriteClassName = sprite.ClassName;

            // Act
            var ms = new MemoryStream();
            spriteSheetGenerator.Save(ms);
            string actual;
            using (var sr = new StreamReader(ms))
            {
                ms.Position = 0;
                actual = sr.ReadToEnd();
            }

            // Assert
            var sb = new StringBuilder();
            sb.AppendFormat("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n");
            sb.AppendFormat("<SpriteSheetGenerator xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://schemas.toofz.com/cssg\">\r\n");
            sb.AppendFormat("    <SpriteSheets>\r\n");
            sb.AppendFormat("        <SpriteSheet>\r\n");
            sb.AppendFormat("            <ClassName>{0}</ClassName>\r\n", spriteSheetClassName);
            sb.AppendFormat("            <X>50</X>\r\n");
            sb.AppendFormat("            <Y>50</Y>\r\n");
            sb.AppendFormat("            <ImageFile>Close.png</ImageFile>\r\n");
            sb.AppendFormat("            <Sprites>\r\n");
            sb.AppendFormat("                <Sprite>\r\n");
            sb.AppendFormat("                    <ClassName>{0}</ClassName>\r\n", spriteClassName);
            sb.AppendFormat("                    <X>0</X>\r\n");
            sb.AppendFormat("                    <Y>0</Y>\r\n");
            sb.AppendFormat("                    <Width>16</Width>\r\n");
            sb.AppendFormat("                    <Height>16</Height>\r\n");
            sb.AppendFormat("                </Sprite>\r\n");
            sb.AppendFormat("            </Sprites>\r\n");
            sb.AppendFormat("        </SpriteSheet>\r\n");
            sb.AppendFormat("    </SpriteSheets>\r\n");
            sb.AppendFormat("</SpriteSheetGenerator>");
            var expected = sb.ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SettingSpriteSheetWidthOrHeight_ToSameValue_DoesNothing()
        {
            // Arrange
            var spriteSheet = new SpriteSheet(new Bitmap(1, 1));

            // Act
            spriteSheet.Width = 1;
            spriteSheet.Height = 1;

            // Assert
            Assert.AreEqual(1, spriteSheet.Width);
            Assert.AreEqual(1, spriteSheet.Height);
        }

        [TestMethod]
        public void GettingIsExpanded_WhileBackingFieldIsNull_ReturnsTrue()
        {
            // Arrange
            var spriteSheet = new SpriteSheet(new Bitmap(1, 1));

            // Act
            var actual = spriteSheet.IsExpanded;

            // Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void SettingIsExpanded_ToSameValue_DoesNothing()
        {
            // Arrange
            var spriteSheet = new SpriteSheet(new Bitmap(1, 1));

            // Act
            spriteSheet.IsExpanded = false;
            var ie1 = spriteSheet.IsExpanded;
            spriteSheet.IsExpanded = false;
            var ie2 = spriteSheet.IsExpanded;

            // Assert
            Assert.AreEqual(ie1, ie2);
        }

        [TestMethod]
        public void CreatingSpriteSheet_WithNullParameter_CreatesDefaultBitmap()
        {
            // Arrange
            var spriteSheet = new SpriteSheet((Bitmap)null);

            // Act
            var actual = spriteSheet.Image;

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Width);
            Assert.AreEqual(1, actual.Height);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DisposingSpriteSheet_DisposesBitmap()
        {
            // Arrange
            var spriteSheet = new SpriteSheet(new Bitmap(1, 1));

            // Act/Assert
            spriteSheet.Dispose();
            var width = spriteSheet.Image.Width;
        }
    }
}