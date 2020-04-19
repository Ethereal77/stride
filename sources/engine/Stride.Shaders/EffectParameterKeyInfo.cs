// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core;
using Xenko.Core.Serialization;
using Xenko.Rendering;

namespace Xenko.Shaders
{
    /// <summary>
    /// The header of a shader parameter.
    /// </summary>
    [DataContract]
    public struct EffectParameterKeyInfo
    {
        /// <summary>
        /// The key of the parameter.
        /// </summary>
        [DataMemberIgnore]
        public ParameterKey Key;

        /// <summary>
        /// The key name.
        /// </summary>
        public string KeyName;
    }
}
