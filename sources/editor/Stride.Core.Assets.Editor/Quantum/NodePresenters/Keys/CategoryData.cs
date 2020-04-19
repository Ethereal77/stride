// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core;

namespace Xenko.Core.Assets.Editor.Quantum.NodePresenters.Keys
{
    public static class CategoryData
    {
        public const string Category = nameof(Category);

        public static readonly PropertyKey<bool> Key = new PropertyKey<bool>(Category, typeof(CategoryData));

        public static string ComputeCategoryNodeName(string categoryName) => categoryName + Category;
    }
}
