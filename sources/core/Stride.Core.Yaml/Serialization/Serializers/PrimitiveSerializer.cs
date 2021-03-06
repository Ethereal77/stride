// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Globalization;

using Stride.Core.Reflection;
using Stride.Core.Yaml.Events;

namespace Stride.Core.Yaml.Serialization.Serializers
{
    [YamlSerializerFactory(YamlSerializerFactoryAttribute.Default)]
    internal class PrimitiveSerializer : ScalarSerializerBase, IYamlSerializableFactory
    {
        public IYamlSerializable TryCreate(SerializerContext context, ITypeDescriptor typeDescriptor)
        {
            return typeDescriptor is PrimitiveDescriptor ? this : null;
        }

        public override object ConvertFrom(ref ObjectContext context, Scalar scalar)
        {
            var primitiveType = (PrimitiveDescriptor) context.Descriptor;
            var type = primitiveType.Type;
            var text = scalar.Value;

            // Return null if expected type is an object and scalar is null
            if (text == null)
            {
                switch (Type.GetTypeCode(type))
                {
                    case TypeCode.Object:
                    case TypeCode.Empty:
                    case TypeCode.String:
                        return null;
                    default:
                        // TODO check this
                        throw new YamlException(scalar.Start, scalar.End, "Unexpected null scalar value");
                }
            }

            // If type is an enum, try to parse it
            if (type.IsEnum)
            {
                bool enumRemapped;
                var result = primitiveType.ParseEnum(text, out enumRemapped);
                if (enumRemapped)
                {
                    context.SerializerContext.HasRemapOccurred = true;
                }
                return result;
            }

            // Parse default types 
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Boolean:
                    object value;
                    context.SerializerContext.Schema.TryParse(scalar, type, out value);
                    return value;
                case TypeCode.DateTime:
                    return DateTime.Parse(text, CultureInfo.InvariantCulture);
                case TypeCode.String:
                    return text;
            }

            if (type == typeof(TimeSpan))
            {
                return TimeSpan.Parse(text, CultureInfo.InvariantCulture);
            }

            // Remove _ character from numeric values
            text = text.Replace("_", string.Empty);

            // Parse default types 
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Char:
                    if (text.Length != 1)
                    {
                        throw new YamlException(scalar.Start, scalar.End, $"Unable to decode char from [{text}]. Expecting a string of length == 1");
                    }
                    return text.ToCharArray()[0];
                case TypeCode.Byte:
                    return byte.Parse(text, CultureInfo.InvariantCulture);
                case TypeCode.SByte:
                    return sbyte.Parse(text, CultureInfo.InvariantCulture);
                case TypeCode.Int16:
                    return short.Parse(text, CultureInfo.InvariantCulture);
                case TypeCode.UInt16:
                    return ushort.Parse(text, CultureInfo.InvariantCulture);
                case TypeCode.Int32:
                    return int.Parse(text, CultureInfo.InvariantCulture);
                case TypeCode.UInt32:
                    return uint.Parse(text, CultureInfo.InvariantCulture);
                case TypeCode.Int64:
                    return long.Parse(text, CultureInfo.InvariantCulture);
                case TypeCode.UInt64:
                    return ulong.Parse(text, CultureInfo.InvariantCulture);
                case TypeCode.Single:
                    return float.Parse(text, CultureInfo.InvariantCulture);
                case TypeCode.Double:
                    return double.Parse(text, CultureInfo.InvariantCulture);
                case TypeCode.Decimal:
                    return decimal.Parse(text, CultureInfo.InvariantCulture);
            }

            // If we are expecting a type object, return directly the string
            if (type == typeof(object))
            {
                // Try to parse the scalar directly
                string defaultTag;
                object scalarValue;
                if (context.SerializerContext.Schema.TryParse(scalar, true, out defaultTag, out scalarValue))
                {
                    return scalarValue;
                }

                return text;
            }

            throw new YamlException(scalar.Start, scalar.End, $"Unable to decode scalar [{scalar}] not supported by current schema");
        }

        /// <summary>
        /// Appends decimal point to arg if it does not exist
        /// </summary>
        /// <param name="text"></param>
        /// <param name="hasNaN">True if the floating point type supports NaN or Infinity.</param>
        /// <returns></returns>
        private static string AppendDecimalPoint(string text, bool hasNaN)
        {
            for (var i = 0; i < text.Length; i++)
            {
                var c = text[i];
                // Do not append a decimal point if floating point type value
                // - is in exponential form, or
                // - already has a decimal point
                if (c == 'e' || c == 'E' || c == '.')
                {
                    return text;
                }
            }
            // Special cases for floating point type supporting NaN and Infinity
            if (hasNaN && (string.Equals(text, "NaN") || text.Contains("Infinity")))
                return text;

            return text + ".0";
        }

        public override string ConvertTo(ref ObjectContext objectContext)
        {
            var text = string.Empty;
            var value = objectContext.Instance;

            // Return null if expected type is an object and scalar is null
            if (value == null)
            {
                return text;
            }

            var valueType = value.GetType();

            // Handle string
            if (valueType.IsEnum)
            {
                text = ((Enum) Enum.ToObject(valueType, value)).ToString("G");
            }
            else
            {
                // Parse default types 
                switch (Type.GetTypeCode(valueType))
                {
                    case TypeCode.String:
                    case TypeCode.Char:
                        text = value.ToString();
                        break;
                    case TypeCode.Boolean:
                        text = (bool) value ? "true" : "false";
                        break;
                    case TypeCode.Byte:
                        text = ((byte) value).ToString("G", CultureInfo.InvariantCulture);
                        break;
                    case TypeCode.SByte:
                        text = ((sbyte) value).ToString("G", CultureInfo.InvariantCulture);
                        break;
                    case TypeCode.Int16:
                        text = ((short) value).ToString("G", CultureInfo.InvariantCulture);
                        break;
                    case TypeCode.UInt16:
                        text = ((ushort) value).ToString("G", CultureInfo.InvariantCulture);
                        break;
                    case TypeCode.Int32:
                        text = ((int) value).ToString("G", CultureInfo.InvariantCulture);
                        break;
                    case TypeCode.UInt32:
                        text = ((uint) value).ToString("G", CultureInfo.InvariantCulture);
                        break;
                    case TypeCode.Int64:
                        text = ((long) value).ToString("G", CultureInfo.InvariantCulture);
                        break;
                    case TypeCode.UInt64:
                        text = ((ulong) value).ToString("G", CultureInfo.InvariantCulture);
                        break;
                    case TypeCode.Single:
                        //Append decimal point to floating point type values 
                        //because type changes in round trip conversion if ( value * 10.0 ) % 10.0 == 0
                        text = AppendDecimalPoint(((float) value).ToString("R", CultureInfo.InvariantCulture), true);
                        break;
                    case TypeCode.Double:
                        text = AppendDecimalPoint(((double) value).ToString("R", CultureInfo.InvariantCulture), true);
                        break;
                    case TypeCode.Decimal:
                        text = AppendDecimalPoint(((decimal) value).ToString("G", CultureInfo.InvariantCulture), false);
                        break;
                    case TypeCode.DateTime:
                        text = ((DateTime) value).ToString("o", CultureInfo.InvariantCulture);
                        break;
                    default:
                        if (valueType == typeof(TimeSpan))
                        {
                            text = ((TimeSpan) value).ToString("G", CultureInfo.InvariantCulture);
                        }
                        break;
                }
            }

            if (text == null)
            {
                throw new YamlException($"Unable to serialize scalar [{value}] not supported");
            }

            return text;
        }
    }
}
