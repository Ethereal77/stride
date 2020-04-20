// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;

using Xenko.Core.Reflection;

namespace Xenko.Core.Yaml.Serialization
{
    /// <summary>
    /// A dynamic member to allow adding dynamic members to objects (that could store additional properties outside of the instance).
    /// </summary>
    public abstract class DynamicMemberDescriptorBase : IMemberDescriptor
    {
        protected DynamicMemberDescriptorBase(string name, Type type, Type declaringType)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (type == null)
                throw new ArgumentNullException("type");
            Name = name;
            Type = type;
            DeclaringType = declaringType;
            OriginalName = Name;
            Mask = 1;
            ShouldSerialize = ObjectDescriptor.ShouldSerializeDefault;
            DefaultNameComparer = StringComparer.OrdinalIgnoreCase;
        }

        public string Name { get; set; }

        public string OriginalName { get; set; }

        public StringComparer DefaultNameComparer { get; set; }

        public Type Type { get; }

        public Type DeclaringType { get; }

        // TODO: store the proper type descriptor here
        public ITypeDescriptor TypeDescriptor => null;

        public int? Order { get; set; }

        public DataMemberMode Mode { get; set; }

        public abstract object Get(object thisObject);

        public abstract void Set(object thisObject, object value);

        public abstract bool HasSet { get; }

        public bool IsPublic { get { return true; } }

        public uint Mask { get; set; }

        public DataStyle Style { get; set; }

        public ScalarStyle ScalarStyle { get; set; }

        public ShouldSerializePredicate ShouldSerialize { get; set; }

        public bool HasDefaultValue => false;
        public object DefaultValue => throw new InvalidOperationException();

        public List<string> AlternativeNames { get; set; }

        public object Tag { get; set; }

        public MemberInfo MemberInfo => null;

        public IEnumerable<T> GetCustomAttributes<T>(bool inherit) where T : Attribute
        {
            yield break;
        }
    }
}
