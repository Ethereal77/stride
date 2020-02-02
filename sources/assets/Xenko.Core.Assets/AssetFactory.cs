// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Xenko.Core.Assets
{
    /// <summary>
    /// A base implementation of the <see cref="IAssetFactory{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of asset this factory can create.</typeparam>
    public abstract class AssetFactory<T> : IAssetFactory<T> where T : Asset
    {
        /// <inheritdoc/>
        public Type AssetType => typeof(T);

        /// <inheritdoc/>
        public abstract T New();
    }
}
