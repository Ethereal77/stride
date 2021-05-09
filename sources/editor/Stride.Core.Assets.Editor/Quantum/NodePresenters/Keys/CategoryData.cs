// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;

namespace Stride.Core.Assets.Editor.Quantum.NodePresenters.Keys
{
    public static class CategoryData
    {
        public const string Category = nameof(Category);

        public static readonly PropertyKey<bool> Key = new PropertyKey<bool>(Category, typeof(CategoryData));

        public static string ComputeCategoryNodeName(string categoryName) => categoryName + Category;
    }
}
