// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

using Stride.Core;
using Stride.Core.Serialization.Contents;

namespace Stride.Core.Assets
{
    /// <summary>
    /// An asset selector
    /// </summary>
    [DataContract(Inherited = true)]
    public abstract class AssetSelector
    {
        public abstract IEnumerable<string> Select(PackageSession packageSession, IContentIndexMap contentIndexMap);
    }
}
