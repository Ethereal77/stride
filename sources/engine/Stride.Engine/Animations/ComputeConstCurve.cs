// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core;
using Xenko.Core.Annotations;

namespace Xenko.Animations
{
    /// <summary>
    /// A node which describes a constant value for a compute curve
    /// </summary>
    /// <typeparam name="T">Sampled data's type</typeparam>
    [DataContract(Inherited = true)]
    [Display("Constant")]
    [InlineProperty]
    public class ComputeConstCurve<T> : IComputeCurve<T> where T : struct 
    {
        /// <summary>
        /// Constant value to return every time this flat curve is sampled
        /// </summary>
        /// <userdoc>
        /// The constant value to return every time this flat function is sampled
        /// </userdoc>
        [DataMember(10)]
        [Display("Value")]
        [InlineProperty]
        public T Value
        {
            get { return typeValue; }
            set
            {
                typeValue = value;
                hasChanged = true;
            }
        }

        private T typeValue;

        private bool hasChanged = true;
        /// <inheritdoc/>
        public bool UpdateChanges()
        {
            if (hasChanged)
            {
                hasChanged = false;
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public T Evaluate(float location)
        {
            return Value;
        }
    }
}
