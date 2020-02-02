// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Engine;

namespace Xenko.Assets.Presentation.AssetEditors.Gizmos
{
    [GizmoComponent(typeof(BackgroundComponent), true)]
    public class BackgroundGizmo : BillboardingGizmo<BackgroundComponent>
    {
        public BackgroundGizmo(EntityComponent component)
            : base(component, "Background", GizmoResources.BackgroundGizmo)
        {
        }
    }
}
