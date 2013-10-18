using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CssSpriteSheetGenerator.Gui.Infrastructure;

namespace CssSpriteSheetGenerator.Gui.Controls
{
    /// <summary>
    /// A <see cref="MenuItem" /> that takes advantage of the additional data made 
    /// available on <see cref="RelayRoutedUICommand" /> to display <see cref="HeaderedItemsControl.Header" />
    /// and <see cref="MenuItem.InputGestureText" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class RelayMenuItem : MenuItem
    {
        // Overrides metadata
        static RelayMenuItem()
        {
            HeaderProperty.OverrideMetadata(
                typeof(RelayMenuItem),
                new FrameworkPropertyMetadata(
                    null,
                    new CoerceValueCallback(CoerceHeader)));
            InputGestureTextProperty.OverrideMetadata(
                typeof(RelayMenuItem),
                new FrameworkPropertyMetadata(
                    string.Empty,
                    null,
                    new CoerceValueCallback(CoerceInputGestureText)));
        }

        // Display text for the header using the command's text if it has not been set
        private static object CoerceHeader(DependencyObject d, object value)
        {
            var menuItem = (MenuItem)d;
            IRelayRoutedUICommand uiCommand;

            // If no header has been set, use the command's text
            if (value == null)
            {
                uiCommand = menuItem.Command as IRelayRoutedUICommand;
                if (uiCommand != null)
                    value = uiCommand.Text;
                return value;
            }

            // If the header had been set to a UICommand by the ItemsControl, replace it with the command's text 
            uiCommand = value as IRelayRoutedUICommand;

            if (uiCommand != null)
            {
                // The header is equal to the command.
                // If this MenuItem was generated for the command, then go ahead and overwrite the header 
                // since the generator automatically set the header.
                var parent = ItemsControl.ItemsControlFromItemContainer(menuItem);
                if (parent != null)
                {
                    var originalItem = parent.ItemContainerGenerator.ItemFromContainer(menuItem);

                    if (originalItem == value)
                        return uiCommand.Text;
                }
            }

            return value;
        }

        // Display text for the input gesture using the command's text if it has not been set
        private static object CoerceInputGestureText(DependencyObject d, object value)
        {
            var menuItem = (MenuItem)d;
            IRelayRoutedUICommand routedCommand;

            if (string.IsNullOrEmpty((string)value)
                && (routedCommand = menuItem.Command as IRelayRoutedUICommand) != null
                && routedCommand != null)
            {
                var col = routedCommand.InputGestures;
                if ((col != null) && (col.Count >= 1))
                {
                    // Search for the first key gesture 
                    for (int i = 0; i < col.Count; i++)
                    {
                        var keyGesture = ((IList)col)[i] as KeyGesture;
                        if (keyGesture != null)
                            return keyGesture.GetDisplayStringForCulture(CultureInfo.CurrentCulture);
                    }
                }
            }

            return value;
        }
    }
}
