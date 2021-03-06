// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Mathematics;
using Stride.Graphics;
using Stride.Rendering;
using Stride.Rendering.Materials;
using Stride.Rendering.Materials.ComputeColors;

namespace Stride.Assets.Presentation.AssetEditors.Gizmos
{
    public static class GizmoEmissiveColorMaterial
    {
        public static Material Create(GraphicsDevice device, Color color, float intensity)
        {
            var material = Material.New(device, new MaterialDescriptor
            {
                Attributes =
                {
                    Diffuse = new MaterialDiffuseMapFeature(new ComputeColor()),
                    DiffuseModel = new MaterialDiffuseLambertModelFeature(),
                    Emissive = new MaterialEmissiveMapFeature(new ComputeColor())
                }
            });

            // set the color to the material
            material.Passes[0].Parameters.Set(MaterialKeys.DiffuseValue, new Color4(color).ToColorSpace(device.ColorSpace));

            material.Passes[0].Parameters.Set(MaterialKeys.EmissiveIntensity, intensity);
            material.Passes[0].Parameters.Set(MaterialKeys.EmissiveValue, new Color4(color).ToColorSpace(device.ColorSpace));

            return material;
        }
    }
}
