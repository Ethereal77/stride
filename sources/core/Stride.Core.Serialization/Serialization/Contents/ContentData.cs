// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Stride.Core.Serialization.Serializers;

namespace Stride.Core.Serialization.Contents
{
    [DataSerializer(typeof(EmptyDataSerializer<ContentData>))]
    [DataContract(Inherited = true)]
    public abstract class ContentData : IContentData
    {
        [DataMemberIgnore]
        public string Url { get; set; }
    }
}
