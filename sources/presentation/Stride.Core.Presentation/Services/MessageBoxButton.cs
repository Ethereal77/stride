// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Core.Presentation.Services
{
    // TODO: make these enum independent from their System.Windows equivalent
    /// <summary>
    /// An enum representing the buttons to display in a message box.
    /// </summary>
    public enum MessageBoxButton
    {
        /// <summary>
        /// Display a single OK button.
        /// </summary>
        OK = 0,
        /// <summary>
        /// Display a OK button and a Cancel button.
        /// </summary>
        OKCancel = 1,
        /// <summary>
        /// Display a Yes button, a No button and a Cancel button.
        /// </summary>
        YesNoCancel = 3,
        /// <summary>
        /// Display a Yes button and a No button.
        /// </summary>
        YesNo = 4,
    }
}
