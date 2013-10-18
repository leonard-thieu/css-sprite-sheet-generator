using System.Windows.Input;
using CssSpriteSheetGenerator.Gui.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CssSpriteSheetGenerator.Gui.Tests.Infrastructure
{
    [TestClass]
    public class RelayRoutedUICommandTests
    {
        private RelayRoutedUICommand relayRoutedUICommand;

        [TestInitialize]
        public void Initialize()
        {
            relayRoutedUICommand = new RelayRoutedUICommand(() => { }, "Text", new InputGestureCollection { new KeyGesture(Key.F1) });
        }

        [TestMethod]
        public void Constructor_InitializesProperly()
        {
            Assert.IsTrue(relayRoutedUICommand.CanExecute());
        }

        [TestMethod]
        public void Constructor_InitializesProperly2()
        {
            relayRoutedUICommand = new RelayRoutedUICommand(() => { }, () => { return false; }, "Text", new InputGestureCollection { new KeyGesture(Key.F1) });
            Assert.IsFalse(relayRoutedUICommand.CanExecute());
        }

        [TestMethod]
        public void TextTest()
        {
            Assert.AreEqual("Text", relayRoutedUICommand.Text);
        }

        [TestMethod]
        public void InputGesturesTest()
        {
            Assert.AreEqual(Key.F1, ((KeyGesture)relayRoutedUICommand.InputGestures[0]).Key);
        }
    }
}
