using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace CssSpriteSheetGenerator.Gui.Controls.Tools
{
    /// <summary>
    /// A basic adorner for sprites to indicate their position and size.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class SpriteAdorner : Adorner
    {
        /// <summary>
        /// Identifies the <see cref="Stroke" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(
            "Stroke",
            typeof(Pen),
            typeof(SpriteAdorner),
            new FrameworkPropertyMetadata(
                new Pen(Brushes.Black, 1.5),
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
        /// Initializes an instance of the <see cref="SpriteAdorner" /> class.
        /// </summary>
        /// <param name="uiElement">The element to adorn.</param>
        public SpriteAdorner(UIElement uiElement)
            : base(uiElement)
        {
            IsHitTestVisible = false;
        }

        /// <summary>
        /// Draws a rectangle with a stroke and transparent fill on the adorned element.
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

            drawingContext.DrawRectangle(Brushes.Transparent, Stroke, new Rect(RenderSize));
        }
    }
}
