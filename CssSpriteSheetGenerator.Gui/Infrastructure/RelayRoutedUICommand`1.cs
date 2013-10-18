using System;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace CssSpriteSheetGenerator.Gui.Infrastructure
{
    /// <summary>
    /// A <see cref="RelayCommand" /> that defines descriptive text and input gestures.
    /// </summary>
    /// <typeparam name="T">The type of the command parameter.</typeparam>
    public class RelayRoutedUICommand<T> : RelayCommand<T>, IRelayRoutedUICommand
    {
        private InputGestureCollection _InputGestures;

        /// <summary>
        /// The text that describes the command. The default is an empty string.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The input gestures.
        /// </summary>
        public InputGestureCollection InputGestures
        {
            get { return _InputGestures; }
        }

        /// <summary>
        /// Initializes an instance of the <see cref="RelayRoutedUICommand" /> class.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="text">Descriptive text for the command.</param>
        /// <param name="inputGestures">A collection of gestures to associate with the command.</param>
        /// <exception cref="ArgumentNullException">If the execute argument is null.</exception>
        public RelayRoutedUICommand(Action<T> execute, string text, InputGestureCollection inputGestures)
            : base(execute)
        {
            Text = text;
            _InputGestures = inputGestures;
        }

        /// <summary>
        /// Initializes an instance of the <see cref="RelayRoutedUICommand" /> class.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        /// <param name="text">Descriptive text for the command.</param>
        /// <param name="inputGestures">A collection of gestures to associate with the command.</param>
        /// <exception cref="ArgumentNullException">If the execute argument is null.</exception>
        public RelayRoutedUICommand(Action<T> execute, Func<T, bool> canExecute, string text, InputGestureCollection inputGestures)
            : base(execute, canExecute)
        {
            Text = text;
            _InputGestures = inputGestures;
        }
    }
}
