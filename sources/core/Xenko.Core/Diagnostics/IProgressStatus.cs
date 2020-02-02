// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Xenko.Core.Diagnostics
{
    /// <summary>
    /// Provides progress of an operation.
    /// </summary>
    public interface IProgressStatus
    {
        // TODO: Current design is poor as it does not support recursive progress

        /// <summary>
        /// An event handler to notify the progress of an operation.
        /// </summary>
        event EventHandler<ProgressStatusEventArgs> ProgressChanged;

        /// <summary>
        /// Handles the <see cref="E:ProgressChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="ProgressStatusEventArgs"/> instance containing the event data.</param>
        void OnProgressChanged(ProgressStatusEventArgs e);
    }
}
