// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

using Stride.Core;
using Stride.Core.Diagnostics;
using Stride.Engine;
using Stride.Games;
using Stride.Input;

namespace Stride.Graphics.Regression
{
    public class GameTester
    {
        public static readonly Logger Logger = GlobalLogger.GetLogger("GameTester");

        private static object uniThreadLock = new object();

        public static void RunGameTest(Game game)
        {
            using (game)
            {
                game.Run();
            }
        }
    }
}
