// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Core.IO;
using Stride.Core.Storage;

namespace Stride.Shaders.Tests
{
    static class TestHelper
    {
        public static IDatabaseFileProviderService CreateDatabaseProvider()
        {
            VirtualFileSystem.CreateDirectory(VirtualFileSystem.ApplicationDatabasePath);
            return new DatabaseFileProviderService(new DatabaseFileProvider(ObjectDatabase.CreateDefaultDatabase()));
        }
    }
}
