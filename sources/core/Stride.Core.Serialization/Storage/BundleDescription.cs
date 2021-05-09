// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

namespace Stride.Core.Storage
{
    /// <summary>
    /// Description of a bundle: header, dependencies, objects and assets.
    /// </summary>
    public class BundleDescription
    {
        public BundleOdbBackend.Header Header { get; set; }

        public List<string> Dependencies { get; private set; }
        public List<ObjectId> IncrementalBundles { get; private set; }
        public List<KeyValuePair<ObjectId, BundleOdbBackend.ObjectInfo>> Objects { get; private set; }
        public List<KeyValuePair<string, ObjectId>> Assets { get; private set; }

        public BundleDescription()
        {
            Dependencies = new List<string>();
            IncrementalBundles = new List<ObjectId>();
            Objects = new List<KeyValuePair<ObjectId, BundleOdbBackend.ObjectInfo>>();
            Assets = new List<KeyValuePair<string, ObjectId>>();
        }
    }
}
