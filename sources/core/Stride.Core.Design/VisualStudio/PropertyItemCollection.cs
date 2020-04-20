// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2009 SLNTools - Christian Warren
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

using Stride.Core.Annotations;

namespace Stride.Core.VisualStudio
{
    /// <summary>
    /// A collection of <see cref="PropertyItem"/>
    /// </summary>
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public sealed class PropertyItemCollection
        : KeyedCollection<string, PropertyItem>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyItemCollection"/> class.
        /// </summary>
        public PropertyItemCollection()
            : base(StringComparer.InvariantCultureIgnoreCase)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyItemCollection"/> class.
        /// </summary>
        /// <param name="items">The items to copy from.</param>
        public PropertyItemCollection(IEnumerable<PropertyItem> items)
            : this()
        {
            this.AddRange(items);
        }

        protected override string GetKeyForItem([NotNull] PropertyItem item)
        {
            return item.Name;
        }

        protected override void InsertItem(int index, [NotNull] PropertyItem item)
        {
            var existingItem = (Contains(GetKeyForItem(item))) ? this[GetKeyForItem(item)] : null;

            if (existingItem == null)
            {
                // Add a clone of the item instead of the item itself
                base.InsertItem(index, item.Clone());
            }
            else if (item.Value != existingItem.Value)
            {
                existingItem.Value = item.Value;
            }
        }

        protected override void SetItem(int index, [NotNull] PropertyItem item)
        {
            // Add a clone of the item instead of the item itself
            base.SetItem(index, item.Clone());
        }
    }
}