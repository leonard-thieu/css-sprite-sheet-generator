using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace CssSpriteSheetGenerator.Gui.Controls.Tools
{
    /// <summary>
    /// An adorner that allows for interactive resizing of an element.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ResizingAdorner : Adorner
    {
        private FrameworkElement adornedElement;    // The adorned element
        private VisualCollection visualChildren;    // Collection of Visuals for this adorner

        // Edge thumbs
        private Thumb grabLeft;                     // The left thumb
        private Thumb grabRight;                    // The right thumb
        private Thumb grabTop;                      // The top thumb
        private Thumb grabBottom;                   // The bottom thumb

        // Corner thumbs
        private Thumb grabTopLeft;                  // The top left thumb
        private Thumb grabTopRight;                 // The top right thumb
        private Thumb grabBottomLeft;               // The bottom left thumb
        private Thumb grabBottomRight;              // The bottom right thumb

        /// <summary>
        /// Identifies the <see cref="Stroke" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(
            "Stroke",
            typeof(Pen),
            typeof(ResizingAdorner),
            new FrameworkPropertyMetadata(
                new Pen(Brushes.Black, 1),
                FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// The <see cref="Pen" /> to stroke the rectangle with.
        /// </summary>
        [Description("The pen to stroke the rectangle with.")]
        [Category("Common")]
        public Pen Stroke
        {
            get { return (Pen)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Fill" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty FillProperty = DependencyProperty.Register(
            "Fill",
            typeof(Brush),
            typeof(ResizingAdorner),
            new FrameworkPropertyMetadata(
                Helper.Get<Brush>("#2C007CFF"),
                FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// The <see cref="Brush" /> to fill the rectangle with.
        /// </summary>
        [Description("The brush to fill the rectangle with.")]
        [Category("Common")]
        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ThumbSize" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty ThumbSizeProperty = DependencyProperty.Register(
            "ThumbSize",
            typeof(double),
            typeof(ResizingAdorner),
            new FrameworkPropertyMetadata(
                10.0,
                FrameworkPropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// The size of the thumbs. This should be sufficiently large to make it easy to 
        /// grab.
        /// </summary>
        [Description("The size of the thumbs. This should be sufficiently large to make it easy to grab.")]
        [Category("Common")]
        public double ThumbSize
        {
            get { return (double)GetValue(ThumbSizeProperty); }
            set { SetValue(ThumbSizeProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="IndicatorSize" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty IndicatorSizeProperty = DependencyProperty.Register(
            "IndicatorSize",
            typeof(Size),
            typeof(ResizingAdorner),
            new FrameworkPropertyMetadata(
                new Size(7, 7),
                FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// The size of the indicator elements that appear on each edge and corner of the 
        /// adorner.
        /// </summary>
        [Description("The size of the indicator elements that appear on each edge and corner of the adorner.")]
        [Category("Common")]
        public Size IndicatorSize
        {
            get { return (Size)GetValue(IndicatorSizeProperty); }
            set { SetValue(IndicatorSizeProperty, value); }
        }

        /// <summary>
        /// The number of visual child elements for this element.
        /// </summary>
        protected override int VisualChildrenCount { get { return visualChildren.Count; } }

        /// <summary>
        /// Initializes an instance of the <see cref="ResizingAdorner" /> class.
        /// </summary>
        /// <param name="adornedElement">The element to adorn.</param>
        /// <exception cref="ArgumentNullException"><paramref name="adornedElement" /> cannot be null.</exception>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        public ResizingAdorner(FrameworkElement adornedElement)
            : base(adornedElement)
        {
            this.adornedElement = adornedElement;
            visualChildren = new VisualCollection(this);

            // Initialize thumbs
            GimmeThumb(out grabLeft, Cursors.ScrollWE, ThumbSize, adornedElement.Height);
            GimmeThumb(out grabRight, Cursors.ScrollWE, ThumbSize, adornedElement.Height);
            GimmeThumb(out grabTop, Cursors.ScrollNS, adornedElement.Width, ThumbSize);
            GimmeThumb(out grabBottom, Cursors.ScrollNS, adornedElement.Width, ThumbSize);
            GimmeThumb(out grabTopLeft, Cursors.SizeNWSE, ThumbSize, ThumbSize);
            GimmeThumb(out grabTopRight, Cursors.SizeNESW, ThumbSize, ThumbSize);
            GimmeThumb(out grabBottomLeft, Cursors.SizeNESW, ThumbSize, ThumbSize);
            GimmeThumb(out grabBottomRight, Cursors.SizeNWSE, ThumbSize, ThumbSize);

            // Add handlers
            grabLeft.DragDelta += new DragDeltaEventHandler(OnGrabLeftDragDelta);
            grabRight.DragDelta += new DragDeltaEventHandler(OnGrabRightDragDelta);
            grabTop.DragDelta += new DragDeltaEventHandler(OnGrabTopDragDelta);
            grabBottom.DragDelta += new DragDeltaEventHandler(OnGrabBottomDragDelta);
            grabTopLeft.DragDelta += new DragDeltaEventHandler(OnGrabTopLeftDragDelta);
            grabTopRight.DragDelta += new DragDeltaEventHandler(OnGrabTopRightDragDelta);
            grabBottomLeft.DragDelta += new DragDeltaEventHandler(OnGrabBottomLeftDragDelta);
            grabBottomRight.DragDelta += new DragDeltaEventHandler(OnGrabBottomRightDragDelta);
        }

        /// <summary>
        /// Returns a child at the specified index from a collection of child elements.
        /// </summary>
        /// <param name="index">The zero-based index of the requested child element in the collection.</param>
        /// <returns>
        /// The requested child element. This should not return null; if the provided index 
        /// is out of range, an exception is thrown.
        /// </returns>
        protected override Visual GetVisualChild(int index)
        {
            return visualChildren[index];
        }

        /// <summary>
        /// Positions child elements and determines a size.
        /// </summary>
        /// <param name="finalSize">
        /// The final area within the parent that this element should use to arrange itself 
        /// and its children.
        /// </param>
        /// <returns>The actual size used.</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            // Compute inner width and height values
            var width = finalSize.Width - grabLeft.Width / 2.0 - grabRight.Width / 2.0;
            var height = finalSize.Height - grabTop.Height / 2.0 - grabBottom.Height / 2.0;

            // Inner width and height must be non-negative
            width = width >= 0 ? width : 0;
            height = height >= 0 ? height : 0;

            // Resize edge thumbs
            grabLeft.Height = height;
            grabRight.Height = height;
            grabTop.Width = width;
            grabBottom.Width = width;

            // Compute final sizes
            var left = new Rect(-(grabLeft.Width / 2.0), grabTop.Height / 2.0, grabLeft.Width, height);
            var right = new Rect(finalSize.Width - (grabRight.Width / 2.0), grabTop.Height / 2.0, grabRight.Width, height);
            var top = new Rect(grabLeft.Width / 2.0, -(grabTop.Height / 2.0), width, grabTop.Height);
            var bottom = new Rect(grabLeft.Width / 2.0, finalSize.Height - (grabBottom.Height / 2.0), width, grabBottom.Height);
            var topLeft = new Rect(-(grabTopLeft.Width / 2.0), -(grabTopLeft.Height / 2.0), grabTopLeft.Width, grabTopLeft.Height);
            var topRight = new Rect(finalSize.Width - (grabTopRight.Width / 2.0), -(grabTopRight.Height / 2.0), grabTopRight.Width, grabTopRight.Height);
            var bottomLeft = new Rect(-(grabBottomLeft.Width / 2.0), finalSize.Height - (grabBottomLeft.Height / 2.0), grabBottomLeft.Width, grabBottomLeft.Height);
            var bottomRight = new Rect(finalSize.Width - (grabBottomRight.Width / 2.0), finalSize.Height - (grabBottomRight.Height / 2.0), grabBottomRight.Width, grabBottomRight.Height);

            // Arrange children
            grabLeft.Arrange(left);
            grabRight.Arrange(right);
            grabTop.Arrange(top);
            grabBottom.Arrange(bottom);
            grabTopLeft.Arrange(topLeft);
            grabTopRight.Arrange(topRight);
            grabBottomLeft.Arrange(bottomLeft);
            grabBottomRight.Arrange(bottomRight);

            return finalSize;
        }

        /// <summary>
        /// Renders a selection rectangle with indicators.
        /// </summary>
        /// <param name="drawingContext">
        /// The drawing instructions for a specific element. This context is provided to the
        /// layout system.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="drawingContext" /> cannot be null.</exception>
        protected override void OnRender(DrawingContext drawingContext)
        {
            if (drawingContext == null)
                throw new ArgumentNullException("drawingContext");

            var indicatorOffsetX = (0.5 * IndicatorSize.Width);
            var indicatorOffsetY = (0.5 * IndicatorSize.Height);
            var leftX = (0.0 * ActualWidth) - indicatorOffsetX;
            var centerX = (0.5 * ActualWidth) - indicatorOffsetX;
            var rightX = (1.0 * ActualWidth) - indicatorOffsetX;
            var topY = (0.0 * ActualHeight) - indicatorOffsetY;
            var centerY = (0.5 * ActualHeight) - indicatorOffsetY;
            var bottomY = (1.0 * ActualHeight) - indicatorOffsetY;

            drawingContext.DrawRectangle(Fill, Stroke, new Rect(RenderSize)); // Background

            DrawIndicator(drawingContext, new Point(leftX, centerY), Shape.Rectangle);      // Left
            DrawIndicator(drawingContext, new Point(centerX, topY), Shape.Rectangle);       // Top
            DrawIndicator(drawingContext, new Point(rightX, centerY), Shape.Rectangle);     // Right
            DrawIndicator(drawingContext, new Point(centerX, bottomY), Shape.Rectangle);    // Bottom
            DrawIndicator(drawingContext, new Point(leftX, topY), Shape.Ellipse);           // Top Left
            DrawIndicator(drawingContext, new Point(rightX, topY), Shape.Ellipse);          // Top Right
            DrawIndicator(drawingContext, new Point(leftX, bottomY), Shape.Ellipse);        // Bottom Left
            DrawIndicator(drawingContext, new Point(rightX, bottomY), Shape.Ellipse);       // Bottom Right
        }

        // Initializes a Thumb
        private void GimmeThumb(out Thumb thumb, Cursor cursor, double width, double height)
        {
            thumb = new Thumb
            {
                Cursor = cursor,
                Width = width,
                Height = height,
                Background = Brushes.Transparent,
                OpacityMask = Brushes.Transparent
            };

            visualChildren.Add(thumb);
        }

        // Draws resize indicators
        private void DrawIndicator(DrawingContext drawingContext, Point point, Shape shape)
        {
            var rectangle = new Rect(point, IndicatorSize);
            switch (shape)
            {
                case Shape.Ellipse:
                    var ellipse = new EllipseGeometry(rectangle);
                    drawingContext.DrawEllipse(Brushes.Transparent, Stroke, ellipse.Center, ellipse.RadiusX, ellipse.RadiusY);
                    break;
                case Shape.Rectangle:
                    drawingContext.DrawRectangle(Brushes.Transparent, Stroke, rectangle);
                    break;
                default:
                    throw new NotSupportedException(string.Format(CultureInfo.CurrentUICulture, "This method does not support drawing shapes of type '{0}'.", shape));
            }
        }

        // Handler for left thumb
        private void OnGrabLeftDragDelta(object sender, DragDeltaEventArgs e)
        {
            var width = adornedElement.Width - e.HorizontalChange;
            if (width > 0)
            {
                var margin = adornedElement.Margin;
                adornedElement.Margin = margin.SetLeft(margin.Left + e.HorizontalChange);
                adornedElement.Width -= e.HorizontalChange;
            }
        }

        // Handler for right thumb
        private void OnGrabRightDragDelta(object sender, DragDeltaEventArgs e)
        {
            var width = adornedElement.Width + e.HorizontalChange;
            if (width > 0)
                adornedElement.Width = width;
        }

        // Handler for top thumb
        private void OnGrabTopDragDelta(object sender, DragDeltaEventArgs e)
        {
            var height = adornedElement.Height - e.VerticalChange;
            if (height > 0)
            {
                var margin = adornedElement.Margin;
                var newThickness = margin.SetTop(margin.Top + e.VerticalChange);
                adornedElement.Margin = newThickness;
                adornedElement.Height -= e.VerticalChange;
            }
        }

        // Handler for bottom thumb
        private void OnGrabBottomDragDelta(object sender, DragDeltaEventArgs e)
        {
            var height = adornedElement.Height + e.VerticalChange;
            if (height > 0)
                adornedElement.Height = height;
        }

        // Handler for top left thumb
        private void OnGrabTopLeftDragDelta(object sender, DragDeltaEventArgs e)
        {
            OnGrabTopDragDelta(sender, e);
            OnGrabLeftDragDelta(sender, e);
        }

        // Handler for top right thumb
        private void OnGrabTopRightDragDelta(object sender, DragDeltaEventArgs e)
        {
            OnGrabTopDragDelta(sender, e);
            OnGrabRightDragDelta(sender, e);
        }

        // Handler for bottom left thumb
        private void OnGrabBottomLeftDragDelta(object sender, DragDeltaEventArgs e)
        {
            OnGrabBottomDragDelta(sender, e);
            OnGrabLeftDragDelta(sender, e);
        }

        // Handler for bottom right thumb
        private void OnGrabBottomRightDragDelta(object sender, DragDeltaEventArgs e)
        {
            OnGrabBottomDragDelta(sender, e);
            OnGrabRightDragDelta(sender, e);
        }

        private enum Shape
        {
            Drawing,
            Ellipse,
            Geometry,
            GlyphRun,
            Image,
            Line,
            Rectangle,
            RoundedRectangle,
            Text,
            Video
        }
    }
}
