using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using CssSpriteSheetGenerator.Gui.Controls.Tools;
using CssSpriteSheetGenerator.Gui.ViewModels;

namespace CssSpriteSheetGenerator.Gui.Controls
{
    /// <summary>
    /// Represents a control that can handle drag and drop operations of images on its
    /// surface.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class DropTargetCanvas : Canvas
    {
        #region Fields

        private ISpriteSheetViewModel viewModel;                        // The view model
        private AdornerLayer adornerLayer;                              // The adorner layer
        private DropImageHereAdorner dropImageHereAdorner;              // The Drop Image Here overlay
        private TargetOverlayAdorner targetOverlayAdorner;              // The target overlay
        private RectangleSelectToolAdorner rectangleSelectToolAdorner;  // The Rectangle Select tool
        private ContentPresenter capturedImage;                         // The image captured during drag and drop operations
        private Vector captureOffset;                                   // The position of the mouse relative to the captured image on initial capture
        private Point capturePoint;                                     // The position of initial capture
        private bool hasMouseMovedDuringCapture;                        // Indicates if the mouse moved during capture

        #endregion

        #region Properties

        /// <summary>
        /// Identifies the <see cref="ShowDropImageHereAdorner" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowDropImageHereAdornerProperty = DependencyProperty.Register(
            "ShowDropImageHereAdorner",
            typeof(bool),
            typeof(DropTargetCanvas),
            new PropertyMetadata(
                new PropertyChangedCallback(OnShowDropImageHereAdornerChanged)));

        private static void OnShowDropImageHereAdornerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (DropTargetCanvas)d;
            obj.OnShowDropImageHereAdornerChanged((bool)e.OldValue, (bool)e.NewValue);
        }

        private void OnShowDropImageHereAdornerChanged(bool oldValue, bool newValue)
        {
            if (oldValue == newValue)
                return;

            if (adornerLayer == null)
                return;

            if (newValue)
            {
                if (!AdornerLayerContains(dropImageHereAdorner))
                    adornerLayer.Add(dropImageHereAdorner);
            }
            else
                adornerLayer.Remove(dropImageHereAdorner);
        }

        /// <summary>
        /// Used as a trigger to to display a <see cref="DropImageHereAdorner" />.
        /// </summary>
        [Description("")]
        [Category("Common")]
        public bool ShowDropImageHereAdorner
        {
            get { return (bool)GetValue(ShowDropImageHereAdornerProperty); }
            set { SetValue(ShowDropImageHereAdornerProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="IsRectangleSelectToolEnabled" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsRectangleSelectToolEnabledProperty = DependencyProperty.Register(
            "IsRectangleSelectToolEnabled",
            typeof(bool),
            typeof(DropTargetCanvas));

        /// <summary>
        /// Indicates if the Rectangle Select tool is enabled.
        /// </summary>
        [Description("Indicates if the Rectangle Select tool is enabled.")]
        [Category("Common")]
        public bool IsRectangleSelectToolEnabled
        {
            get { return (bool)GetValue(IsRectangleSelectToolEnabledProperty); }
            set { SetValue(IsRectangleSelectToolEnabledProperty, value); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes an instance of the <see cref="DropTargetCanvas" /> class.
        /// </summary>
        public DropTargetCanvas()
        {
            AllowDrop = true;
            ClipToBounds = true;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Sets other variables that could not be set in the constructor and initializes 
        /// the adorners.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> that contains the event data.</param>
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            Background = Background ?? Brushes.Transparent;

            viewModel = (ISpriteSheetViewModel)DataContext;
            SetBinding(IsRectangleSelectToolEnabledProperty, "IsRectangleSelectToolEnabled");
            SetBinding(ShowDropImageHereAdornerProperty, "IsEmpty");

            dropImageHereAdorner = new DropImageHereAdorner(this);
            targetOverlayAdorner = new TargetOverlayAdorner(this);
            rectangleSelectToolAdorner = new RectangleSelectToolAdorner(this);

            adornerLayer = AdornerLayer.GetAdornerLayer(this);
            adornerLayer.Add(dropImageHereAdorner);
        }

        /// <summary>
        /// Shows the target overlay when a drag and drop operation enters the element.
        /// </summary>
        /// <param name="e">The <see cref="DragEventArgs" /> that contains the event data.</param>
        protected override void OnDragEnter(DragEventArgs e)
        {
            ShowTargetOverlay();
        }

        /// <summary>
        /// Updates the target overlay when a drag and drop operation moves over the element.
        /// </summary>
        /// <param name="e">The <see cref="DragEventArgs" /> that contains the event data.</param>
        protected override void OnDragOver(DragEventArgs e)
        {
            UpdateTargetOverlay(e);
        }

        /// <summary>
        /// Hides the target overlay when a drag and drop operation leaves the element.
        /// </summary>
        /// <param name="e">The <see cref="DragEventArgs" /> that contains the event data.</param>
        protected override void OnDragLeave(DragEventArgs e)
        {
            HideTargetOverlay();
        }

        /// <summary>
        /// Hides the target overlay when a drag and drop operation drops on to the element.
        /// Creates a sprite sheet based on the data dropped.
        /// </summary>
        /// <param name="e">The <see cref="DragEventArgs" /> that contains the event data.</param>
        /// <exception cref="ArgumentNullException"><paramref name="e" /> cannot be null.</exception>
        protected override void OnDrop(DragEventArgs e)
        {
            HideTargetOverlay();

            if (e == null)
                throw new ArgumentNullException("e");

            var files = ((string[])e.Data.GetData(DataFormats.FileDrop));
            if (files != null)
            {
                double offset = 0;

                foreach (var fileName in files)
                {
                    var position = e.GetPosition(this);
                    position.Offset(0, offset);

                    var spriteSheet = viewModel.AddSpriteSheet(fileName, position);
                    offset += spriteSheet.Image.Height;
                }
            }
        }

        /// <summary>
        ///     <para>If an image is under the mouse cursor, capture the image and  show the target overlay.</para>
        ///     <para>If the Rectangle Select tool is enabled, show the Rectangle Select tool.</para>
        /// </summary>
        /// <param name="e">The <see cref="MouseButtonEventArgs" /> that contains the event data.</param>
        /// <exception cref="ArgumentNullException"><paramref name="e" /> cannot be null.</exception>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException("e");

            var image = Children.OfType<ContentPresenter>().SingleOrDefault(i => i.IsMouseOver == true);
            if (image != null)
            {
                capturedImage = image;
                capturedImage.CaptureMouse();
                captureOffset = (Vector)e.GetPosition(capturedImage);
                capturePoint = e.GetPosition(this);
                hasMouseMovedDuringCapture = false;

                ShowTargetOverlay();
                UpdatePosition(e);
            }
            else if (IsRectangleSelectToolEnabled)
            {
                rectangleSelectToolAdorner.BeginSelect(e.GetPosition(this));
                CaptureMouse();
                try
                {
                    adornerLayer.Add(rectangleSelectToolAdorner);
                }
                catch (ArgumentException ex)
                {
                    Debug.WriteLine(ex.Message);
                    throw;
                }
            }
        }

        /// <summary>
        ///     <para>If an image is captured, update its position.</para>
        ///     <para>If the Rectangle Select tool is enabled, update its position.</para>
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs" /> that contains the event data.</param>
        /// <exception cref="ArgumentNullException"><paramref name="e" /> cannot be null.</exception>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException("e");

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (capturedImage != null && capturedImage.IsMouseCaptured && capturedImage.IsMouseOver)
                {
                    if (e.GetPosition(this) != capturePoint)
                        hasMouseMovedDuringCapture = true;
                    UpdatePosition(e);
                }

                if (IsRectangleSelectToolEnabled && rectangleSelectToolAdorner.IsSelecting)
                    rectangleSelectToolAdorner.Destination = e.GetPosition(this);
            }
            else
            {
                adornerLayer.Remove(targetOverlayAdorner);
                adornerLayer.Remove(rectangleSelectToolAdorner);

                if (capturedImage != null)
                {
                    capturedImage.ReleaseMouseCapture();
                    capturedImage = null;
                    captureOffset = new Vector();
                }
            }
        }

        /// <summary>
        ///     <para>Hides the target overlay.</para>
        ///     <para>If an image is captured, release it.</para>
        ///     <para>If the Rectangle Select tool is enabled, add a sprite using its properties.</para>
        /// </summary>
        /// <param name="e">The <see cref="MouseButtonEventArgs" /> that contains the event data.</param>
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            HideTargetOverlay();

            if (capturedImage != null)
            {
                capturedImage.ReleaseMouseCapture();
                capturedImage = null;
                captureOffset = new Vector();
            }

            if (IsRectangleSelectToolEnabled)
            {
                viewModel.AddSprite("sprite", rectangleSelectToolAdorner.Rect);
                ReleaseMouseCapture();
                rectangleSelectToolAdorner.EndSelect();
                adornerLayer.Remove(rectangleSelectToolAdorner);
            }

            if (!hasMouseMovedDuringCapture)
            {
                var sprite = Children.OfType<ContentPresenter>().SingleOrDefault(i => i.IsMouseOver == true);
                if (sprite != null)
                {
                    var capturedSprite = sprite.FindVisualChild<SelfAdorningFrameworkElement>();
                    if (capturedSprite != null)
                        capturedSprite.IsSelected = true;
                }
            }
        }

        #endregion

        #region Private Methods

        // Indicates if the adorner layer contains the specified adorner
        private bool AdornerLayerContains(Adorner adorner)
        {
            if (adornerLayer.GetAdorners(this) != null)
                if (adornerLayer.GetAdorners(this).Contains(adorner))
                    return true;
            return false;
        }

        // Shows the target overlay
        private void ShowTargetOverlay()
        {
            adornerLayer.Add(targetOverlayAdorner);
        }

        // Updates the position of the target overlay
        private void UpdateTargetOverlay(DragEventArgs e)
        {
            UpdateTargetOverlay(e.GetPosition(this));
        }

        // Updates the position of the target overlay
        private void UpdateTargetOverlay(Point position)
        {
            targetOverlayAdorner.Origin = position;
        }

        // Hides the target overlay
        private void HideTargetOverlay()
        {
            adornerLayer.Remove(targetOverlayAdorner);
        }

        // Updates the position of the captured image and the target overlay
        private void UpdatePosition(MouseEventArgs e)
        {
            var capturedImageRenderSize = capturedImage != null ? (Vector)capturedImage.RenderSize : new Vector();
            var maxPosition = (Point)RenderSize - capturedImageRenderSize;
            var newPosition = (e.GetPosition(this) - captureOffset).Clamp(maxPosition);

            if (capturedImage != null)
            {
                Canvas.SetLeft(capturedImage, newPosition.X);
                Canvas.SetTop(capturedImage, newPosition.Y);
            }

            UpdateTargetOverlay(newPosition);
        }

        #endregion
    }
}
