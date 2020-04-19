// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Xenko.Core.Diagnostics;

namespace Xenko.Core.Assets.Editor.Services
{
    /// <summary>
    /// This class represents the argument of the <see cref="IBuildService.AssetBuilt"/> event.
    /// </summary>
    public class AssetBuiltEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssetBuiltEventArgs"/> class.
        /// </summary>
        /// <param name="assetItem">The asset item that has been built.</param>
        /// <param name="buildLog">The log of the build.</param>
        public AssetBuiltEventArgs(AssetItem assetItem, LoggerResult buildLog)
        {
            AssetItem = assetItem;
            BuildLog = buildLog;
        }

        /// <summary>
        /// Gets the asset item that has been built.
        /// </summary>
        public AssetItem AssetItem { get; private  set; }

        /// <summary>
        /// Gets the log of the build.
        /// </summary>
        public LoggerResult BuildLog { get; set; }
    }
}
