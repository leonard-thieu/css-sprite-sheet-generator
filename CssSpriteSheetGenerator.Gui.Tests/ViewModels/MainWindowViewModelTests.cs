using System.ComponentModel;
using System.IO;
using System.Windows;
using CssSpriteSheetGenerator.Gui.Properties;
using CssSpriteSheetGenerator.Gui.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CssSpriteSheetGenerator.Gui.Tests.ViewModels
{
    [TestClass]
    public class MainWindowViewModelTests
    {
        private ISpriteSheetViewModel spriteSheetViewModel;
        private MainWindowViewModel mainWindowViewModel;

        [TestInitialize]
        public void Initialize()
        {
            spriteSheetViewModel = new SpriteSheetViewModel();
            mainWindowViewModel = new MainWindowViewModel(spriteSheetViewModel);
        }

        [TestMethod]
        public void ConstructorTest()
        {
            Assert.AreEqual(spriteSheetViewModel, mainWindowViewModel.SpriteSheetViewModel);
            Assert.AreEqual(spriteSheetViewModel.FileName, mainWindowViewModel.FileName);
        }

        [TestMethod]
        public void ExitCommand_SendsExitMessage()
        {
            var executed = false;
            Messenger.Default.Register<NotificationMessage>(this, m => { executed = true; });

            mainWindowViewModel.Exit.Execute(null);

            Assert.IsTrue(executed);
        }

        [TestMethod]
        public void ExitCommand_IfModifiedAndCancelsSave_CancelsExit()
        {
            Messenger.Default.Register<DialogMessage>(this, m => m.ProcessCallback(MessageBoxResult.Cancel));
            var cancelEventArgs = new CancelEventArgs();
            mainWindowViewModel.SpriteSheetViewModel.IsModified = true;

            mainWindowViewModel.Exit.Execute(cancelEventArgs);

            Assert.IsTrue(cancelEventArgs.Cancel);
        }

        [TestMethod]
        public void HelpCommand_SendsHelpMessage()
        {
            var executed = false;
            Messenger.Default.Register<NotificationMessage>(this, m => { executed = true; });
            Directory.CreateDirectory(Path.GetDirectoryName(Settings.Default.HelpFile));
            File.Create(Settings.Default.HelpFile);

            mainWindowViewModel.Help.Execute(null);

            Assert.IsTrue(executed);
        }

        [TestMethod]
        public void AboutCommand_SendsAboutMessage()
        {
            var executed = false;
            Messenger.Default.Register<NotificationMessage>(this, m => { executed = true; });

            mainWindowViewModel.About.Execute();

            Assert.IsTrue(executed);
        }

        [TestMethod]
        public void OptionsCommand_CanNotExecute()
        {
            var executed = false;
            Messenger.Default.Register<NotificationMessage>(this, m => { executed = true; });

            mainWindowViewModel.Options.Execute();

            Assert.IsFalse(executed);
            Assert.IsFalse(mainWindowViewModel.Options.CanExecute());
        }
    }
}
