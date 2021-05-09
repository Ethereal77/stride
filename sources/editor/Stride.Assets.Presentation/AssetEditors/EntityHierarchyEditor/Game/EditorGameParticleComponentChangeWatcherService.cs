// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core.Quantum;
using Stride.Assets.Presentation.AssetEditors.GameEditor.Services;
using Stride.Particles.Components;
using Stride.Particles.Materials;

namespace Stride.Assets.Presentation.AssetEditors.EntityHierarchyEditor.Game
{
    public class EditorGameParticleComponentChangeWatcherService : EditorGameComponentChangeWatcherService
    {
        public EditorGameParticleComponentChangeWatcherService(IEditorGameController controller)
            : base(controller)
        {
        }

        public override Type ComponentType => typeof(ParticleSystemComponent);

        protected override void ComponentPropertyChanged(object sender, INodeChangeEventArgs e)
        {
            var memberNode = e.Node as IMemberNode;
            if (memberNode == null)
                return;

            if (memberNode.Name == nameof(ParticleMaterialSimple.AlphaAdditive) ||
                memberNode.Name == nameof(ParticleMaterialSimple.ZOffset) ||
                memberNode.Name == nameof(ParticleMaterialSimple.SoftEdgeDistance))
            {
                (memberNode.Parent.Retrieve() as ParticleMaterialSimple)?.ForceUpdate();
            }
        }
    }
}