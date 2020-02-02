// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;

using Xenko.Core;
using Xenko.Core.Serialization;
using Xenko.Core.Serialization.Contents;
using Xenko.Graphics;

namespace Xenko.SpriteStudio.Runtime
{
    [DataContract]
    [ReferenceSerializer, DataSerializerGlobal(typeof(ReferenceSerializer<SpriteStudioSheet>), Profile = "Content")]
    [ContentSerializer(typeof(DataContentSerializer<SpriteStudioSheet>))]
    public class SpriteStudioSheet
    {
        public List<SpriteStudioNode> NodesInfo { get; set; }

        public SpriteSheet SpriteSheet { get; set; }

        [DataMemberIgnore]
        public Sprite[] Sprites { get; internal set; }
    }
}
