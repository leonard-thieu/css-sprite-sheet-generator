using System.Windows.Input;
using CssSpriteSheetGenerator.Gui.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CssSpriteSheetGenerator.Gui.Tests.Infrastructure
{
    [TestClass]
    public class RelayRoutedUICommand_T_Tests
    {
        private RelayRoutedUICommand<int> relayRoutedUICommand;

        [TestInitialize]
        public void Initialize()
        {
            relayRoutedUICommand = new RelayRoutedUICommand<int>(i => { }, "Text", new InputGestureCollection { new KeyGesture(Key.F1) });
        }

        [TestMethod]
        public void Constructor_InitializesProperly()
        {
            Assert.IsTrue(relayRoutedUICommand.CanExecute(null));
        }

        [TestMethod]
        public void Constructor_InitializesProperly2()
        {
            relayRoutedUICommand = new RelayRoutedUICommand<int>(i => { }, i => { return false; }, "Text", new InputGestureCollection { new KeyGesture(Key.F1) });
            Assert.IsFalse(relayRoutedUICommand.CanExecute(null));
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
