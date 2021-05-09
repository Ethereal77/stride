// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core;

namespace Stride.Rendering.Images
{
    /// <summary>
    ///   Represents the base class for a tonemap operator.
    /// </summary>
    [DataContract]
    public abstract class ToneMapOperator : ColorTransformBase
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref="ToneMapOperator"/> class.
        /// </summary>
        /// <param name="effectName">Name of the effect.</param>
        /// <exception cref="ArgumentNullException"><paramref name="effectName"/> is a <c>null</c> reference.</exception>
        protected ToneMapOperator(string effectName) : base(effectName) { }


        [DataMemberIgnore]
        public override bool Enabled
        {
            get => base.Enabled;
            set => base.Enabled = value;
        }
    }
}
