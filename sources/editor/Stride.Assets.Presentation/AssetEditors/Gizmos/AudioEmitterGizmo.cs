// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Engine;

namespace Stride.Assets.Presentation.AssetEditors.Gizmos
{
    [GizmoComponent(typeof(AudioEmitterComponent), true)]
    public class AudioEmitterGizmo : BillboardingGizmo<AudioEmitterComponent>
    {
        public AudioEmitterGizmo(EntityComponent component)
            : base(component, "AudioEmitter", GizmoResources.AudioEmitterGizmo)
        {
        }
    }
}
