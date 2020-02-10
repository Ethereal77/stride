// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Engine;

namespace Xenko.Assets.Presentation.AssetEditors.Gizmos
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
