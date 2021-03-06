// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

using Stride.Core.Annotations;

namespace Stride.Core.Assets.Editor.Services
{
    /// <summary>
    /// Represents the result of <see cref="ICopyPasteService.DeserializeCopiedData"/>.
    /// </summary>
    public interface IPasteResult
    {
        /// <summary>
        /// The collection of the pasted items.
        /// </summary>
        [ItemNotNull, NotNull]
        IReadOnlyList<IPasteItem> Items { get; }
    }

    /// <summary>
    /// Represents an item of the result of a paste operation.
    /// </summary>
    public interface IPasteItem
    {
        /// <summary>
        /// The pasted data.
        /// </summary>
        [CanBeNull]
        object Data { get; }

        /// <summary>
        /// The processor that was used to process the data.
        /// </summary>
        [CanBeNull]
        IPasteProcessor Processor { get; }
    }
}