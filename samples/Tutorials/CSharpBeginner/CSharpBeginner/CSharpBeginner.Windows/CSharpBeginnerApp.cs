// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Engine;

namespace CSharpBeginner
{
    class CSharpBeginnerApp
    {
        static void Main(string[] args)
        {
            using (var game = new Stride.Engine.Game())
            {
                game.Run();
            }
        }
    }
}
