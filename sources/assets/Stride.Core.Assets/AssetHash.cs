// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Storage;

namespace Stride.Core.Assets
{
    /// <summary>
    /// Helper to compute a stable hash from an asset including all meta informations (ids, overrides).
    /// </summary>
    public static class AssetHash
    {
        /// <summary>
        /// Computes a stable hash from an asset including all meta informations (ids, overrides).
        /// </summary>
        /// <param name="asset">An object instance</param>
        /// <param name="flags">Flags used to control the serialization process</param>
        /// <returns>a stable hash</returns>
        public static ObjectId Compute(object asset, AssetClonerFlags flags = AssetClonerFlags.None)
        {
            return AssetCloner.ComputeHash(asset, flags);
        }
    }
}
