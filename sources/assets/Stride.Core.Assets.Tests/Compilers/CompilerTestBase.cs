// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;

using Stride.Core.Serialization;

using Xunit;

namespace Stride.Core.Assets.Tests.Compilers
{
    public class CompilerTestBase : IDisposable
    {
        public CompilerTestBase()
        {
            TestCompilerBase.CompiledAssets = new HashSet<AssetItem>();
        }

        public void Dispose()
        {
            TestCompilerBase.CompiledAssets = null;
        }

        protected static TContentType CreateRef<TContentType>(AssetItem assetItem) where TContentType : class, new()
        {
            return AttachedReferenceManager.CreateProxyObject<TContentType>(assetItem.Id, assetItem.Location);
        }
    }
}
