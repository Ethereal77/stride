// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Input
{
    /// <summary>
    /// An event to describe a change in a game controller axis state
    /// </summary>
    public class GameControllerAxisEvent : AxisEvent
    {
        /// <summary>
        /// Index of the axis
        /// </summary>
        public int Index;

        /// <summary>
        /// The game controller that sent this event
        /// </summary>
        public IGameControllerDevice GameController => (IGameControllerDevice)Device;

        public override string ToString()
        {
            return $"{nameof(Index)}: {Index} ({GameController.AxisInfos[Index].Name}), {nameof(Value)}: {Value}, {nameof(GameController)}: {GameController.Name}";
        }
    }
}