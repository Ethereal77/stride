// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Threading.Tasks;

using Stride.Engine;
using Stride.Physics;

namespace SpriteStudioDemo
{
    public class BeamScript : AsyncScript
    {
        private const float maxWidthX = 8f + 2f;
        private const float minWidthX = -8f - 2f;

        private bool dead;

        public void Die()
        {
            dead = true;
        }

        public override async Task Execute()
        {
            while(Game.IsRunning)
            {
                await Script.NextFrame();

                if ((Entity.Transform.Position.X <= minWidthX) || (Entity.Transform.Position.X >= maxWidthX) || dead)
                {
                    SceneSystem.SceneInstance.RootScene.Entities.Remove(Entity);
                    return;
                }
            }
        }
    }
}
