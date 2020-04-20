// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xunit.runner.stride
{
    class Program
    {
        public static void Main(string[] args) => StrideXunitRunner.Main(args, interactiveMode => GameTestBase.ForceInteractiveMode = interactiveMode);
    }
}
