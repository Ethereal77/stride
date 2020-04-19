// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Xenko.Core.Assets.Editor.Services
{
    /// <summary>
    /// This interface represents a service that build assets.
    /// </summary>
    public interface IBuildService
    {
        /// <summary>
        /// Raised when an asset has been built.
        /// </summary>
        event EventHandler<AssetBuiltEventArgs> AssetBuilt;
    }
}
