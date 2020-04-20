// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Engine;

namespace Stride.Assets.Presentation.AssetEditors.Gizmos
{
    [GizmoComponent(typeof(AudioListenerComponent), true)]
    public class AudioListenerGizmo : BillboardingGizmo<AudioListenerComponent>
    {
        public AudioListenerGizmo(EntityComponent component)
            : base(component, "AudioListener", GizmoResources.AudioListenerGizmo)
        {
        }
    }
}
