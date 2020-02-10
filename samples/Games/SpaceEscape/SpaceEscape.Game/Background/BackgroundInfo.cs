// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;

using Xenko.Engine;

namespace SpaceEscape.Background
{
    public class BackgroundInfo : ScriptComponent
    {
        public BackgroundInfo()
        {
            Holes = new List<Hole>();
        }

        public int MaxNbObstacles { get; set; }
        public List<Hole> Holes { get; private set; }
    }
}
