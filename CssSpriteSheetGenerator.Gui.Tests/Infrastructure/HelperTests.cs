using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using CssSpriteSheetGenerator.Gui.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32;
using Drawing = System.Drawing;

namespace CssSpriteSheetGenerator.Gui.Tests.Infrastructure
{
    [TestClass]
    public class HelperTests
    {
        #region Extension Methods

        #region WPF Extension Methods

        #region BitmapImage Extensions

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ToDrawingBitmap_WithNullParameter_ThrowsException()
        {
            Helper.ToDrawingBitmap(null);
        }

        #endregion

        #region Dependency Object Extensions



        #endregion

        #region Point Extensions

        [TestMethod]
        public void ClampTest1()
        {
            var wpfPoint = new Point(1, 1);
            var max = new Point();

            var actual = wpfPoint.Clamp(max);

            Assert.AreEqual(new Point(), actual);
        }

        [TestMethod]
        public void ClampTest2()
        {
            var wpfPoint = new Point(1, 1);
            var max = new Size();

            var actual = wpfPoint.Clamp(max);

            Assert.AreEqual(new Point(), actual);
        }

        [TestMethod]
        public void OffsetTest1()
        {
            var wpfPoint = new Point();
            var offset = new Vector(1, 1);

            var actual = wpfPoint.Offset(offset);

            Assert.AreEqual(new Point(1, 1), actual);
        }

        [TestMethod]
        public void OffsetTest2()
        {
            var wpfPoint = new Point();
            var offset = new Size(1, 1);

            var actual = wpfPoint.Offset(offset);

            Assert.AreEqual(new Point(1, 1), actual);
        }

        [TestMethod]
        public void ToDrawingPointTest()
        {
            var wpfPoint = new Point();
            var drawingPoint = wpfPoint.ToDrawingPoint();

            Assert.AreEqual(new Drawing.Point(), drawingPoint);
        }

        [TestMethod]
        public void ToThicknessTest()
        {
            var wpfPoint = new Point();

            var actual = wpfPoint.ToThickness();

            Assert.AreEqual(new Thickness(), actual);
        }

        #endregion

        #region Rect Extensions

        [TestMethod]
        public void ToDrawingRectangleTest()
        {
            var wpfRect = new Rect();
            var drawingRect = wpfRect.ToDrawingRectangle();

            Assert.AreEqual(new Drawing.Rectangle(), drawingRect);
        }

        #endregion

        #region Size Extensions

        [TestMethod]
        public void ToDrawingSizeTest()
        {
            var wpfSize = new Size();

            var actual = wpfSize.ToDrawingSize();

            Assert.AreEqual(new Drawing.Size(), actual);
        }

        #endregion

        #region Thickness Extensions

        [TestMethod]
        public void SetLeftTest()
        {
            var thickness = new Thickness();

            var actual = thickness.SetLeft(1);

            Assert.AreEqual(new Thickness(1, 0, 0, 0), actual);
        }

        [TestMethod]
        public void SetTopTest()
        {
            var thickness = new Thickness();

            var actual = thickness.SetTop(1);

            Assert.AreEqual(new Thickness(0, 1, 0, 0), actual);
        }

        [TestMethod]
        public void SetRightTest()
        {
            var thickness = new Thickness();

            var actual = thickness.SetRight(1);

            Assert.AreEqual(new Thickness(0, 0, 1, 0), actual);
        }

        [TestMethod]
        public void SetBottomTest()
        {
            var thickness = new Thickness();

            var actual = thickness.SetBottom(1);

            Assert.AreEqual(new Thickness(0, 0, 0, 1), actual);
        }

        [TestMethod]
        public void ThicknessToPointTest()
        {
            var thickness = new Thickness();

            var actual = thickness.ToPoint();

            Assert.AreEqual(new Point(), actual);
        }

        #endregion

        #region UIElement Extensions



        #endregion

        #region Vector Extensions

        [TestMethod]
        public void FlipXTest()
        {
            var vector = new Vector(1, 1);

            var actual = vector.FlipX();

            Assert.AreEqual(new Vector(-1, 1), actual);
        }

        [TestMethod]
        public void FlipYTest()
        {
            var vector = new Vector(1, 1);

            var actual = vector.FlipY();

            Assert.AreEqual(new Vector(1, -1), actual);
        }

        #endregion

        #region Window Extensions

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExecuteDialog_WithNullCallbackParameter_ThrowsException()
        {
            var window = new Window();

            window.ExecuteDialog<OpenFileDialog>("", "", null);
        }

        #endregion

        #endregion

        #region System.Drawing Extension Methods

        #region Drawing.Point Extensions

        [TestMethod]
        public void DrawingPointToWpfPointTest()
        {
            var drawingPoint = new Drawing.Point();

            var actual = drawingPoint.ToWpfPoint();

            Assert.AreEqual(new Point(), actual);
        }

        #endregion

        #region Drawing.Rectangle Extensions

        [TestMethod]
        public void DrawingRectangleToWpfRectTest()
        {
            var drawingRectangle = new Drawing.Rectangle();

            var actual = drawingRectangle.ToWpfRect();

            Assert.AreEqual(new Rect(), actual);
        }

        #endregion

        #region Drawing.Size Extensions

        [TestMethod]
        public void DrawingSizeToWpfSizeTest()
        {
            var drawingSize = new Drawing.Size();

            var actual = drawingSize.ToWpfSize();

            Assert.AreEqual(new Size(), actual);
        }

        #endregion

        #endregion

        #region Miscellaneous Extension Methods

        #region double Extensions

        [TestMethod]
        public void DoubleToIntTest()
        {
            var d1 = 1.4;
            var d2 = 1.5;

            var actual1 = d1.ToInt();
            var actual2 = d2.ToInt();

            Assert.AreEqual(1, actual1);
            Assert.AreEqual(2, actual2);
        }

        #endregion

        #region Exception Extensions



        #endregion

        #region RelayCommand Extensions

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CanExecute_WithNullParameter_ThrowsException()
        {
            Helper.CanExecute(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Execute_WithNullParameter_ThrowsException()
        {
            Helper.Execute(null);
        }

        #endregion

        #region ISpriteSheetViewModel Extensions

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PromptSaveExecute_WithNullSpriteSheetViewModelParameter_ThrowsException()
        {
            Helper.PromptSaveExecute(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void PromptSaveExecute_WithNullExecuteParameter_ThrowsException()
        {
            ISpriteSheetViewModel iSpriteSheetViewModel = new SpriteSheetViewModel();

            iSpriteSheetViewModel.PromptSaveExecute(null);
        }

        [TestMethod]
        public void PromptSaveExecute_CallsExecute_IfIsNotModified()
        {
            ISpriteSheetViewModel iSpriteSheetViewModel = new SpriteSheetViewModel();
            var executed = false;

            iSpriteSheetViewModel.PromptSaveExecute(() => { executed = true; });

            Assert.IsTrue(executed);
        }

        [TestMethod]
        public void PromptSaveExecute_AnsweredYes_SavesAndExecutesExecute()
        {
            ISpriteSheetViewModel iSpriteSheetViewModel = new SpriteSheetViewModel();
            iSpriteSheetViewModel.IsModified = true;
            var executed = false;
            Messenger.Default.Register<DialogMessage>(this, m => { m.ProcessCallback(MessageBoxResult.Yes); });

            iSpriteSheetViewModel.PromptSaveExecute(() => { executed = true; });

            Assert.IsTrue(executed);
        }

        [TestMethod]
        public void PromptSaveExecute_AnsweredNo_ExecutesExecute()
        {
            ISpriteSheetViewModel iSpriteSheetViewModel = new SpriteSheetViewModel();
            iSpriteSheetViewModel.IsModified = true;
            var executed = false;
            Messenger.Default.Register<DialogMessage>(this, m => { m.ProcessCallback(MessageBoxResult.No); });

            iSpriteSheetViewModel.PromptSaveExecute(() => { executed = true; });

            Assert.IsTrue(executed);
        }

        [TestMethod]
        public void PromptSaveExecute_AnsweredCancel_ExecutesCancel()
        {
            ISpriteSheetViewModel iSpriteSheetViewModel = new SpriteSheetViewModel();
            iSpriteSheetViewModel.IsModified = true;
            var cancelled = false;
            Messenger.Default.Register<DialogMessage>(this, m => { m.ProcessCallback(MessageBoxResult.Cancel); });

            iSpriteSheetViewModel.PromptSaveExecute(() => { }, () => { cancelled = true; });

            Assert.IsTrue(cancelled);
        }

        #endregion

        #endregion

        #endregion

        #region Helper Methods

        #region General Purpose Helpers

        [TestMethod]
        public void Clamp_ValueInRange_ToValue()
        {
            var value = 2;
            var min = 1;
            var max = 3;

            var actual = Helper.Clamp(value, min, max);

            Assert.AreEqual(value, actual);
        }

        [TestMethod]
        public void Clamp_ValueLessThanMin_ToMin()
        {
            var value = 1;
            var min = 2;
            var max = 3;

            var actual = Helper.Clamp(value, min, max);

            Assert.AreEqual(min, actual);
        }

        [TestMethod]
        public void Clamp_ValueMoreThanMax_ToMax()
        {
            var value = 3;
            var min = 1;
            var max = 2;

            var actual = Helper.Clamp(value, min, max);

            Assert.AreEqual(max, actual);
        }
        #endregion

        #region Messenger Helpers

        [TestMethod]
        public void SendMessage_SendsMessage()
        {
            string notification = "NOTIFICATION";
            string actual = string.Empty;
            Messenger.Default.Register<NotificationMessage>(this, m => { actual = m.Notification; });

            Helper.SendMessage(notification);

            Assert.AreEqual(notification, actual);
        }

        #endregion

        #region FormattedText Helpers

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetSize_WithNullFormattedTextParameter_ThrowsException()
        {
            Helper.GetSize(null);
        }

        [TestMethod]
        public void GetSize_ReturnsFormattedTextSize()
        {
            var formattedText = new FormattedText("TEXT", CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface("Consolas"), 12, Brushes.Black);

            var actual = formattedText.GetSize();

            Assert.AreEqual(formattedText.Width, actual.Width);
            Assert.AreEqual(formattedText.Height, actual.Height);
        }

        [TestMethod]
        public void CreateFormattedTextTest()
        {
            var formattedText = Helper.CreateFormattedText("TEXT");

            Assert.AreEqual("TEXT", formattedText.Text);
        }

        #endregion

        #endregion
    }
}
