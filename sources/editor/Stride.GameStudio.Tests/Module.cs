// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.IO;
using System.Runtime.CompilerServices;

using Xenko.Core.Assets;
using Xenko.Core;
using Xenko.Core.Reflection;

namespace Xenko.GameStudio.Tests
{
    public class Module
    {
        [ModuleInitializer]
        internal static void Initialize()
        {
            PackageSessionPublicHelper.FindAndSetMSBuildVersion();
        }
    }
}
