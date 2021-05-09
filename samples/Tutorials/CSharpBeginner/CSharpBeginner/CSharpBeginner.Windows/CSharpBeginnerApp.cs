// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// See the LICENSE.md file in the project root for full license information.

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
