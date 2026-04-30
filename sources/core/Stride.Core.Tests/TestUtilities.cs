// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Xunit;

namespace Stride.Core.Tests
{
    public class TestUtilities
    {
        [StructLayout(LayoutKind.Sequential, Size = 8)]
        struct S
        {
            public int A;
            public int B;
        }

        [Fact]
        public unsafe void Base()
        {
            // Allocate memory
            var data = Utilities.AllocateMemory(32, 16);

            // Check allocation and alignment
            Assert.True(data != IntPtr.Zero);
            Assert.True(((long)data % 16) == 0);

            // FreeMemory
            Utilities.FreeMemory(data);
        }
    }
}
