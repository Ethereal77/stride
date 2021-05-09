// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace Stride.Core.Tests
{
    public class TestUnmanagedArray
    {
        [Fact]
        public void Base()
        {
            using (var testing = new UnmanagedArray<float>(128))
            {
                for (var i = 0; i < testing.Length; i++)
                {
                    testing[i] = i;
                }
                for (var i = 0; i < testing.Length; i++)
                {
                    Assert.Equal(i, testing[i]);
                    testing[i] = -1.0f;
                }

                var managedArray = new float[128];
                for (var i = 0; i < testing.Length; i++)
                {
                    managedArray[i] = i;
                }
                testing.Write(managedArray);
                var managedArray2 = new float[128];
                testing.Read(managedArray2);
                for (var i = 0; i < testing.Length; i++)
                {
                    Assert.Equal(i, testing[i]);
                }
            }
        }
    }
}
