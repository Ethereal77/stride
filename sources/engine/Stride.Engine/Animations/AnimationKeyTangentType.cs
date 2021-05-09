// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

#pragma warning disable SA1402 // File may only contain a single type

namespace Stride.Animations
{
    public enum AnimationKeyTangentType
    {
        /// <summary>
        /// Linear - the value is linearly calculated as V1 * (1 - t) + V2 * t
        /// </summary>
        Linear,        
    }
}
