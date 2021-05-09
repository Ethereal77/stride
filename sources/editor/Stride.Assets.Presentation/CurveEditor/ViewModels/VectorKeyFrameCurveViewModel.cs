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
    /// Represents an animation curve with <see cref="Vector2"/> keyframes.
    /// Each component is represented by a child curve.
    /// </summary>
    public sealed class Vector2KeyFrameCurveViewModel : DecomposedCurveViewModel<Vector2>
    {
        public Vector2KeyFrameCurveViewModel([NotNull] CurveEditorViewModel editor, [NotNull] ComputeAnimationCurve<Vector2> computeCurve, CurveViewModelBase parent = null, string name = null)
            : base(editor, parent, computeCurve, name)
        {
            Children.Add(new Vector2ComponentCurveViewModel(editor, this, computeCurve, VectorComponent.X));
            Children.Add(new Vector2ComponentCurveViewModel(editor, this, computeCurve, VectorComponent.Y));
        }
    }

    /// <summary>
    /// Represents an animation curve with <see cref="Vector3"/> keyframes.
    /// Each component is represented by a child curve.
    /// </summary>
    public sealed class Vector3KeyFrameCurveViewModel : DecomposedCurveViewModel<Vector3>
    {
        public Vector3KeyFrameCurveViewModel([NotNull] CurveEditorViewModel editor, [NotNull] ComputeAnimationCurve<Vector3> computeCurve, CurveViewModelBase parent = null, string name = null)
            : base(editor, parent, computeCurve, name)
        {
            Children.Add(new Vector3ComponentCurveViewModel(editor, this, computeCurve, VectorComponent.X));
            Children.Add(new Vector3ComponentCurveViewModel(editor, this, computeCurve, VectorComponent.Y));
            Children.Add(new Vector3ComponentCurveViewModel(editor, this, computeCurve, VectorComponent.Z));
        }
    } 

    /// <summary>
    /// Represents an animation curve with <see cref="Vector4"/> keyframes.
    /// Each component is represented by a child curve.
    /// </summary>
    public sealed class Vector4KeyFrameCurveViewModel : DecomposedCurveViewModel<Vector4>
    {
        public Vector4KeyFrameCurveViewModel([NotNull] CurveEditorViewModel editor, [NotNull] ComputeAnimationCurve<Vector4> computeCurve, CurveViewModelBase parent = null, string name = null)
            : base(editor, parent, computeCurve,name)
        {
            Children.Add(new Vector4ComponentCurveViewModel(editor, this, computeCurve, VectorComponent.X));
            Children.Add(new Vector4ComponentCurveViewModel(editor, this, computeCurve, VectorComponent.Y));
            Children.Add(new Vector4ComponentCurveViewModel(editor, this, computeCurve, VectorComponent.Z));
            Children.Add(new Vector4ComponentCurveViewModel(editor, this, computeCurve, VectorComponent.W));
        }
    }
}
