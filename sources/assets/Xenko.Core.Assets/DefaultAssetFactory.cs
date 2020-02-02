// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Xenko.Core.Assets
{
    /// <summary>
    /// An implementation of the <see cref="AssetFactory{T}"/> class that uses the default public parameterless constructor
    /// of the associated asset type.
    /// </summary>
    /// <typeparam name="T">The type of asset this factory can create.</typeparam>
    public class DefaultAssetFactory<T> : AssetFactory<T> where T : Asset
    {
        public static T Create()
        {
            return (T)Activator.CreateInstance(typeof(T));
        }

        /// <inheritdoc/>
        public override T New()
        {
            if (typeof(T).GetConstructor(Type.EmptyTypes) == null)
                throw new InvalidOperationException("The associated asset type does not have a public parameterless constructor.");

            return Create();
        }
    }
}
