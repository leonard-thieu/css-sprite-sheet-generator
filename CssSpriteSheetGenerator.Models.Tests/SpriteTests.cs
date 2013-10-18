using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CssSpriteSheetGenerator.Models.Tests
{
    [TestClass]
    public class SpriteTests
    {
        [TestMethod]
        public void Sprite_IsGarbageCollected_AfterRemovedFromParent()
        {
            var spriteSheet = new SpriteSheet((Bitmap)null);
            WeakReference reference = null;
            new Action(() =>
            {
                var sprite = new Sprite("SPRITE", 0, 0, 1, 1);
                spriteSheet.Sprites.Add(sprite);
                spriteSheet.Sprites.Remove(sprite);

                reference = new WeakReference(sprite, true);
            })();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            Assert.IsNull(reference.Target);
        }
    }
}
