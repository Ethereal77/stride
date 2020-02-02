// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;

using Xenko.Core.Mathematics;
using Xenko.Engine;
using Xenko.Engine.Events;

namespace JumpyJet
{
    /// <summary>
    /// The script in charge of creating and updating the pipes.
    /// </summary>
    public class PipesScript : SyncScript
    {
        private const float GapBetweenPipe = 4f;
        private const float StartPipePosition = 4f;

        private EventReceiver gameOverListener = new EventReceiver(GameGlobals.GameOverEventKey);
        private EventReceiver gameResetListener = new EventReceiver(GameGlobals.GameResetEventKey);
        private EventReceiver gameStartedListener = new EventReceiver(GameGlobals.GameStartedventKey);

        private readonly List<Entity> pipeSets = new List<Entity>();
        
        private bool isScrolling;

        private readonly Random random = new Random();

        private float sceneWidth;
        private float pipeOvervaluedWidth = 1f;

        public override void Start()
        {
            var pipeSetPrefab = Content.Load<Prefab>("Pipe Set");

            // Create PipeSets
            sceneWidth = GameGlobals.GamePixelToUnitScale*GraphicsDevice.Presenter.BackBuffer.Width;
            var numberOfPipes = (int) Math.Ceiling(sceneWidth + 2* pipeOvervaluedWidth / GapBetweenPipe);
            for (int i = 0; i < numberOfPipes; i++)
            {
                var pipeSet = pipeSetPrefab.Instantiate()[0];
                pipeSets.Add(pipeSet);
                Entity.AddChild(pipeSet);
            }

            // Reset the position of the PipeSets
            Reset();
        }

        public override void Update()
        {
            if (gameOverListener.TryReceive())
                isScrolling = false;

            if (gameStartedListener.TryReceive())
                isScrolling = true;

            if(gameResetListener.TryReceive())
                Reset();

            if (!isScrolling)
                return;

            var elapsedTime = (float) Game.UpdateTime.Elapsed.TotalSeconds;

            for (int i = 0; i < pipeSets.Count; i++)
            {
                var pipeSetTransform = pipeSets[i].Transform;

                // update the position of the pipe
                pipeSetTransform.Position -= new Vector3(elapsedTime * GameGlobals.GameSpeed, 0, 0);
                    
                // move the pipe to the end of screen if not visible anymore
                if (pipeSetTransform.Position.X + pipeOvervaluedWidth/2 < -sceneWidth/2)
                {

                    // When a pipe is determined to be reset,
                    // get its next position by adding an offset to the position
                    // of a pipe which index is before itself.
                    var prevPipeSetIndex =  (i + pipeSets.Count - 1) % pipeSets.Count;

                    var nextPosX = pipeSets[prevPipeSetIndex].Transform.Position.X + GapBetweenPipe;
                    pipeSetTransform.Position = new Vector3(nextPosX, GetPipeRandoYPosition(), 0);
                }
            }
        }

        private float GetPipeRandoYPosition()
        {
            return GameGlobals.GamePixelToUnitScale * random.Next(50, 225);
        }

        private void Reset()
        {
            for (var i = 0; i < pipeSets.Count; ++i)
                pipeSets[i].Transform.Position = new Vector3(StartPipePosition + i * GapBetweenPipe, GetPipeRandoYPosition(), 0);
        }

        public override void Cancel()
        {
            // remove all the children pipes.
            Entity.Transform.Children.Clear();
        }
    }
}
