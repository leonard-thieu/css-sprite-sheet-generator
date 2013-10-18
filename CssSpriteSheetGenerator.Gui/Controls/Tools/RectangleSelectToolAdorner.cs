using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace CssSpriteSheetGenerator.Gui.Controls.Tools
{
    /// <summary>
    /// The Rectangle Select tool. Draws a rectangular selection on to the adorned 
    /// element.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class RectangleSelectToolAdorner : Adorner
    {
        /// <summary>
        /// Indicates if the tool is currently selecting.
        /// </summary>
        public bool IsSelecting { get; protected set; }

        /// <summary>
        /// Identifies the <see cref="Stroke" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(
            "Stroke",
            typeof(Pen),
            typeof(RectangleSelectToolAdorner),
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
            typeof(RectangleSelectToolAdorner),
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
        /// Identifies the <see cref="Origin" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty OriginProperty = DependencyProperty.Register(
            "Origin",
            typeof(Point),
            typeof(RectangleSelectToolAdorner));

        /// <summary>
        /// The point where the Rectangle Select tool begins selecting at.
        /// </summary>
        [Description("The point where the Rectangle Select tool begins selecting at.")]
        [Category("Common")]
        public Point Origin
        {
            get { return (Point)GetValue(OriginProperty); }
            set { SetValue(OriginProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Destination" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty DestinationProperty = DependencyProperty.Register(
            "Destination",
            typeof(Point),
            typeof(RectangleSelectToolAdorner),
            new FrameworkPropertyMetadata(
                new PropertyChangedCallback(OnDestinationChanged),
                new CoerceValueCallback(CoerceDestination)));

        // Called when Destination changes
        private static void OnDestinationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var selectionRectangle = (RectangleSelectToolAdorner)d;
            selectionRectangle.OnDestinationChanged((Point)e.OldValue, (Point)e.NewValue);
        }

        // Called when Destination changes. Contains the logic that allows the Rectangle 
        // Select tool to pivot about Origin as Destination changes.
        private void OnDestinationChanged(Point oldValue, Point newValue)
        {
            if (oldValue == newValue)
                return;

            if (!IsSelecting)
                return;

            var newSize = newValue - Origin;

            double left = Origin.X;
            double top = Origin.Y;
            double width = newSize.X;
            double height = newSize.Y;

            if (width < 0)
            {
                left += newSize.X;
                width = Math.Abs(width);
            }

            if (height < 0)
            {
                top += newSize.Y;
                height = Math.Abs(height);
            }

            var rect = Rect;
            rect.Location = new Point(left, top);
            rect.Width = width;
            rect.Height = height;
            Rect = rect;
        }

        // Clamps the Rectangle Select tool within the bounds of the adorned element.
        private static object CoerceDestination(DependencyObject d, object baseValue)
        {
            return ((Point)baseValue).Clamp(((RectangleSelectToolAdorner)d).RenderSize);
        }

        /// <summary>
        /// The point that the mouse has dragged to.
        /// </summary>
        [Description("The point that the mouse has dragged to.")]
        [Category("Common")]
        public Point Destination
        {
            get { return (Point)GetValue(DestinationProperty); }
            set { SetValue(DestinationProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Rect" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty RectProperty = DependencyProperty.Register(
            "Rect",
            typeof(Rect),
            typeof(RectangleSelectToolAdorner),
            new FrameworkPropertyMetadata(
                new Rect(),
                FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// The position and bounds of the Rectangle Select tool.
        /// </summary>
        [Description("The position and bounds of the Rectangle Select tool.")]
        [Category("Common")]
        public Rect Rect
        {
            get { return (Rect)GetValue(RectProperty); }
            protected set { SetValue(RectProperty, value); }
        }

        /// <summary>
        /// Initializes an instance of the <see cref="RectangleSelectToolAdorner" /> class.
        /// </summary>
        /// <param name="adornedElement"></param>
        public RectangleSelectToolAdorner(UIElement adornedElement)
            : base(adornedElement)
        {
            IsHitTestVisible = false;
        }

        /// <summary>
        /// Displays the selection rectangle and will size it according to
        /// <see cref="Origin" /> and <see cref="Destination" /> until <see cref="EndSelect" /> is called.
        /// </summary>
        /// <param name="origin">The point that the selection rectangle will pivot about when resizing.</param>
        public void BeginSelect(Point origin)
        {
            IsSelecting = true;
            Origin = origin;
            Destination = origin; // Why doesn't this call its PropertyChangedCallback?
            OnDestinationChanged(new Point(), origin);
        }

        /// <summary>
        /// Locks the collection into its current state.
        /// </summary>
        public void EndSelect()
        {
            IsSelecting = false;
        }

        /// <summary>
        /// Draws a rectangular selection on to the adorned element.
        /// </summary>
        /// <param name="drawingContext">The context to draw on.</param>
        /// <exception cref="ArgumentNullException"><paramref name="drawingContext" /> cannot be null.</exception>
        protected override void OnRender(DrawingContext drawingContext)
        {
            if (drawingContext == null)
                throw new ArgumentNullException("drawingContext");

            drawingContext.DrawRectangle(Fill, Stroke, Rect);
        }
    }
}
