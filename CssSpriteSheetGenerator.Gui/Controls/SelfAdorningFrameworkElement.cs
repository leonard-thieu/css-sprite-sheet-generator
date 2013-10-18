using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Documents;
using CssSpriteSheetGenerator.Gui.Controls.Tools;

namespace CssSpriteSheetGenerator.Gui.Controls
{
    /// <summary>
    /// An element that can be initialized with an adorner.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class SelfAdorningFrameworkElement : FrameworkElement, IDisposable
    {
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
        static SelfAdorningFrameworkElement()
        {
            WidthProperty.OverrideMetadata(
                typeof(SelfAdorningFrameworkElement),
                new FrameworkPropertyMetadata { BindsTwoWayByDefault = true });
            HeightProperty.OverrideMetadata(
                typeof(SelfAdorningFrameworkElement),
                new FrameworkPropertyMetadata { BindsTwoWayByDefault = true });
            MarginProperty.OverrideMetadata(
                typeof(SelfAdorningFrameworkElement),
                new FrameworkPropertyMetadata { BindsTwoWayByDefault = true });
        }

        private AdornerLayer adornerLayer; // The adorner layer for this element

        /// <summary>
        /// Identifies the <see cref="IsSelected" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
            "IsSelected",
            typeof(bool),
            typeof(SelfAdorningFrameworkElement),
            new FrameworkPropertyMetadata(
                new PropertyChangedCallback(OnIsSelectedChanged)));

        private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (SelfAdorningFrameworkElement)d;
            obj.OnIsSelectedChanged((bool)e.OldValue, (bool)e.NewValue);
        }

        private void OnIsSelectedChanged(bool oldValue, bool newValue)
        {
            if (oldValue == newValue)
                return;

            if (newValue)
                Adorner = new SelectedAdorner(this);
            else
                Adorner = new SpriteAdorner(this);
        }

        /// <summary>
        ///     <para>If true, sets <see cref="Adorner" /> to <see cref="SelectedAdorner" />.</para>
        ///     <para>If false, sets <see cref="Adorner" /> to <see cref="SpriteAdorner" />.</para>
        /// </summary>
        [Description("If true, sets Adorner to SelectedAdorner. If false, sets Adorner to SpriteAdorner.")]
        [Category("Common")]
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Adorner" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty AdornerProperty = DependencyProperty.Register(
            "Adorner",
            typeof(Adorner),
            typeof(SelfAdorningFrameworkElement),
            new PropertyMetadata(
                new PropertyChangedCallback(OnAdornerChanged)));

        private static void OnAdornerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (SelfAdorningFrameworkElement)d;
            obj.OnAdornerChanged((Adorner)e.OldValue, (Adorner)e.NewValue);
        }

        private void OnAdornerChanged(Adorner oldValue, Adorner newValue)
        {
            if (oldValue == newValue)
                return;

            if (adornerLayer != null)
            {
                this.ClearAdorners();
                adornerLayer.Add(newValue);
            }
        }

        /// <summary>
        /// The <see cref="Adorner" /> to adorn this <see cref="SelfAdorningFrameworkElement" /> with.
        /// </summary>
        [Description("The adorner to adorn this element with.")]
        [Category("Common")]
        public Adorner Adorner
        {
            get { return (Adorner)GetValue(AdornerProperty); }
            set { SetValue(AdornerProperty, value); }
        }

        /// <summary>
        /// Adorns this element with the adorner specified in <see cref="Adorner" />.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> that contains the event data.</param>
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            if (Adorner == null)
                Adorner = new SpriteAdorner(this);

            adornerLayer = AdornerLayer.GetAdornerLayer(this);
            try
            {
                adornerLayer.Add(Adorner);
            }
            catch (NullReferenceException ex)
            {
                ex.LogAndSendExceptionNotification();
            }
        }

        /// <summary>
        /// Removes the adorner from the adorner layer.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Removes the adorner from the adorner layer.
        /// </summary>
        /// <param name="disposing">
        /// If true, disposes of managed and unmanaged resources. If false, only disposes of
        /// unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                if (adornerLayer != null)
                    adornerLayer.Remove(Adorner);
        }
    }
}
