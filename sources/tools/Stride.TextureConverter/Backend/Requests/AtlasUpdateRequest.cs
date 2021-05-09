// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.TextureConverter.Requests
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
