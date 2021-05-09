// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core;

namespace Stride.Core.Assets.Quantum
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
