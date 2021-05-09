// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Copyright (c) 2007-2011 SlimDX Group
// See the LICENSE.md file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Reflection;

using Stride.Core.Annotations;

namespace Stride.Core.TypeConverters
{
    public sealed class FieldPropertyDescriptor : PropertyDescriptor, IEquatable<FieldPropertyDescriptor>
    {
        private readonly FieldInfo fieldInfo;

        [NotNull]
        public FieldInfo FieldInfo => fieldInfo;

        [CanBeNull]
        public override Type ComponentType => fieldInfo.DeclaringType;

        public override bool IsReadOnly => false;

        [NotNull]
        public override Type PropertyType => fieldInfo.FieldType;

        public FieldPropertyDescriptor([NotNull] FieldInfo fieldInfo)
            : base(fieldInfo.Name, new Attribute[0])
        {
            this.fieldInfo = fieldInfo;

            var attributesObject = fieldInfo.GetCustomAttributes(true);
            var attributes = new Attribute[attributesObject.Length];
            for (int i = 0; i < attributes.Length; i++)
                attributes[i] = (Attribute)attributesObject[i];
            AttributeArray = attributes;
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override object GetValue(object component)
        {
            return fieldInfo.GetValue(component);
        }

        public override void ResetValue(object component)
        {
        }

        public override void SetValue(object component, object value)
        {
            fieldInfo.SetValue(component, value);
            OnValueChanged(component, EventArgs.Empty);
        }

        public override bool ShouldSerializeValue(object component)
        {
            return true;
        }

        /// <inheritdoc />
        public bool Equals(FieldPropertyDescriptor other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(fieldInfo, other.fieldInfo);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (GetType() != obj.GetType()) return false;
            return Equals(obj as FieldPropertyDescriptor);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return fieldInfo.GetHashCode();
        }

        public static bool operator ==(FieldPropertyDescriptor left, FieldPropertyDescriptor right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(FieldPropertyDescriptor left, FieldPropertyDescriptor right)
        {
            return !Equals(left, right);
        }
    }
}
