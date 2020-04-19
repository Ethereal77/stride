// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;

using Xenko.Core;

namespace Xenko.SpriteStudio.Offline
{
    [DataContract]
    public class NodeAnimationData
    {
        public NodeAnimationData()
        {
            Data = new Dictionary<string, List<Dictionary<string, string>>>();
        }

        public Dictionary<string, List<Dictionary<string, string>>> Data;
    }
}
