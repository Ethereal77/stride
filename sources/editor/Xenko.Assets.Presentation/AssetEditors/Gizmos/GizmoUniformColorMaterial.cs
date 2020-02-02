// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Xenko.Core.Mathematics;
using Xenko.Graphics;
using Xenko.Rendering;
using Xenko.Rendering.Materials;
using Xenko.Rendering.Materials.ComputeColors;

namespace Xenko.Assets.Presentation.AssetEditors.Gizmos
{
    public static class GizmoUniformColorMaterial
    {
        public static readonly ValueParameterKey<Color4> GizmoColorKey = ParameterKeys.NewValue<Color4>();

        public static Material Create(GraphicsDevice device, Color color, bool emissive = true)
        {
            var desc = new MaterialDescriptor();
            if (emissive)
            {
                desc.Attributes.Emissive = new MaterialEmissiveMapFeature(new ComputeColor() { Key = GizmoColorKey });
            }
            else
            {
                desc.Attributes.Diffuse = new MaterialDiffuseMapFeature(new ComputeColor() { Key = GizmoColorKey });
                desc.Attributes.DiffuseModel = new MaterialDiffuseLambertModelFeature();
            }

            var material = Material.New(device, desc);

            // set the color to the material
            UpdateColor(device, material, color);

            // set the transparency property to the material if necessary
            if (color.A < Byte.MaxValue)
            {

                material.Passes[0].HasTransparency = true;
                // TODO GRAPHICS REFACTOR
                //material.Parameters.SetResourceSlow(Graphics.Effect.BlendStateKey, device.BlendStates.NonPremultiplied);
            }

            return material;
        }

        public static void UpdateColor(GraphicsDevice device, Material material, Color color)
        {
            // set the color to the material
            material.Passes[0].Parameters.Set(GizmoColorKey, new Color4(color).ToColorSpace(device.ColorSpace));
        }
    }
}
