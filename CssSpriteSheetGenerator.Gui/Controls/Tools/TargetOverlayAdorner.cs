using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace CssSpriteSheetGenerator.Gui.Controls.Tools
{
    /// <summary>
    /// Adorns an element with a target and text displaying the current coordinates of 
    /// the center of the target.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class TargetOverlayAdorner : Adorner
    {
        /// <summary>
        /// Identifies the <see cref="Origin" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty OriginProperty = DependencyProperty.Register(
            "Origin",
            typeof(Point),
            typeof(TargetOverlayAdorner),
            new FrameworkPropertyMetadata(
                new Point(),
                FrameworkPropertyMetadataOptions.AffectsRender,
                null,
                new CoerceValueCallback(CoerceOrigin)));

        private static object CoerceOrigin(DependencyObject d, object value)
        {
            return ((Point)value).Clamp(((TargetOverlayAdorner)d).RenderSize);
        }

        /// <summary>
        /// The point that the overlay is centered upon.
        /// </summary>
        [Description("The point that the overlay is centered upon.")]
        [Category("Common")]
        public Point Origin
        {
            get { return (Point)GetValue(OriginProperty); }
            set { SetValue(OriginProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="CoordinateTextOffset" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty CoordinateTextOffsetProperty = DependencyProperty.Register(
            "CoordinateTextOffset",
            typeof(Vector),
            typeof(TargetOverlayAdorner),
            new PropertyMetadata(
                new Vector(-5, -5)));

        /// <summary>
        /// The position offset of the coordinate text relative to the <see cref="Origin" />.
        /// </summary>
        [Description("The position offset of the coordinate text relative to the origin.")]
        [Category("Common")]
        public Vector CoordinateTextOffset
        {
            get { return (Vector)GetValue(CoordinateTextOffsetProperty); }
            set { SetValue(CoordinateTextOffsetProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="CoordinateTextBounds" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty CoordinateTextBoundsProperty = DependencyProperty.Register(
            "CoordinateTextBounds",
            typeof(Rect),
            typeof(TargetOverlayAdorner),
            new FrameworkPropertyMetadata(
                null,
                new CoerceValueCallback(CoerceCoordinateTextBounds)));

        private static object CoerceCoordinateTextBounds(DependencyObject d, object value)
        {
            var obj = (TargetOverlayAdorner)d;
            var bounds = (Rect)value;

            // Shift right
            if (bounds.Left < 0)
            {
                bounds.X += bounds.Width;
                bounds.X -= obj.CoordinateTextOffset.X;
                bounds.X -= obj.CoordinateTextOffset.X;
            }

            // Shift left
            if (bounds.Right > obj.Width)
            {
                bounds.X -= bounds.Width;
                bounds.X -= obj.CoordinateTextOffset.X;
                bounds.X -= obj.CoordinateTextOffset.X;
            }

            // Shift down
            if (bounds.Top < 0)
            {
                bounds.Y += bounds.Height;
                bounds.Y -= obj.CoordinateTextOffset.Y;
                bounds.Y -= obj.CoordinateTextOffset.Y;
            }

            // Shift up
            if (bounds.Bottom > obj.Height)
            {
                bounds.Y -= bounds.Height;
                bounds.Y -= obj.CoordinateTextOffset.Y;
                bounds.Y -= obj.CoordinateTextOffset.Y;
            }

            return bounds;
        }

        /// <summary>
        /// The bounding box of the coordinate text.
        /// </summary>
        [Description("The bounding box of the coordinate text.")]
        [Category("Common")]
        public Rect CoordinateTextBounds
        {
            get { return (Rect)GetValue(CoordinateTextBoundsProperty); }
            set { SetValue(CoordinateTextBoundsProperty, value); }
        }

        /// <summary>
        /// Initializes an instance of the <see cref="TargetOverlayAdorner" /> class.
        /// </summary>
        /// <param name="adornedElement">The element to be adorned.</param>
        public TargetOverlayAdorner(UIElement adornedElement)
            : base(adornedElement)
        {
            IsHitTestVisible = false;
        }

        /// <summary>
        /// Draws the overlay.
        /// </summary>
        /// <param name="drawingContext">The context to draw on.</param>
        /// <exception cref="ArgumentNullException"><paramref name="drawingContext" /> cannot be null.</exception>
        protected override void OnRender(DrawingContext drawingContext)
        {
            if (drawingContext == null)
                throw new ArgumentNullException("drawingContext");

            var blackPen = new Pen(Brushes.Black, 1);
            drawingContext.DrawLine(blackPen, new Point(0, Origin.Y), new Point(ActualWidth, Origin.Y));
            drawingContext.DrawLine(blackPen, new Point(Origin.X, 0), new Point(Origin.X, ActualHeight));

            var text = Helper.CreateFormattedText(string.Format(CultureInfo.InvariantCulture, "({0},{1})", (int)Origin.X, (int)Origin.Y));
            var textPosition = CalculateTextPosition(CoordinateTextOffset, text.Width, text.Height);
            CoordinateTextBounds = new Rect(textPosition, text.GetSize());
            drawingContext.DrawText(text, CoordinateTextBounds.Location);
        }

        // Calculates the offset position of the upper-left corner of the coordinate text
        private Point CalculateTextPosition(Vector offset, double textWidth, double textHeight)
        {
            if (offset.X < 0)
                textWidth = -textWidth;
            else
                textWidth = 0;

            if (offset.Y < 0)
                textHeight = -textHeight;
            else
                textHeight = 0;

            return Origin + offset + new Vector(textWidth, textHeight);
        }
    }
}
