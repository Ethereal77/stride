// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Xenko.Core;

namespace Xenko.Core.Assets.Quantum
{
    [Flags]
    public enum SubHierarchyCloneFlags
    {
        /// <summary>
        /// No specific flag.
        /// </summary>
        None = 0,
        /// <summary>
        /// Clean any reference to an <see cref="IIdentifiable"/> object that is external to the sub-hierarchy.
        /// </summary>
        CleanExternalReferences = 1,
        /// <summary>
        /// Generates new identifiers for any <see cref="IIdentifiable"/> object that is internal to the sub-hierarchy.
        /// </summary>
        GenerateNewIdsForIdentifiableObjects = 2,
        /// <summary>
        /// Do not apply overrides on the cloned sub-hierarchy.
        /// </summary>
        RemoveOverrides = 4,
    }
}
