using System;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CssSpriteSheetGenerator.Models.Tests
{
    [TestClass]
    public class SpriteCollectionTests
    {
        private SpriteSheet spriteSheet;
        private SpriteCollection spriteCollection;

        [TestInitialize]
        public void Initialize()
        {
            spriteSheet = new SpriteSheet((Bitmap)null);
            spriteCollection = new SpriteCollection(spriteSheet);
        }

        [TestMethod]
        public void Constructor_InitializesProperly()
        {
            // Taken care of by Initialize()
        }

        [TestMethod]
        public void AddingSprite_SetsParent()
        {
            var sprite = GimmeSprite();
            spriteCollection.Add(sprite);
            Assert.AreEqual(spriteSheet, sprite.Parent);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddingNull_ThrowsException()
        {
            spriteCollection.Add(null);
        }

        [TestMethod]
        public void RemovingSprite_GarbageCollectsSprite()
        {
            WeakReference reference = null;
            new Action(() =>
            {
                var sprite = GimmeSprite();
                spriteCollection.Add(sprite);
                spriteCollection.Remove(sprite);

                reference = new WeakReference(sprite, true);
            })();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            Assert.IsNull(reference.Target);
        }

        [TestMethod]
        public void ClearingCollection_GarbageCollectsChildren()
        {
            List<WeakReference> references = new List<WeakReference>();
            new Action(() =>
            {
                var sprites = new Sprite[]
                {
                    GimmeSprite(),
                    GimmeSprite(),
                    GimmeSprite(),
                    GimmeSprite(),
                    GimmeSprite()
                };

                foreach (var sprite in sprites)
                {
                    spriteCollection.Add(sprite);
                    references.Add(new WeakReference(sprite, true));
                }
                spriteCollection.Clear();
            })();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            foreach (var reference in references)
                Assert.IsNull(reference.Target);
        }

        private Sprite GimmeSprite()
        {
            return new Sprite("", 0, 0, 1, 1);
        }
    }
}
