using System;
using System.Drawing;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CssSpriteSheetGenerator.Gui.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CssSpriteSheetGenerator.Gui.Tests.Converters
{
    [TestClass]
    public class BitmapToImageBrushConverterTests
    {
        private BitmapToImageBrushConverter convert;

        [TestInitialize]
        public void Initialize()
        {
            convert = new BitmapToImageBrushConverter();
        }

        [TestMethod]
        public void ConvertingBitmap_ResultsInCorrectImageBrush()
        {
            // Arrange
            var bitmap = new Bitmap(1, 1);

            // Act
            var imageBrush = (ImageBrush)convert.Convert(bitmap, typeof(ImageBrush), null, null);

            // Assert
            var actualWidth = (int)imageBrush.ImageSource.Width;
            var actualHeight = (int)imageBrush.ImageSource.Height;
            Assert.AreEqual(1, actualWidth);
            Assert.AreEqual(1, actualHeight);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ConvertingImageBrush_ToBitmap_ThrowsException()
        {
            // Arrange
            var streamSource = File.OpenRead(@"Close.png");

            try
            {
                var bitmapImage = new BitmapImage { StreamSource = streamSource };
                var imageBrush = new ImageBrush(bitmapImage);

                // Act
                var bitmap = (Bitmap)convert.ConvertBack(imageBrush, typeof(Bitmap), null, null);
            }
            finally
            {
                streamSource.Dispose();
            }
        }
    }
}
