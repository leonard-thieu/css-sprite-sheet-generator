using System.ComponentModel;

namespace CssSpriteSheetGenerator.Models
{
    /// <summary>
    /// Provides data for the <see cref="INotifyPropertyChanging.PropertyChanging" /> event.
    /// </summary>
    /// <typeparam name="T">The type of the property whose value is changing.</typeparam>
    public class PropertyChangingEventArgs<T> : PropertyChangingEventArgs
    {
        /// <summary>
        /// The new value of the property whose value is changing.
        /// </summary>
        public T NewValue { get; private set; }

        /// <summary>
        /// Initializes an instance of the <see cref="PropertyChangingEventArgs" /> class.
        /// </summary>
        /// <param name="propertyName">The name of the property whose value is changing.</param>
        /// <param name="newValue">The new value of the property whose value is changing.</param>
        public PropertyChangingEventArgs(string propertyName, T newValue)
            : base(propertyName)
        {
            NewValue = newValue;
        }
    }
}
