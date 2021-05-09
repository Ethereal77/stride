// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Shaders;

namespace Stride.Rendering.Materials
{
    /// <summary>
    /// Base interface for a shading light dependent model material feature.
    /// </summary>
    public interface IMaterialShadingModelFeature : IMaterialFeature, IEquatable<IMaterialShadingModelFeature>
    {
    }
}
