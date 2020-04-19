// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

using Xenko.Core;
using Xenko.Core.Diagnostics;
using Xenko.Engine;
using Xenko.Games;
using Xenko.Input;

namespace Xenko.Graphics.Regression
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
