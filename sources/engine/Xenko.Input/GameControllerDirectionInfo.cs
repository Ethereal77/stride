// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Input
{
    /// <summary>
    /// Provides information about a gamepad direction input
    /// </summary>
    public class GameControllerDirectionInfo : GameControllerObjectInfo
    {
        public override string ToString()
        {
            return $"GameController Direction {{{Name}}}";
        }
    }
}