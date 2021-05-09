// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2020 Tebjan Halm
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using Stride.Core.Mathematics;

namespace Stride.Engine
{
    public enum ModelTransformUsage
    {
        Ignore,
        PreMultiply,
        PostMultiply
    }

    public interface IInstancing
    {
        int InstanceCount { get; }

        BoundingBox BoundingBox { get; }

        ModelTransformUsage ModelTransformUsage { get; }

        void Update();
    }
}
