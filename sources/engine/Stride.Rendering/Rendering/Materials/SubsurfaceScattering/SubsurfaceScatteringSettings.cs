// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;
using Stride.Data;

namespace Stride.Rendering.Materials
{
    [DataContract]
    [Display("Subsurface Scattering Settings")]
    public class SubsurfaceScatteringSettings : Configuration
    {
        [DataMember(10)]
        public int SamplesPerScatteringKernel = 25;   // When this value is changed, all SSS materials need to be regenerated.

        public const int SamplesPerScatteringKernel2 = 25;  // TODO: Replace this by the above member!
    }
}