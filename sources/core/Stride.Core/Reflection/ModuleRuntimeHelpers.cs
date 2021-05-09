// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Reflection;

using Stride.Core.Annotations;

namespace Stride.Core.Reflection
{
    public static class ModuleRuntimeHelpers
    {
        public static void RunModuleConstructor([NotNull] Module module)
        {
            System.Runtime.CompilerServices.RuntimeHelpers.RunModuleConstructor(module.ModuleHandle);
        }
    }
}
