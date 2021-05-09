// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.IO;
using System.Runtime.CompilerServices;

using Stride.Core.Assets;
using Stride.Core;
using Stride.Core.Reflection;

namespace Stride.GameStudio.Tests
{
    public class Module
    {
        [Core.ModuleInitializer]
        internal static void Initialize()
        {
            PackageSessionPublicHelper.FindAndSetMSBuildVersion();
        }
    }
}
