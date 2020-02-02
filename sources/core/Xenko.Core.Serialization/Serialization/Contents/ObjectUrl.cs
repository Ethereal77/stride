// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Xenko.Core.Serialization.Contents
{
    [DataContract]
    [DataSerializer(typeof(Serializer))]
    public struct ObjectUrl : IEquatable<ObjectUrl>
    {
        public static readonly ObjectUrl Empty = new ObjectUrl(UrlType.None, string.Empty);

        public readonly UrlType Type;
        public readonly string Path;

        public ObjectUrl(UrlType type, string path)
        {
            if (path == null)
                throw new ArgumentException("path");

            Type = type;
            Path = path;
        }

        public bool Equals(ObjectUrl other)
        {
            return Type == other.Type && string.Equals(Path, other.Path);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is ObjectUrl && Equals((ObjectUrl)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int)Type * 397) ^ Path.GetHashCode();
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
