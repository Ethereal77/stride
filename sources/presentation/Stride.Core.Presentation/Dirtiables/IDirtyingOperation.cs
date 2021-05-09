// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

using Stride.Core.Annotations;

namespace Stride.Core.Presentation.Dirtiables
{
    public interface IDirtyingOperation
    {
        /// <summary>
        /// Gets whether this operation is currently realized.
        /// </summary>
        bool IsDone { get; }

        /// <summary>
        /// Gets the dirtiable objects associated to this operation, or <c>null</c> if no dirtiable is associated.
        /// </summary>
        [NotNull]
        IReadOnlyList<IDirtiable> Dirtiables { get; }
    }
}
