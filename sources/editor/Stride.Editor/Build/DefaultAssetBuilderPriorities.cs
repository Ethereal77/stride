// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Editor.Build
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
