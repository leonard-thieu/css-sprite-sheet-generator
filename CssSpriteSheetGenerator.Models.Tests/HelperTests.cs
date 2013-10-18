using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CssSpriteSheetGenerator.Models.Tests
{
    [TestClass]
    public class HelperTests
    {
        [TestMethod]
        public void ResizeArray_ProducesCorrectlySizedArray()
        {
            // Arrange
            var array = new bool[1, 1];

            // Act
            var actual = Helper.ResizeArray(array, new[] { 2, 3 });

            // Assert
            Assert.AreEqual(2, actual.GetLength(0));
            Assert.AreEqual(3, actual.GetLength(1));
        }

        [TestMethod]
        public void ResizeArray_WithSmallNewSizesParameter_ProducesCorrectlySizedArray()
        {
            // Arrange
            var array = new bool[2, 3];

            // Act
            var actual = Helper.ResizeArray(array, new[] { 1, 1 });

            // Assert
            Assert.AreEqual(1, actual.GetLength(0));
            Assert.AreEqual(1, actual.GetLength(1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ResizeArray_WithNullArrayParameter_ThrowsException()
        {
            // Arrange
            var array = new bool[1, 1];

            // Act/Assert
            var actual = Helper.ResizeArray(null, new[] { 2, 3 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ResizeArray_WithNullNewSizesParameter_ThrowsException()
        {
            // Arrange
            var array = new bool[1, 1];

            // Act/Assert
            var actual = Helper.ResizeArray(array, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ResizeArray_WithIncorrectNewSizesParameter_ThrowsException()
        {
            // Arrange
            var array = new bool[1, 1];

            // Act/Assert
            var actual = Helper.ResizeArray(array, new int[] { 2, 3, 4 });
        }

        [TestMethod]
        public void GetAllMessagesFromExceptions_GetsAllMessages()
        {
            // Arrange
            var ex1 = new Exception("ex1");
            var ex2 = new Exception("ex2", ex1);

            // Act
            var messages = ex2.GetAllMessages();

            // Assert
            Assert.AreEqual("ex2\r\nex1", messages);
        }

        [TestMethod]
        public void GetBitsPerPixel_GetsCorrectBitsPerPixel()
        {
            // Arrange
            var pixelFormats = new PixelFormat[]
            { 
                PixelFormat.Format1bppIndexed,
                PixelFormat.Format4bppIndexed,
                PixelFormat.Format8bppIndexed,
                PixelFormat.Format16bppRgb565,
                PixelFormat.Format24bppRgb,
                PixelFormat.Format32bppArgb,
                PixelFormat.Format48bppRgb,
                PixelFormat.Format64bppArgb,
                PixelFormat.Undefined
            };
            var bpps = new byte[9];

            // Act
            for (int i = 0; i < pixelFormats.Length; i++)
                bpps[i] = Helper.GetBitsPerPixel(pixelFormats[i]);

            // Assert
            Assert.AreEqual(1, bpps[0]);
            Assert.AreEqual(4, bpps[1]);
            Assert.AreEqual(8, bpps[2]);
            Assert.AreEqual(16, bpps[3]);
            Assert.AreEqual(24, bpps[4]);
            Assert.AreEqual(32, bpps[5]);
            Assert.AreEqual(48, bpps[6]);
            Assert.AreEqual(64, bpps[7]);
            Assert.AreEqual(0, bpps[8]);
        }

        [TestMethod]
        public void Clamping_ValueInRange_DoesNothing()
        {
            // Arrange
            var min = 0;
            var value = 1;
            var max = 2;

            // Act
            var actual = value.Clamp(max, min);

            // Assert
            Assert.AreEqual(1, actual);
        }

        [TestMethod]
        public void Clamping_ValueAboveMax_ClampsToMax()
        {
            // Arrange
            var min = 0;
            var value = 3;
            var max = 2;

            // Act
            var actual = value.Clamp(max, min);

            // Assert
            Assert.AreEqual(2, actual);
        }

        [TestMethod]
        public void Clamping_ValueBelowMin_ClampsToMin()
        {
            // Arrange
            var min = 0;
            var value = -1;
            var max = 2;

            // Act
            var actual = value.Clamp(max, min);

            // Assert
            Assert.AreEqual(0, actual);
        }

        [TestMethod]
        public void SetColor_SetsColor()
        {
            // Arrange
            var data = new byte[4];
            var pixelIndex = 0;
            var color = Color.White;

            // Act
            Helper.SetColor(data, pixelIndex, color);

            // Assert
            Assert.AreEqual(data[0], byte.MaxValue);
            Assert.AreEqual(data[1], byte.MaxValue);
            Assert.AreEqual(data[2], byte.MaxValue);
            Assert.AreEqual(data[3], byte.MaxValue);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetColor_WithNullData_ThrowsException()
        {
            // Arrange
            byte[] data = null;
            var pixelIndex = 0;
            var color = Color.White;

            // Act/Assert
            Helper.SetColor(data, pixelIndex, color);
        }
    }
}
