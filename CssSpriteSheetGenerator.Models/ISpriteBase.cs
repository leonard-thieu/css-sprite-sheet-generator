using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace CssSpriteSheetGenerator.Models
{
    /// <summary>
    /// The base class for sprites and sprite sheets.
    /// </summary>
    public interface ISpriteBase : INotifyPropertyChanged, INotifyPropertyChanging, IExtensibleDataObject
    {
        /// <summary>
        /// The CSS class name for this element.
        /// </summary>
        string ClassName { get; set; }

        /// <summary>
        /// The x-coordinate position of this element.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "X")]
        int X { get; set; }

        /// <summary>
        /// The y-coordinate position of this element.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Y")]
        int Y { get; set; }

        /// <summary>
        /// Indicates whether this sprite is selected or not.
        /// </summary>
        bool IsSelected { get; set; }
    }
}
