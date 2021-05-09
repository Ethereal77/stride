// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Stride.Core;
using Stride.Core.IO;
using Stride.Core.Storage;

namespace Stride.StorageTool
{
    /// <summary>
    ///   Description of an object entry in the bundle.
    /// </summary>
    public class ObjectEntry
    {
        public string Location { get; set; }
        public ObjectId Id { get; set; }
        public long Size { get; set; }
        public long SizeNotCompressed { get; set; }

        public override string ToString() => $"{Location}\t{Id}\t{Size}\t{SizeNotCompressed}";
    }

    /// <summary>
    ///   Utility class to manipulate storage and bundles.
    /// </summary>
    public class StorageToolApp
    {
        /// <summary>
        ///   Lists the specified bundle to a text file and opens it.
        /// </summary>
        /// <param name="bundlePath">The bundle path.</param>
        public static void View(string bundlePath)
        {
            var entries = GetBundleListing(bundlePath);
            var dumpFilePath = bundlePath + ".txt";

            var text = new StringBuilder();
            foreach (var entry in entries)
                text.AppendLine(entry.ToString());
            File.WriteAllText(dumpFilePath, text.ToString());

            System.Diagnostics.Process.Start(dumpFilePath);
        }

        /// <summary>
        ///   Gets the list of objects contained in a bundle.
        /// </summary>
        /// <param name="bundlePath">The bundle path.</param>
        /// <returns>List of the entries in the specified bundle.</returns>
        private static List<ObjectEntry> GetBundleListing(string bundlePath)
        {
            if (bundlePath is null)
                throw new ArgumentNullException(nameof(bundlePath));
            if (Path.GetExtension(bundlePath) != BundleOdbBackend.BundleExtension)
                throw new StorageAppException($"Invalid bundle file [{bundlePath}] not having extension [{BundleOdbBackend.BundleExtension}]");
            if (!File.Exists(bundlePath))
                throw new StorageAppException($"Bundle file [{bundlePath}] not found");

            BundleDescription bundle;
            using (var stream = File.OpenRead(bundlePath))
            {
                bundle = BundleOdbBackend.ReadBundleDescription(stream);
            }

            var objectInfos = bundle.Objects.ToDictionary(x => x.Key, x => x.Value);

            var entries = new List<ObjectEntry>();
            foreach (var locationIds in bundle.Assets)
            {
                var entry = new ObjectEntry { Location = locationIds.Key, Id = locationIds.Value };

                if (objectInfos.TryGetValue(entry.Id, out BundleOdbBackend.ObjectInfo objectInfo))
                {
                    entry.Size = objectInfo.EndOffset - objectInfo.StartOffset;
                    entry.SizeNotCompressed = objectInfo.SizeNotCompressed;
                }

                entries.Add(entry);
            }

            return entries;
        }
    }
}
