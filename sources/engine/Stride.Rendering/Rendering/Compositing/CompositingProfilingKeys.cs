// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Diagnostics;

namespace Stride.Rendering.Compositing
{
    public class CompositingProfilingKeys
    {
        public static readonly ProfilingKey Compositing = new ProfilingKey("Compositing");

        public static readonly ProfilingKey Opaque = new ProfilingKey(Compositing, "Opaque");

        public static readonly ProfilingKey Transparent = new ProfilingKey(Compositing, "Transparent");

        public static readonly ProfilingKey MsaaResolve = new ProfilingKey(Compositing, "MSAA Resolve");

        public static readonly ProfilingKey LightShafts = new ProfilingKey(Compositing, "LightShafts");

        public static readonly ProfilingKey GBuffer = new ProfilingKey(Compositing, "GBuffer");
    }
}
