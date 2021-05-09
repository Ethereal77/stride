// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;
using Stride.Core.Reflection;

namespace Stride.Core.Quantum.Tests
{
    public class Module
    {
        [ModuleInitializer]
        internal static void Initialize()
        {
            ShadowObject.Enable = true;
        }
    }
}
