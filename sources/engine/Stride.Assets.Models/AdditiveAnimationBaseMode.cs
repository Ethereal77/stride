// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;

namespace Stride.Assets.Models
{
    [DataContract]
    public enum AdditiveAnimationBaseMode
    {
        /// <summary>
        /// Uses first frame of animation.
        /// </summary>
        FirstFrame = 1,

        /// <summary>
        /// Uses animation.
        /// </summary>
        Animation = 2,
    }
}
