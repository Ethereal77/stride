// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Annotations;
using Xenko.Core.Quantum;

namespace Xenko.Assets.Presentation.CurveEditor.ViewModels
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
