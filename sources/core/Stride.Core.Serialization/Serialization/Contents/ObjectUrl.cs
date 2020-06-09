// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Stride.Core.Serialization.Contents
{
    [DataContract, Serializable]
    [DataSerializer(typeof(Serializer))]
    public struct ObjectUrl : IEquatable<ObjectUrl>
    {
        public static readonly ObjectUrl Empty = new ObjectUrl(UrlType.None, string.Empty);

        public readonly UrlType Type;
        public readonly string Path;

        public ObjectUrl(UrlType type, string path)
        {
            if (path is null)
                throw new ArgumentException(nameof(path));

            Type = type;
            Path = path;
        }

        public bool Equals(ObjectUrl other)
        {
            return Type == other.Type && string.Equals(Path, other.Path);
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;

            return obj is ObjectUrl url && Equals(url);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) Type * 397) ^ Path.GetHashCode();
            }
        }

        public static bool operator ==(ObjectUrl left, ObjectUrl right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ObjectUrl left, ObjectUrl right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return Path;
        }

        internal class Serializer : DataSerializer<ObjectUrl>
        {
            public override void Serialize(ref ObjectUrl obj, ArchiveMode mode, SerializationStream stream)
            {
                if (mode == ArchiveMode.Serialize)
                {
                    stream.Write(obj.Type);
                    stream.Write(obj.Path);
                }
                else
                {
                    var type = stream.Read<UrlType>();
                    var path = stream.ReadString();
                    obj = new ObjectUrl(type, path);
                }
            }
        }
    }
}
