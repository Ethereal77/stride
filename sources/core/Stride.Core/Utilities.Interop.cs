// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Core
{
    [Obsolete("Do not use.", DiagnosticId = "STRIDE2000")]
    internal sealed class Interop
    {
        [Obsolete("Do not use.", DiagnosticId = "STRIDE2000")]
        public static void Pin<T>(T data) where T : class
        {
            throw new NotImplementedException();
        }
    }
}
