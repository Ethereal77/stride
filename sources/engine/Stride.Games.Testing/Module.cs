// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Threading.Tasks;

using Xenko.Core;
using Xenko.Core.Mathematics;
using Xenko.Engine;

namespace Xenko.Games.Testing
{
    //This is how we inject the assembly to run automatically at game start, paired with Xenko.targets and the MSBuild property XenkoAutoTesting
    internal class Module
    {
        [ModuleInitializer]
        public static void Initialize()
        {
            //Quit after 10 seconds anyway!
            Task.Run(async () =>
            {
                await Task.Delay(20000);
                if (!GameTestingSystem.Initialized)
                {
                    Console.WriteLine(@"FATAL: Test launch timeout. Aborting.");

                    GameTestingSystem.Quit();
                }
            });

            //quit after 10 seconds in any case
            Game.GameStarted += (sender, args) =>
            {
                var game = (Game)sender;
                var testingSystem = new GameTestingSystem(game.Services);
                game.GameSystems.Add(testingSystem);
            };
        }
    }
}
