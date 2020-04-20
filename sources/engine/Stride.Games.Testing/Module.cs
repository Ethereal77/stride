// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Threading.Tasks;

using Stride.Core;
using Stride.Core.Mathematics;
using Stride.Engine;

namespace Stride.Games.Testing
{
    //This is how we inject the assembly to run automatically at game start, paired with Stride.targets and the MSBuild property StrideAutoTesting
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
