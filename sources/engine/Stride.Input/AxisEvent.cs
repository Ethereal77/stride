// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Stride.Input
{
    /// <summary>
    /// Event for an axis changing state on a device
    /// </summary>
    public abstract class AxisEvent : InputEvent
    {
        /// <summary>
        /// The new value of the axis
        /// </summary>
        public float Value;
    }
}