using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace CssSpriteSheetGenerator.Models
{
    /// <summary>
    /// The base class for sprites and sprite sheets.
    /// </summary>
    [DataContract]
    public abstract class SpriteBase : ISpriteBase
    {
        /// <summary>
        /// A <see cref="System.Runtime.Serialization.ExtensionDataObject" /> that contains 
        /// data that is not recognized as belonging to the data contract.
        /// </summary>
        public ExtensionDataObject ExtensionData { get; set; }

        /// <summary>
        /// Identifies the <see cref="ClassName" /> property.
        /// </summary>
        public const string ClassNamePropertyName = "ClassName";
        private string _ClassName;
        /// <summary>
        /// The CSS class name for this element.
        /// </summary>
        [DataMember(Order = 0)]
        public string ClassName
        {
            get { return _ClassName ?? (_ClassName = string.Empty); }
            set
            {
                if (_ClassName == value)
                    return;
                OnPropertyChanging(ClassNamePropertyName, value);
                _ClassName = value;
                OnPropertyChanged(ClassNamePropertyName);
            }
        }

        /// <summary>
        /// Identifies the <see cref="X" /> property.
        /// </summary>
        public const string XPropertyName = "X";
        private int _X;
        /// <summary>
        /// The x-coordinate position of this element.
        /// </summary>
        [DataMember(Order = 1)]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "X")]
        public int X
        {
            get { return _X; }
            set
            {
                if (_X == value)
                    return;
                _X = value;
                OnPropertyChanged(XPropertyName);
            }
        }

        /// <summary>
        /// Identifies the <see cref="Y" /> property.
        /// </summary>
        public const string YPropertyName = "Y";
        private int _Y;
        /// <summary>
        /// The y-coordinate position of this element.
        /// </summary>
        [DataMember(Order = 2)]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Y")]
        public int Y
        {
            get { return _Y; }
            set
            {
                if (_Y == value)
                    return;
                _Y = value;
                OnPropertyChanged(YPropertyName);
            }
        }

        /// <summary>
        /// Identifies the <see cref="IsSelected" /> property.
        /// </summary>
        public const string IsSelectedPropertyName = "IsSelected";
        private bool _IsSelected;
        /// <summary>
        /// Indicates whether this sprite is selected or not.
        /// </summary>
        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                if (_IsSelected == value)
                    return;
                _IsSelected = value;
                OnPropertyChanged(IsSelectedPropertyName);
            }
        }

        /// <summary>
        /// Occurs when a property value is changing.
        /// </summary>
        public event PropertyChangingEventHandler PropertyChanging;

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the <see cref="PropertyChanging" /> event.
        /// </summary>
        /// <typeparam name="T">The type of the property whose value is changing.</typeparam>
        /// <param name="propertyName">The name of the property whose value is changing.</param>
        /// <param name="newValue">The new value of the property whose value is changing.</param>
        protected virtual void OnPropertyChanging<T>(string propertyName, T newValue)
        {
            if (PropertyChanging != null)
                PropertyChanging(this, new PropertyChangingEventArgs<T>(propertyName, newValue));
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged" /> event.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed value.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
