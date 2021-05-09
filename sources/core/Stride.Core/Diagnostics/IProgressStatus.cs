// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Core.Diagnostics
{
    /// <summary>
    ///   Provides a way to know the progress of an operation.
    /// </summary>
    public interface IProgressStatus
    {
        // TODO: Current design is poor as it does not support recursive progress

        /// <summary>
        ///   An event handler to notify the progress of an operation.
        /// </summary>
        event EventHandler<ProgressStatusEventArgs> ProgressChanged;

        /// <summary>
        ///   Handles the <see cref="ProgressChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="ProgressStatusEventArgs"/> instance containing the event data.</param>
        void OnProgressChanged(ProgressStatusEventArgs e);
    }
}
