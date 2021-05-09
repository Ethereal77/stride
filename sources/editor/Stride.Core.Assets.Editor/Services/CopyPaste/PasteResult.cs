// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

namespace Stride.Core.Assets.Editor.Services
{
    /// <inheritdoc cref="IPasteResult" />
    internal class PasteResult : IPasteResult
    {
        private readonly List<PasteItem> items = new List<PasteItem>();

        /// <inheritdoc/>
        public IReadOnlyList<IPasteItem> Items => items;

        /// <summary>
        /// Adds the provided <paramref name="item"/> to the list of <see cref="Items"/>.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void AddItem(PasteItem item)
        {
            items.Add(item);
        }
    }

    /// <inheritdoc cref="IPasteItem" />
    internal class PasteItem : IPasteItem
    {
        public object Data;

        /// <inheritdoc/>
        object IPasteItem.Data => Data;

        /// <inheritdoc />
        public IPasteProcessor Processor { get; set; }
    }
}