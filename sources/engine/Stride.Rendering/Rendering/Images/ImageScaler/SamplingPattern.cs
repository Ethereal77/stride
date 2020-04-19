// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xenko.Rendering.Images
{
    /// <summary>
    /// How samples are fetched from the source texture when scaling
    /// </summary>
    public enum SamplingPattern
    {
        /// <summary>
        /// tilted pyramid gather, 9 taps, weights inverse to distance to center. (inspired by "Next Generation Post Processing in Call of Duty Advanced Warfare")
        /// </summary>
        Expanded,

        /// <summary>
        /// simple unique sampling at the center (the hardware sampler creates the 4 neighbor gathering).
        /// </summary>
        Linear,
    }
}
