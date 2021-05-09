// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Core.Assets.Editor.Services
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
