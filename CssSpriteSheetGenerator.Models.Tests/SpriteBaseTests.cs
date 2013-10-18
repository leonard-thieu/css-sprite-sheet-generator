using System.ComponentModel;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CssSpriteSheetGenerator.Models.Tests
{
    [TestClass]
    public class SpriteBaseTests
    {
        private SpriteBase spriteBase;

        [TestInitialize]
        public void Initialize()
        {
            spriteBase = new SpriteSheet((Bitmap)null);
        }

        [TestMethod]
        public void ClassNameTest()
        {
            spriteBase.ClassName = "SPRITE";
            spriteBase.ClassName = "SPRITE";
            Assert.AreEqual("SPRITE", spriteBase.ClassName);
        }

        [TestMethod]
        public void ClassName_NeverSet_ReturnsEmptyString()
        {
            Assert.AreEqual(string.Empty, spriteBase.ClassName);
        }

        [TestMethod]
        public void XTest()
        {
            spriteBase.X = 2;
            spriteBase.X = 2;
            Assert.AreEqual(2, spriteBase.X);
        }

        [TestMethod]
        public void YTest()
        {
            spriteBase.Y = 2;
            spriteBase.Y = 2;
            Assert.AreEqual(2, spriteBase.Y);
        }

        [TestMethod]
        public void IsSelectedTest()
        {
            spriteBase.IsSelected = true;
            spriteBase.IsSelected = true;
            Assert.IsTrue(spriteBase.IsSelected);
        }

        [TestMethod]
        public void PropertyChangedEvent_IsRaised_WhenThereAreSubscribers()
        {
            bool triggered = false;
            spriteBase.PropertyChanged += new PropertyChangedEventHandler((s, e) => { triggered = true; });
            spriteBase.ClassName = "CHANGED";
            Assert.IsTrue(triggered);
        }
    }
}
