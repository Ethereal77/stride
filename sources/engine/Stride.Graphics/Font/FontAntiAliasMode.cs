// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;

namespace Stride.Graphics.Font
{
    /// <summary>
    /// Available antialias mode.
    /// </summary>
    [DataContract]
    public enum FontAntiAliasMode
    {
        /// <summary>
        /// The default grayscale anti-aliasing
        /// </summary>
        /// <userdoc>Equivalent to 'Grayscale'.</userdoc>
        Default,

        /// <summary>
        /// Use grayscale antialiasing
        /// </summary>
        /// <userdoc>Uses several levels of gray smooth the character edges. 
        /// 'ClearType' .</userdoc>
        Grayscale = Default,

        /// <summary>
        /// Use cleartype antialiasing.
        /// </summary>
        /// <userdoc>Uses the display red/green/blue sub-pixels to smooth character edges</userdoc>
        ClearType,

        /// <summary>
        /// Don't use any antialiasing
        /// </summary>
        /// <userdoc>Does not perform any anti-aliasing</userdoc>
        Aliased,
    }
}
