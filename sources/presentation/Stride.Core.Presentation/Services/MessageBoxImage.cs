// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Core.Presentation.Services
{
    // TODO: make these enum independent from their System.Windows equivalent
    /// <summary>
    /// An enum representing the image to display in a message box.
    /// </summary>
    public enum MessageBoxImage
    {
        /// <summary>
        /// No image will be displayed in the message box.
        /// </summary>
        None = 0,
        /// <summary>
        /// An image representing an error will be displayed in the message box.
        /// </summary>
        Error = 16,
        /// <summary>
        /// An image representing a question will be displayed in the message box.
        /// </summary>
        Question = 32,
        /// <summary>
        /// An image representing a warning will be displayed in the message box.
        /// </summary>
        Warning = 48,
        /// <summary>
        /// An image representing an information will be displayed in the message box.
        /// </summary>
        Information = 64,
    }
}
