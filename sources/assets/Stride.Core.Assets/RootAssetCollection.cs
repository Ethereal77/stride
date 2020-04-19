// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Xenko.Core.Collections;
using Xenko.Core.Serialization;
using Xenko.Core.Serialization.Serializers;

namespace Xenko.Core.Assets
{
    [DataSerializer(typeof(KeyedSortedListSerializer<RootAssetCollection, AssetId, AssetReference>))]
    public class RootAssetCollection : KeyedSortedList<AssetId, AssetReference>
    {
        /// <inheritdoc/>
        protected override AssetId GetKeyForItem(AssetReference item)
        {
            return item.Id;
        }
    }
}
