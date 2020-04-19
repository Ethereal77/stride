// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Storage;

namespace Xenko.Core.Assets
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
