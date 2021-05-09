// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Diagnostics;

using Stride.Core;
using Stride.Core.Serialization;
using Stride.Rendering;
using Stride.Graphics;

namespace Stride.Shaders
{
    /// <summary>
    /// Binding to a sampler.
    /// </summary>
    [DataContract]
    [DebuggerDisplay("SamplerState {Key} ({Description.Filter})")]
    public class EffectSamplerStateBinding
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EffectSamplerStateBinding"/> class.
        /// </summary>
        public EffectSamplerStateBinding()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectSamplerStateBinding"/> class.
        /// </summary>
        /// <param name="keyName">Name of the key.</param>
        /// <param name="description">The description.</param>
        public EffectSamplerStateBinding(string keyName, SamplerStateDescription description)
        {
            KeyName = keyName;
            Description = description;
        }

        /// <summary>
        /// The key used to bind this sampler, used internaly.
        /// </summary>
        [DataMemberIgnore]
        public ParameterKey Key;

        /// <summary>
        /// The key name.
        /// </summary>
        public string KeyName;

        /// <summary>
        /// The description of this sampler.
        /// </summary>
        public SamplerStateDescription Description;
    }
}
