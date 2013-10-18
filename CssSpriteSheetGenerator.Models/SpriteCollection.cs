using System;
using System.Collections.ObjectModel;

namespace CssSpriteSheetGenerator.Models
{
    /// <summary>
    /// A custom <see cref="System.Collections.ObjectModel.ObservableCollection{T}" /> 
    /// that updates the <see cref="Sprite.Parent" /> property of items when the 
    /// collection is modified.
    /// </summary>
    public class SpriteCollection : ObservableCollection<Sprite>
    {
        private WeakReference _Parent;
        /// <summary>
        /// The sprite sheet that this collection is a member of.
        /// </summary>
        private SpriteSheet Parent
        {
            get { return _Parent.Target as SpriteSheet; }
            set { _Parent = new WeakReference(value); }
        }

        /// <summary>
        /// Initializes an instance of the <see cref="SpriteCollection" /> class.
        /// </summary>
        /// <param name="parent">The sprite sheet that this collection is a member of.</param>
        public SpriteCollection(SpriteSheet parent)
        {
            this.Parent = parent;
        }

        /// <summary>
        /// Sets <paramref name="item" />'s <see cref="Sprite.Parent" /> property and then 
        /// adds it to the collection.
        /// </summary>
        /// <param name="index">The zero-based index at which item should be inserted.</param>
        /// <param name="item">The object to insert.</param>
        /// <exception cref="ArgumentNullException"><paramref name="item" /> cannot be null.</exception>
        protected override void InsertItem(int index, Sprite item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            item.Parent = Parent;
            base.InsertItem(index, item);
        }
    }
}
