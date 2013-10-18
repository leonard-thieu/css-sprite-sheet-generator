using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using AppResources = CssSpriteSheetGenerator.Gui.Properties.Resources;

namespace CssSpriteSheetGenerator.Gui.Controls
{
    /// <summary>
    /// Displays an area indicating to drop an image there.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class DropImageHereAdorner : Adorner
    {
        /// <summary>
        /// Initializes an instance of the <see cref="DropImageHereAdorner" /> class.
        /// </summary>
        /// <param name="uiElement">The element to adorn.</param>
        public DropImageHereAdorner(UIElement uiElement)
            : base(uiElement)
        {
            IsHitTestVisible = false;
        }

        /// <summary>
        /// Renders the adorner.
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

            var background = Helper.Get<Brush>("#FFF5F5F5");
            var borderBrush = new Pen(Helper.Get<Brush>("#FFCCCCCC"), 1);
            var rectSize = new Size(0.8 * AdornedElement.RenderSize.Width, 115);
            var rectLocation = GetLocationThatWillCenter(rectSize);
            var rect = new Rect(rectLocation, rectSize);
            drawingContext.DrawRectangle(background, borderBrush, rect);

            var formattedText = Helper.CreateFormattedText(AppResources.DropImageHere, new Typeface("Arial"), 20, Helper.Get<Brush>("#FF777777"));
            formattedText.SetFontWeight(FontWeights.Bold);
            var textLocation = GetLocationThatWillCenter(formattedText.GetSize());
            drawingContext.DrawText(formattedText, textLocation);
        }

        // Gets the upper-left point that will center the element on the adorned element
        private Point GetLocationThatWillCenter(Size size)
        {
            var elementSize = AdornedElement.RenderSize;
            var centerX = (elementSize.Width - size.Width) / 2.0;
            var centerY = (elementSize.Height - size.Height) / 2.0;

            return new Point(centerX, centerY);
        }
    }
}
