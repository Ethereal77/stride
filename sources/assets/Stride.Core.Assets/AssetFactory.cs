// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Core.Assets
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
