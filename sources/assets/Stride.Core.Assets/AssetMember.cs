// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Core.Reflection;

namespace Stride.Core.Assets
{
    /// <summary>
    /// Represents a member of an asset.
    /// </summary>
    public struct AssetMember
    {
        /// <summary>
        /// The asset.
        /// </summary>
        public Asset Asset;

        /// <summary>
        /// The path to the member in the asset.
        /// </summary>
        public MemberPath MemberPath;
    }
}
