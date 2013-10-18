using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CssSpriteSheetGenerator.Models.Tests
{
    [TestClass]
    public class ClassNameKeyedCollectionTests
    {
        private ClassNameKeyedCollection classNameKeyedCollection;

        [TestInitialize]
        public void Initialize()
        {
            classNameKeyedCollection = new ClassNameKeyedCollection();
        }

        [TestMethod]
        public void ChangingItemClassName_AfterAdding_UpdatesKey()
        {
            var spriteBase = new Sprite("SPRITE", 0, 0, 1, 1);
            classNameKeyedCollection.Add(spriteBase);
            var className = "SUPERSPRITE";

            spriteBase.ClassName = className;

            var actual = classNameKeyedCollection.Contains(className);
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void AddingItem_WithExistingClassNames_ChangesClassNameAndAdds()
        {
            var spriteBase1 = new Sprite("SPRITE", 0, 0, 1, 1);
            classNameKeyedCollection.Add(spriteBase1);
            var spriteBase2 = new Sprite("SPRITE", 0, 0, 1, 1);
            classNameKeyedCollection.Add(spriteBase2);
            var spriteBase3 = new Sprite("SPRITE", 0, 0, 1, 1);
            classNameKeyedCollection.Add(spriteBase3);

            Assert.AreNotSame(spriteBase1.ClassName, spriteBase2.ClassName);
            Assert.AreNotSame(spriteBase1.ClassName, spriteBase3.ClassName);
            Assert.AreNotSame(spriteBase2.ClassName, spriteBase3.ClassName);
            Assert.IsTrue(classNameKeyedCollection.Contains(spriteBase1));
            Assert.IsTrue(classNameKeyedCollection.Contains(spriteBase2));
            Assert.IsTrue(classNameKeyedCollection.Contains(spriteBase3));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddingNull_ThrowsException()
        {
            classNameKeyedCollection.Add(null);
        }
    }
}
