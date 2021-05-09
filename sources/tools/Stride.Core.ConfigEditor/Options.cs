// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Reflection;
using System.IO;
using System.Xml.Serialization;

namespace Stride.ConfigEditor
{
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class Options
    {
        private string stridePath;
        [XmlElement]
        public string StridePath
        {
            get => stridePath;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException($"Invalid '{nameof(StridePath)}' property value.");

                stridePath = value;
            }
        }

        [XmlElement]
        public string StrideConfigFilename { get; set; }

        private static readonly XmlSerializer serializer = new XmlSerializer(typeof(Options));

        public static Options Load()
        {
            try
            {
                var filename = Path.ChangeExtension(Assembly.GetEntryAssembly().Location, ".config");
                return (Options) serializer.Deserialize(new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
            }
            catch
            {
                return null;
            }
        }

        public void Save()
        {
            var filename = Path.ChangeExtension(Assembly.GetEntryAssembly().Location, ".config");
            serializer.Serialize(new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite), this);
        }
    }
}
