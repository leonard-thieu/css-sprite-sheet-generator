using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CssSpriteSheetGenerator.Models.Tests
{
    [TestClass]
    public class SpriteMapperTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddMappedImage_WithNullParameter_ThrowsException()
        {
            // Arrange
            var spriteMapper = new SpriteMapper();

            // Act/Assert
            spriteMapper.AddMappedImage(null);
        }

        [TestMethod]
        public void Area_ReturnsResult()
        {
            // Arrange
            var spriteMapper = new SpriteMapper();

            // Act
            var actual = spriteMapper.Area;

            // Assert
            Assert.AreEqual(0, actual);
        }
    }
}
