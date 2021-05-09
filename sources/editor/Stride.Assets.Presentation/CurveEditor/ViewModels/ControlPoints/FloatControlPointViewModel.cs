// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Annotations;
using Stride.Core.Quantum;

namespace Stride.Assets.Presentation.CurveEditor.ViewModels
{
    public sealed class FloatControlPointViewModel : KeyFrameControlPointViewModel<float>
    {
        public FloatControlPointViewModel([NotNull] CurveViewModelBase curve, [NotNull] IMemberNode keyNode, [NotNull] IMemberNode valueNode, [NotNull] IMemberNode tangentTypeNode)
            : base(curve, keyNode, valueNode, tangentTypeNode)
        {
            SynchronizePoint();
        }

        protected override double GetValue()
        {
            return ValueBinding.Value;
        }

        protected override void SetValue(double value)
        {
            ValueBinding.Value = (float)value;
        }
    }
}
