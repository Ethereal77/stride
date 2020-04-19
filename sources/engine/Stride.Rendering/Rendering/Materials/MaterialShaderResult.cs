// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;

using Xenko.Core.Diagnostics;
using Xenko.Rendering;
using Xenko.Rendering.Materials;
using Xenko.Shaders;

namespace Xenko.Rendering.Materials
{
    public class MaterialShaderResult : LoggerResult
    {
        public Material Material { get; set; }
    }
}
