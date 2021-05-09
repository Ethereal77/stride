// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Text;

using Newtonsoft.Json;

using ServiceWire;

namespace Stride.VisualStudio.Commands
{
    public class NewtonsoftSerializer : ISerializer
    {
        private readonly JsonSerializerSettings settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        public T Deserialize<T>(byte[] bytes)
        {
            if (bytes is null || bytes.Length == 0)
                return default;

            var json = Encoding.UTF8.GetString(bytes);
            return JsonConvert.DeserializeObject<T>(json, settings);
        }

        public object Deserialize(byte[] bytes, string typeConfigName)
        {
            if (typeConfigName is null)
                throw new ArgumentNullException(nameof(typeConfigName));

            var type = typeConfigName.ToType();

            if (typeConfigName  is null || bytes  is null|| bytes.Length == 0)
                return type.GetDefault();

            var json = Encoding.UTF8.GetString(bytes);
            return JsonConvert.DeserializeObject(json, type, settings);
        }

        public byte[] Serialize<T>(T obj)
        {
            if (obj == null)
                return null;

            var json = JsonConvert.SerializeObject(obj, settings);
            return Encoding.UTF8.GetBytes(json);
        }

        public byte[] Serialize(object obj, string typeConfigName)
        {
            if (obj is null)
                return null;

            var type = typeConfigName.ToType();
            var json = JsonConvert.SerializeObject(obj, type, settings);
            return Encoding.UTF8.GetBytes(json);
        }
    }
}
