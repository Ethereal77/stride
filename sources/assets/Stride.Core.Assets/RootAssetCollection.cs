// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core.Collections;
using Stride.Core.Serialization;
using Stride.Core.Serialization.Serializers;

namespace Stride.Core.Assets
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
