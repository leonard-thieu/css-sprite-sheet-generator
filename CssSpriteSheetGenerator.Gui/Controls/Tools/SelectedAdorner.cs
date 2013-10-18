using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CssSpriteSheetGenerator.Gui.Controls.Tools
{
    /// <summary>
    /// Displays an indication that an item is selected.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class SelectedAdorner : Adorner
    {
        /// <summary>
        /// Identifies the <see cref="Stroke" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(
            "Stroke",
            typeof(Pen),
            typeof(SelectedAdorner),
            new FrameworkPropertyMetadata(
                new Pen(Brushes.Black, 1) { DashStyle = new DashStyle(new double[] { 4, 4 }, 0) },
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
        /// Initializes an instance of the <see cref="SelectedAdorner" /> class.
        /// </summary>
        /// <param name="uiElement">The element to be adorned.</param>
        public SelectedAdorner(UIElement uiElement)
            : base(uiElement)
        {
            IsHitTestVisible = false;
            Loaded += new RoutedEventHandler(OnSpriteAdornerLoaded);
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

        // Animates the selection border
        private void OnSpriteAdornerLoaded(object sender, RoutedEventArgs e)
        {
            var animation = new DoubleAnimation
            {
                From = Stroke.DashStyle.Dashes.Sum(),
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(0.5)),
                RepeatBehavior = RepeatBehavior.Forever
            };

            var storyboard = new Storyboard();
            storyboard.Children.Add(animation);
            Storyboard.SetTarget(storyboard, this);
            Storyboard.SetTargetProperty(animation, new PropertyPath("Stroke.DashStyle.Offset"));

            storyboard.Begin(this);
        }
    }
}
