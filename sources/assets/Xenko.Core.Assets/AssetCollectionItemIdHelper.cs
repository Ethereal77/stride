// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Reflection;

namespace Xenko.Core.Assets
{
    // TODO: at some point we should converge to a state where collection ids, which are for override and asset specific, should move from Core.Design to this assembly (with related yaml serializer). Meanwhile, we have to split some of the logic in an unclean manner.
    public static class AssetCollectionItemIdHelper
    {
        public static void GenerateMissingItemIds(object rootObject)
        {
            var visitor = new CollectionIdGenerator();
            visitor.Visit(rootObject);
        }
    }
}
