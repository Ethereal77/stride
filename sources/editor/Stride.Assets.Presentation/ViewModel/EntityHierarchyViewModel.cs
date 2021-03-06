// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;

using Stride.Core.Assets.Editor.ViewModel;
using Stride.Core.Annotations;
using Stride.Core.Reflection;
using Stride.Assets.Entities;
using Stride.Core.Quantum;
using Stride.Animations;
using Stride.Assets.Presentation.AssetEditors.EntityHierarchyEditor.ViewModels;
using Stride.Engine;

namespace Stride.Assets.Presentation.ViewModel
{
    public abstract class EntityHierarchyViewModel : AssetCompositeHierarchyViewModel<EntityDesign, Entity>
    {
        protected EntityHierarchyViewModel([NotNull] AssetViewModelConstructionParameters parameters)
            : base(parameters)
        {
        }

        internal new EntityHierarchyEditorViewModel Editor => (EntityHierarchyEditorViewModel)base.Editor;

        /// <inheritdoc />
        protected override bool ShouldConstructPropertyMember(IMemberNode member)
        {
            // Hide child nodes of a compute curve.
            if (member.Parent.Type.HasInterface(typeof(IComputeCurve<>)) && member.Name == nameof(ComputeAnimationCurve<int>.KeyFrames))
                return false;
            return base.ShouldConstructPropertyMember(member);
        }
    }
}
