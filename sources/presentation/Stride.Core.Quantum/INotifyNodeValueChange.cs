// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Xenko.Core.Quantum
{
    /// <summary>
    /// An interface representing an object notifying changes when the value of a related node changes.
    /// </summary>
    public interface INotifyNodeValueChange
    {
        /// <summary>
        /// Raised just before a change to the value of a related node occurs.
        /// </summary>
        event EventHandler<MemberNodeChangeEventArgs> ValueChanging;

        /// <summary>
        /// Raised when a change to the value of a related node has occurred.
        /// </summary>
        event EventHandler<MemberNodeChangeEventArgs> ValueChanged;
    }
}
