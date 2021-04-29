// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Stride.Core.Serialization.Contents;

namespace Stride.Core.Assets.Analysis
{
    /// <summary>
    ///   Represents a link between <see cref="Asset"/>s.
    /// </summary>
    public struct AssetLink : IContentLink
    {
        /// <summary>
        ///   The asset item pointed by the dependency.
        /// </summary>
        public readonly AssetItem Item;
        private readonly IReference assetReference;


        /// <summary>
        ///   Create an asset dependency of a specified <paramref name="type"/> pointing to an asset <paramref name="item"/>.
        /// </summary>
        /// <param name="item">The item the dependency is pointing to.</param>
        /// <param name="type">The type of the dependency between the items.</param>
        public AssetLink(AssetItem item, ContentLinkType type)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            Item = item;
            Type = type;
            assetReference = item.ToReference();
        }

        //
        // This constructor exists for better factorization of code in AssetDependencies.
        // It should not be turned into public as AssetItem is not valid.
        //
        internal AssetLink(IReference reference, ContentLinkType type)
        {
            Item = null;
            Type = type;
            assetReference = reference ?? throw new ArgumentNullException(nameof(reference));
        }


        /// <summary>
        ///   Gets or sets the type of the dependency for the referenced <see cref="Element"/>.
        /// </summary>
        /// <value>
        ///   One or more of the values of <see cref="ContentLinkType"/> representing the type of link
        ///   for the referenced asset.
        /// </value>
        public ContentLinkType Type { get; set; }

        /// <summary>
        ///   Gets the reference to the element at the opposite side of the link.
        /// </summary>
        public IReference Element => assetReference;


        /// <summary>
        ///   Gets a copy of the asset dependency.
        /// </summary>
        /// <returns>The cloned asset link.</returns>
        public AssetLink Clone() => new(Item.Clone(keepPackage: true), Type);
    }
}
