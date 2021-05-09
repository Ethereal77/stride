// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Core.Presentation.Dirtiables
{
    public interface IDirtiable
    {
        /// <summary>
        /// Gets the dirty state of this object.
        /// </summary>
        bool IsDirty { get; }

        /// <summary>
        /// Updates the <see cref="IsDirty"/> property to the given value.
        /// </summary>
        /// <param name="value">The new value for the dirty flag.</param>
        void UpdateDirtiness(bool value);
    }
}
