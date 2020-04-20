// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Stride.Engine.Network;
using Stride.Games;

namespace Stride.RemoteShaderCompiler
{
    class Program
    {
        static void Main(string[] args)
        {
            var shaderCompilerServer = new ShaderCompilerServer();
            shaderCompilerServer.Listen(13335);
        }
    }
}
