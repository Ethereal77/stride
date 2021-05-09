// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Annotations;
using Stride.Core.Mathematics;
using Stride.Animations;

namespace Stride.Assets.Presentation.CurveEditor.ViewModels
{
    /// <summary>
    /// Represents an animation curve with <see cref="Color4"/> keyframes.
    /// Each component is represented by a child curve.
    /// </summary>
    public sealed class Color4KeyFrameCurveViewModel : DecomposedCurveViewModel<Color4>
    {
        public Color4KeyFrameCurveViewModel([NotNull] CurveEditorViewModel editor, [NotNull] ComputeAnimationCurve<Color4> computeCurve, CurveViewModelBase parent = null, string name = null)
            : base(editor, parent, computeCurve,name)
        {
            Children.Add(new Color4ComponentCurveViewModel(editor, this, computeCurve, ColorComponent.R));
            Children.Add(new Color4ComponentCurveViewModel(editor, this, computeCurve, ColorComponent.G));
            Children.Add(new Color4ComponentCurveViewModel(editor, this, computeCurve, ColorComponent.B));
            Children.Add(new Color4ComponentCurveViewModel(editor, this, computeCurve, ColorComponent.A));
        }
    }
}
