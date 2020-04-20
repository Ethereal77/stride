// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Stride.Input
{
    /// <summary>
    /// Provides information about a gamepad button
    /// </summary>
    public class GameControllerButtonInfo : GameControllerObjectInfo
    {
        /// <summary>
        /// The type of button
        /// </summary>
        public GameControllerButtonType Type;

        public override string ToString()
        {
            return $"GameController Button {{{Name}}} [{Type}]";
        }
    }
}