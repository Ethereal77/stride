// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Xenko.Core.Assets.Editor.ViewModel
{
    /// <summary>
    /// Arguments of the  <see cref="SessionViewModel.AssetPropertiesChanged"/> event.
    /// </summary>
    public class AssetChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssetChangedEventArgs"/> class.
        /// </summary>
        /// <param name="assets">The collection of assets that have changed.</param>
        public AssetChangedEventArgs(IReadOnlyCollection<AssetViewModel> assets)
        {
            Assets = assets;
        }

        /// <summary>
        /// Gets the collection of assets that have changed.
        /// </summary>
        public IReadOnlyCollection<AssetViewModel> Assets { get; }
    }
}
