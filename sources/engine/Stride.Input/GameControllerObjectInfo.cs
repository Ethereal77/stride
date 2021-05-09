// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Input
{
    /// <summary>
    /// Provides information about an object exposed by a gamepad
    /// </summary>
    public class GameControllerObjectInfo
    {
        /// <summary>
        /// The name of the object, reported by the device
        /// </summary>
        public string Name;

        public override string ToString()
        {
            return $"GameController Object {{{Name}}}";
        }
    }
}