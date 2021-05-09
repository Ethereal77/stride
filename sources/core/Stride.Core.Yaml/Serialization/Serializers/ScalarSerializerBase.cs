// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core.Yaml.Events;

namespace Stride.Core.Yaml.Serialization.Serializers
{
    public abstract class ScalarSerializerBase : IYamlSerializable
    {
        public object ReadYaml(ref ObjectContext objectContext)
        {
            var scalar = objectContext.Reader.Expect<Scalar>();
            return ConvertFrom(ref objectContext, scalar);
        }

        public abstract object ConvertFrom(ref ObjectContext context, Scalar fromScalar);

        public void WriteYaml(ref ObjectContext objectContext)
        {
            var value = objectContext.Instance;
            var typeOfValue = value.GetType();

            var context = objectContext.SerializerContext;

            var isSchemaImplicitTag = context.Schema.IsTagImplicit(objectContext.Tag);
            var scalar = new ScalarEventInfo(value, typeOfValue)
            {
                IsPlainImplicit = isSchemaImplicitTag,
                Style = objectContext.ScalarStyle,
                Anchor = objectContext.Anchor,
                Tag = objectContext.Tag,
            };


            if (scalar.Style == ScalarStyle.Any)
            {
                // Parse default types 
                switch (Type.GetTypeCode(typeOfValue))
                {
                    case TypeCode.Object:
                    case TypeCode.String:
                    case TypeCode.Char:
                        break;
                    default:
                        scalar.Style = ScalarStyle.Plain;
                        break;
                }
            }

            scalar.RenderedValue = ConvertTo(ref objectContext);

            // Emit the scalar
            WriteScalar(ref objectContext, scalar);
        }

        /// <summary>
        /// Writes the scalar to the <see cref="SerializerContext.Writer"/>. See remarks.
        /// </summary>
        /// <param name="objectContext">The object context.</param>
        /// <param name="scalar">The scalar.</param>
        /// <remarks>
        /// This method can be overloaded to replace the converted scalar just before writing it.
        /// </remarks>
        protected virtual void WriteScalar(ref ObjectContext objectContext, ScalarEventInfo scalar)
        {
            // Emit the scalar
            objectContext.SerializerContext.Writer.Emit(scalar);
        }

        public abstract string ConvertTo(ref ObjectContext objectContext);
    }
}