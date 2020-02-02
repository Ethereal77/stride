// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Editor.Build
{
    public static class DefaultAssetBuilderPriorities
    {
        // Some default priorities for AssetBuildUnit.PriorityMajor
        // Later we will add Effect too
        public static readonly int ScenePriority = -10;
        public static readonly int PreviewPriority = 10;
        public static readonly int ThumbnailPriority = 20;
    }
}
