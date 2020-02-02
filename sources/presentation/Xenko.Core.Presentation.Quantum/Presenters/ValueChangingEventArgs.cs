// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Xenko.Core.Presentation.Quantum.Presenters
{
    /// <summary>
    /// Arguments of the <see cref="INodePresenter.ValueChanging"/> event.
    /// </summary>
    public class ValueChangingEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueChangingEventArgs"/> class.
        /// </summary>
        /// <param name="newValue">The new value of the node.</param>
        public ValueChangingEventArgs(object newValue)
        {
            NewValue = newValue;
        }

        /// <summary>
        /// The new value of the node.
        /// </summary>
        public object NewValue { get; private set; }
    }
}
