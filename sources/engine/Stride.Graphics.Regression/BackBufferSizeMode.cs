// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Graphics.Regression
{
    /// <summary>
    /// Describes the different way to compute the back buffer size.
    /// </summary>
    public enum BackBufferSizeMode
    {
        /// <summary>
        /// Fit the size of the back buffer to the desired sizes
        /// </summary>
        FitToDesiredValues,

        /// <summary>
        /// Fit the size of the back buffer to the size of the window
        /// </summary>
        FitToWindowSize,

        /// <summary> 
        /// Calculate the back buffer size based on the window ratio and desired height/width.
        /// </summary>
        FitToWindowRatio,
    };
}
