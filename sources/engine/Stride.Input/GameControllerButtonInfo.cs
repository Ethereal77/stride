// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

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