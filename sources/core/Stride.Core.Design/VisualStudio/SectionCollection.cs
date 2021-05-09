// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2009 SLNTools - Christian Warren
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

using Stride.Core.Annotations;

namespace Stride.Core.VisualStudio
{
    /// <summary>
    /// A collection of <see cref="Section"/>
    /// </summary>
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public sealed class SectionCollection
        : KeyedCollection<string, Section>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SectionCollection"/> class.
        /// </summary>
        public SectionCollection()
            : base(StringComparer.InvariantCultureIgnoreCase)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SectionCollection"/> class.
        /// </summary>
        /// <param name="items">The items.</param>
        public SectionCollection(IEnumerable<Section> items)
            : this()
        {
            this.AddRange(items);
        }

        protected override string GetKeyForItem([NotNull] Section item)
        {
            return item.Name;
        }

        protected override void InsertItem(int index, [NotNull] Section item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            // Add a clone of the item instead of the item itself
            base.InsertItem(index, item.Clone());
        }

        protected override void SetItem(int index, [NotNull] Section item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            // Add a clone of the item instead of the item itself
            base.SetItem(index, item.Clone());
        }
    }
}
