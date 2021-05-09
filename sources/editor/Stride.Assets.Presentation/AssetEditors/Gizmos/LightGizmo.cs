// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Graphics;
using Stride.Rendering.Lights;

namespace Stride.Assets.Presentation.AssetEditors.Gizmos
{
    /// <summary>
    /// The base class for all light gizmos
    /// </summary>
    public class LightGizmo : BillboardingGizmo<LightComponent>
    {
        private static readonly LightComponent DefaultLightComponent = new LightComponent();

        protected LightComponent LightComponent => ContentEntity.Get<LightComponent>() ?? DefaultLightComponent;

        protected LightGizmo(EntityComponent component, string lightName, byte[] lightImageData)
            : base(component,lightName, lightImageData)
        {
        }

        protected Color3 GetLightColor(GraphicsDevice graphicsDevice)
        {
            var component = LightComponent;
            var colorLight = component.Type as IColorLight;

            return colorLight?.ComputeColor(graphicsDevice.ColorSpace, 1f) ?? new Color3(); // don't want to include light intensity in gizmo color (post effects not applied on gizmos)
        }
    }
}
