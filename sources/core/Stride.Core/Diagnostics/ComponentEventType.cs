// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Core.Diagnostics
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public enum ComponentEventType
    {
        /// <summary>
        /// ComponentBase constructor event.
        /// </summary>
        Instantiate = 0,

        /// <summary>
        /// ComponentBase.Destroy() event.
        /// </summary>
        Destroy = 1,

        /// <summary>
        /// IReferencable.AddReference() event.
        /// </summary>
        AddReference = 2,

        /// <summary>
        /// IReferenceable.Release() event.
        /// </summary>
        Release = 3,
    }
}
