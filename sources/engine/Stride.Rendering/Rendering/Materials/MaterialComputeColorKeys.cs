// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Mathematics;
using Stride.Graphics;

namespace Stride.Rendering.Materials
{
    public class MaterialComputeColorKeys
    {
        public MaterialComputeColorKeys(ObjectParameterKey<Texture> textureBaseKey, ParameterKey valueBaseKey, Color? defaultTextureValue = null, bool isColor = true)
        {
            //if (textureBaseKey == null) throw new ArgumentNullException("textureBaseKey");
            //if (valueBaseKey == null) throw new ArgumentNullException("valueBaseKey");
            TextureBaseKey = textureBaseKey;
            ValueBaseKey = valueBaseKey;
            DefaultTextureValue = defaultTextureValue;
            IsColor = isColor;
        }

        public ObjectParameterKey<Texture> TextureBaseKey { get; private set; }

        public ParameterKey ValueBaseKey { get; private set; }

        public Color? DefaultTextureValue { get; private set; }

        public bool IsColor { get; private set; }
    }
}
