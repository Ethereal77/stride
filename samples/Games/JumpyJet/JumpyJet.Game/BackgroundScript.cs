// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Threading.Tasks;

using Stride.Engine;
using Stride.Engine.Events;
using Stride.Physics;
using Stride.Rendering.Compositing;

namespace JumpyJet
{
    public class BackgroundScript : AsyncScript
    {
        private EventReceiver gameOverListener = new EventReceiver(GameGlobals.GameOverEventKey);
        private EventReceiver gameResetListener = new EventReceiver(GameGlobals.GameResetEventKey);

        public override async Task Execute()
        {
            // Find our JumpyJetRenderer to start/stop parallax background
            var renderer = (JumpyJetRenderer)((SceneCameraRenderer)SceneSystem.GraphicsCompositor.Game).Child;

            while (Game.IsRunning)
            {
                await gameOverListener.ReceiveAsync();
                renderer.StopScrolling();

                await gameResetListener.ReceiveAsync();
                renderer.StartScrolling();
            }
        }
    }
}
