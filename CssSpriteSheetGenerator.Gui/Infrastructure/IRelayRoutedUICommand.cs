using System.Windows.Input;

namespace CssSpriteSheetGenerator.Gui.Infrastructure
{
    /// <summary>
    /// Defines descriptive text and input gestures for a command.
    /// </summary>
    public interface IRelayRoutedUICommand
    {
        /// <summary>
        /// The text that describes the command. The default is an empty string.
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// The input gestures.
        /// </summary>
        InputGestureCollection InputGestures { get; }
    }
}
