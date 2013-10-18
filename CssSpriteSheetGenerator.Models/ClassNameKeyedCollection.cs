using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace CssSpriteSheetGenerator.Models
{
    /// <summary>
    /// A collection for managing class names.
    /// </summary>
    public class ClassNameKeyedCollection : KeyedCollection<string, SpriteBase>
    {
        /// <summary>
        /// Gets the class name for <paramref name="item" />.
        /// </summary>
        /// <param name="item">The item to get the class name for.</param>
        /// <returns>The class name for <paramref name="item" />.</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        protected override string GetKeyForItem(SpriteBase item)
        {
            return item.ClassName;
        }

        /// <summary>
        /// Adds a <see cref="PropertyChangedEventHandler" /> to <paramref name="item" /> and inserts it into the collection.
        /// </summary>
        /// <param name="index">The zero-based index at which item should be inserted.</param>
        /// <param name="item">The object to insert.</param>
        /// <exception cref="ArgumentNullException"><paramref name="item" /> cannot be null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">index is less than 0.-or-index is greater than <see cref="System.Collections.ObjectModel.Collection{T}.Count" />.</exception>
        protected override void InsertItem(int index, SpriteBase item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            if (Contains(item.ClassName))
            {
                string className;

                do
                    className = "{0}-{1}".Invariant(item.ClassName, Helper.GenerateRandomString(4));
                while (Contains(className));

                item.ClassName = className;
            }

            item.PropertyChanging += new PropertyChangingEventHandler(OnItemPropertyChanging);
            base.InsertItem(index, item);
        }

        // Changes the item key in the collection when it changes on the item
        private void OnItemPropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            if (e.PropertyName == SpriteBase.ClassNamePropertyName)
            {
                var spriteBase = sender as SpriteBase;
                var eventArgs = e as PropertyChangingEventArgs<string>;
                if (spriteBase != null && eventArgs != null)
                    ChangeItemKey(spriteBase, eventArgs.NewValue);
            }
        }
    }
}
