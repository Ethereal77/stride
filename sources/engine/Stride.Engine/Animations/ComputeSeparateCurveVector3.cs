// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;
using Stride.Core.Annotations;
using Stride.Core.Mathematics;

namespace Stride.Animations
{
    [DataContract("ComputeSeparateCurveVector3")]
    [Display("3 Channels")]
    public class ComputeSeparateCurveVector3 : IComputeCurve<Vector3>
    {
        [DataMember(10)]
        [NotNull]
        [Display("X")]
        public IComputeCurve<float> X
        {
            get { return xValue; }
            set
            {
                xValue = value;
                hasChanged = true;
            }
        }

        [DataMember(20)]
        [NotNull]
        [Display("Y")]
        public IComputeCurve<float> Y
        {
            get { return yValue; }
            set
            {
                yValue = value;
                hasChanged = true;
            }
        }

        [DataMember(30)]
        [NotNull]
        [Display("Z")]
        public IComputeCurve<float> Z
        {
            get { return zValue; }
            set
            {
                zValue = value;
                hasChanged = true;
            }
        }

        public Vector3 Evaluate(float t)
        {
            return new Vector3(X.Evaluate(t), Y.Evaluate(t), Z.Evaluate(t));
        }

        private bool hasChanged = true;
        private IComputeCurve<float> xValue = new ComputeConstCurveFloat();
        private IComputeCurve<float> yValue = new ComputeConstCurveFloat();
        private IComputeCurve<float> zValue = new ComputeConstCurveFloat();

        /// <inheritdoc/>
        public bool UpdateChanges()
        {
            if (hasChanged)
            {
                hasChanged = false;
                return true;
            }

            return (X?.UpdateChanges() ?? false) || (Y?.UpdateChanges() ?? false) || (Z?.UpdateChanges() ?? false);
        }
    }
}
