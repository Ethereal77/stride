// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

#pragma warning disable SA1402 // File may only contain a single type

using Stride.Core;

namespace Stride.Animations
{
    [DataContract]
    [Display("KeyFrame")]
    public class AnimationKeyFrame<T> where T : struct
    {
        private T val;
        [DataMember(20)]
        [Display("Value")]
        public T Value { get { return val; } set { val = value; HasChanged = true; } }

        private float key;
        [DataMember(10)]
        [Display("Key")]
        public float Key { get { return key; } set { key = value; HasChanged = true; } }

        private AnimationKeyTangentType tangentType = AnimationKeyTangentType.Linear;
        [DataMember(30)]
        [Display("Tangent")]
        public AnimationKeyTangentType TangentType { get { return tangentType; } set { tangentType = value; HasChanged = true; } }

        [DataMemberIgnore]
        public bool HasChanged = true;
    }
}
