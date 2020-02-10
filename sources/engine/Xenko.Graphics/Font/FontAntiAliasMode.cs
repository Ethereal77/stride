// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core;

namespace Xenko.Graphics.Font
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
