// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.TextureConverter.Requests
{
    /// <summary>
    /// Request to update a texture from an atlas
    /// </summary>
    class AtlasUpdateRequest : IRequest
    {
        public override RequestType Type { get { return RequestType.AtlasUpdate; } }

        /// <summary>
        /// The name of the texture to replace in the atlas.
        /// </summary>
        public string Name { get; private set; }


        /// <summary>
        /// The new texture.
        /// </summary>
        public TexImage Texture { get; private set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="AtlasUpdateRequest"/> class.
        /// </summary>
        /// <param name="texture">The new texture.</param>
        /// <param name="name">The name of the texture to replace.</param>
        public AtlasUpdateRequest(TexImage texture, string name)
        {
            Texture = texture;
            Name = name;
        }
    }
}
