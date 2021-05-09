// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;

using Stride.Core.Reflection;
using Stride.Core.Yaml.Events;

namespace Stride.Core.Yaml.Serialization.Serializers
{
    [YamlSerializerFactory(YamlSerializerFactoryAttribute.Default)]
    internal class ArraySerializer : IYamlSerializable, IYamlSerializableFactory
    {
        public IYamlSerializable TryCreate(SerializerContext context, ITypeDescriptor typeDescriptor)
        {
            return typeDescriptor is ArrayDescriptor ? this : null;
        }

        public virtual object ReadYaml(ref ObjectContext objectContext)
        {
            var reader = objectContext.Reader;
            var arrayDescriptor = (ArrayDescriptor) objectContext.Descriptor;

            bool isArray = objectContext.Instance != null && objectContext.Instance.GetType().IsArray;
            var arrayList = (IList) objectContext.Instance;

            reader.Expect<SequenceStart>();
            int index = 0;
            if (isArray)
            {
                while (!reader.Accept<SequenceEnd>())
                {
                    var node = reader.Peek<ParsingEvent>();
                    if (index >= arrayList.Count)
                    {
                        throw new YamlException(node.Start, node.End, $"Unable to deserialize array. Current number of elements [{index}] exceeding array size [{arrayList.Count}].");
                    }

                    // Handle aliasing
                    arrayList[index++] = ReadYaml(objectContext.SerializerContext, arrayDescriptor.ElementType);
                }
            }
            else
            {
                var results = new List<object>();
                while (!reader.Accept<SequenceEnd>())
                {
                    results.Add(ReadYaml(objectContext.SerializerContext, arrayDescriptor.ElementType));
                }

                // Handle aliasing
                arrayList = arrayDescriptor.CreateArray(results.Count);
                foreach (var arrayItem in results)
                {
                    arrayList[index++] = arrayItem;
                }
            }
            reader.Expect<SequenceEnd>();

            return arrayList;
        }

        public void WriteYaml(ref ObjectContext objectContext)
        {
            var value = objectContext.Instance;
            var arrayDescriptor = (ArrayDescriptor) objectContext.Descriptor;

            var valueType = value.GetType();
            var arrayList = (IList) value;

            // Emit a Flow sequence or block sequence depending on settings
            objectContext.Writer.Emit(new SequenceStartEventInfo(value, valueType)
            {
                Tag = objectContext.Tag,
                Style = objectContext.Style != DataStyle.Any ? objectContext.Style : (arrayList.Count < objectContext.Settings.LimitPrimitiveFlowSequence ? DataStyle.Compact : DataStyle.Normal)
            });

            foreach (var element in arrayList)
            {
                WriteYaml(objectContext.SerializerContext, element, arrayDescriptor.ElementType);
            }
            objectContext.Writer.Emit(new SequenceEndEventInfo(value, valueType));
        }

        private static object ReadYaml(SerializerContext context, Type expectedType)
        {
            var node = context.Reader.Parser.Current;
            try
            {
                var objectContext = new ObjectContext(context, null, context.FindTypeDescriptor(expectedType));
                // TODO: We should go through the ObjectSerializerBackend, not directly use the ObjectSerializer!
                return context.Serializer.ObjectSerializer.ReadYaml(ref objectContext);
            }
            catch (YamlException)
            {
                throw;
            }
            catch (Exception ex)
            {
                ex = ex.Unwrap();
                throw new YamlException(node, ex);
            }
        }

        private static void WriteYaml(SerializerContext context, object value, Type expectedType)
        {
            var objectContext = new ObjectContext(context, value, context.FindTypeDescriptor(expectedType));
            // TODO: We should go through the ObjectSerializerBackend, not directly use the ObjectSerializer!
            context.Serializer.ObjectSerializer.WriteYaml(ref objectContext);
        }
    }
}
