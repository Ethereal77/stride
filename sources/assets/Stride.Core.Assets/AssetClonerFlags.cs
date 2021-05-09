// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core;

namespace Stride.Core.Assets
{
    /// <summary>
    /// Flags used by <see cref="AssetCloner"/>
    /// </summary>
    [Flags]
    public enum AssetClonerFlags
    {
        /// <summary>
        /// No special flags while cloning.
        /// </summary>
        None,

        /// <summary>
        /// Attached references will be cloned as <c>null</c>
        /// </summary>
        ReferenceAsNull = 1,

        /// <summary>
        /// Remove ids attached to item of collections when cloning
        /// </summary>
        RemoveItemIds = 2,

        /// <summary>
        /// Removes invalid objects
        /// </summary>
        RemoveUnloadableObjects = 4,

        /// <summary>
        /// Generates new ids for objects that implement <see cref="IIdentifiable"/>.
        /// </summary>
        GenerateNewIdsForIdentifiableObjects = 8,

        /// <summary>
        /// Clears any external references in the cloned object
        /// </summary>
        ClearExternalReferences = 16,

        /// <summary>
        /// Attached references will be kept as is
        /// </summary>
        KeepReferences = 32,
    }
}
