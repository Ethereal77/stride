// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;

using Stride.Graphics;

namespace Stride.Rendering.Images
{
    public class ColorTransformContext
    {
        private readonly ColorTransformGroup group;

        private readonly RenderContext renderContext;

        private readonly ParameterCollection sharedParameters;

        private readonly ParameterCollection transformParameters;

        private readonly List<Texture> inputs;

        public ColorTransformContext(ColorTransformGroup @group, RenderContext renderContext)
        {
            this.group = group;
            this.renderContext = renderContext;
            inputs = new List<Texture>();
            sharedParameters = group.Parameters;
            transformParameters = new ParameterCollection();
        }

        public ColorTransformGroup Group
        {
            get
            {
                return group;
            }
        }

        public List<Texture> Inputs
        {
            get
            {
                return inputs;
            }
        }

        public ParameterCollection SharedParameters
        {
            get
            {
                return sharedParameters;
            }
        }

        public RenderContext RenderContext
        {
            get
            {
                return renderContext;
            }
        }
    }
}
