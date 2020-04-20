// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Stride.Input
{
    /// <summary>
    /// An event that was generated from an <see cref="IInputDevice"/>
    /// </summary>
    public abstract class InputEvent : IInputEventArgs
    {
        /// <inheritdoc/>
        public IInputDevice Device { get; protected internal set; }
    }
}
