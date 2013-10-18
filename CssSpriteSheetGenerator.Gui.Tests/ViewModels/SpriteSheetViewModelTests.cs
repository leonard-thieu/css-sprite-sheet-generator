using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using CssSpriteSheetGenerator.Gui.Properties;
using CssSpriteSheetGenerator.Gui.ViewModels;
using CssSpriteSheetGenerator.Models;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CssSpriteSheetGenerator.Gui.Tests.ViewModels
{
    [TestClass]
    public class SpriteSheetViewModelTests
    {
        private SpriteSheetViewModel spriteSheetViewModel;

        [TestInitialize]
        public void Initialize()
        {
            spriteSheetViewModel = new SpriteSheetViewModel();
        }

        [TestMethod]
        public void New_ResetsSpriteSheetViewModel()
        {
            var spriteSheetGenerator = spriteSheetViewModel.SpriteSheetGenerator;
            Messenger.Default.Register<DialogMessage>(this, m => m.ProcessCallback(MessageBoxResult.No));

            spriteSheetViewModel.New.Execute();

            Assert.AreNotSame(spriteSheetGenerator, spriteSheetViewModel.SpriteSheetGenerator);
            Assert.AreEqual(Resources.Untitled, spriteSheetViewModel.FileName);
            Assert.IsFalse(spriteSheetViewModel.IsModified);
        }

        [TestMethod]
        public void LoadStateTest()
        {
            var spriteSheetGenerator = spriteSheetViewModel.SpriteSheetGenerator;
            Messenger.Default.Register<DialogMessage>(this, m => m.ProcessCallback(MessageBoxResult.No));
            Messenger.Default.Register<NotificationMessageAction<string>>(this, m => m.Execute("YEAHHHHHHHHHHHHHHh.cssg"));

            spriteSheetViewModel.Import.Execute();

            Assert.AreNotSame(spriteSheetGenerator, spriteSheetViewModel.SpriteSheetGenerator);
            Assert.IsFalse(spriteSheetViewModel.IsModified);
        }

        [TestMethod]
        public void SaveStateTest()
        {
            var fileName = "SAVE.cssg";
            spriteSheetViewModel.AddSpriteSheet("Close.png", new Point());
            Messenger.Default.Register<NotificationMessageAction<string>>(this, m => m.Execute(fileName));

            spriteSheetViewModel.Export.Execute();

            Assert.IsTrue(File.Exists(fileName));
            Assert.IsFalse(spriteSheetViewModel.IsModified);

            File.Delete(fileName);
        }

        [TestMethod]
        public void SaveState_WhenFileNameIsNull_SendsMessage()
        {
            spriteSheetViewModel.AddSpriteSheet("Close.png", new Point());
            var received = false;
            Messenger.Default.Register<NotificationMessageAction<string>>(this, m => m.Execute(null));
            Messenger.Default.Register<NotificationMessage<string>>(this, m => { received = true; });

            spriteSheetViewModel.Export.Execute();

            Assert.IsTrue(received);
        }

        [TestMethod]
        public void SaveImageTest()
        {
            var fileName = "IMAGE.png";
            var imageFileName = "Close.png";
            Messenger.Default.Register<NotificationMessageAction<string>>(this, m => m.Execute(fileName));
            spriteSheetViewModel.AddSpriteSheet(imageFileName, new Point());

            spriteSheetViewModel.SaveImage.Execute();

            Assert.IsTrue(File.Exists(fileName));

            File.Delete(fileName);
        }

        [TestMethod]
        public void SaveCssTest()
        {
            var fileName = "SHEET.css";
            var imageFileName = "Close.png";
            Messenger.Default.Register<NotificationMessageAction<string>>(this, m => m.Execute(fileName));
            spriteSheetViewModel.AddSpriteSheet(imageFileName, new Point());

            spriteSheetViewModel.SaveCss.Execute();

            Assert.IsTrue(File.Exists(fileName));

            File.Delete(fileName);
        }

        [TestMethod]
        public void ToggleRectangleSelectToolTest()
        {
            spriteSheetViewModel.ToggleRectangleSelectTool.Execute();
            Assert.IsTrue(spriteSheetViewModel.IsRectangleSelectToolEnabled);

            spriteSheetViewModel.ToggleRectangleSelectTool.Execute();
            Assert.IsFalse(spriteSheetViewModel.IsRectangleSelectToolEnabled);
        }

        [TestMethod]
        public void BuildTest()
        {
            var spriteSheet = spriteSheetViewModel.AddSpriteSheet("Close.png", new Point(1, 1));

            spriteSheetViewModel.Build.Execute();

            Assert.AreEqual(new Point(), new Point(spriteSheet.X, spriteSheet.Y));
        }

        [TestMethod]
        public void RemoveSpriteSheetTest()
        {
            var spriteSheet = spriteSheetViewModel.AddSpriteSheet("Close.png", new Point(1, 1));
            var className = spriteSheet.ClassName;

            spriteSheetViewModel.RemoveSpriteSheet(className);

            Assert.AreEqual(0, spriteSheetViewModel.SpriteSheetGenerator.SpriteSheets.Count);
        }

        [TestMethod]
        public void RemoveSpriteTest()
        {
            spriteSheetViewModel.AddSpriteSheet("Close.png", new Point(1, 1));

            var spriteClassName = spriteSheetViewModel.SpriteSheetGenerator.Sprites[0].ClassName;
            spriteSheetViewModel.RemoveSprite(spriteClassName);

            Assert.AreEqual(0, spriteSheetViewModel.SpriteSheetGenerator.Sprites.Count);
        }
    }
}
