// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;

namespace Stride.Engine.Design
{
    [DataContract]
    public class EntityComponentProperty
    {
        public EntityComponentProperty()
        {
        }

        public EntityComponentProperty(EntityComponentPropertyType type, string name, object value)
        {
            Type = type;
            Name = name;
            Value = value;
        }

        public EntityComponentPropertyType Type { get; set; }
        public string Name { get; set; }

        public object Value { get; set; }
    }
}
