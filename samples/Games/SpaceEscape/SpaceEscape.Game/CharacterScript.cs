// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Animations;
using Stride.Input;

namespace SpaceEscape
{
    /// <summary>
    ///   Represents the main character that is controllable by a user.
    ///   It could change lane to left and right, and slide.
    /// </summary>
    public class CharacterScript : AsyncScript
    {
        /// <summary>
        ///   The sprite component containing the shadow of the vessel.
        /// </summary>
        public SpriteComponent CharacterShadow;

        private enum AgentState
        {
            Run,
            ChangeLaneLeft,
            ChangeLaneRight,
            Slide,
            Die
        }

        private enum InputState
        {
            None,
            Left,
            Right,
            Down
        }

        private enum AgentAnimationKeys
        {
            Active,
            DodgeLeft,
            DodgeRight,
            Slide,
            Crash
        }

        private enum BoundingBoxKeys
        {
            Normal,
            Slide
        }

        private const int LeftLane = 0;
        private const int MiddleLane = 1;
        private const int RightLane = 2;
        private const float LaneLength = 5f;
        private float laneHeight;

        public int CurLane { get; private set; }
        private BoundingBox activeBoundingBox;
        private AgentState State
        {
            get => state;
            set
            {
                state = value;
                OnEnter(state);
            }
        }

        private bool shouldProcessInput;
        private bool started;
        private AgentState state;                   // Current state of the agent
        private PlayingAnimation playingAnimation;  // Current active animation
        private float startChangeLanePosX;          // X position before changing lane
        private float targetChangeLanePosX;         // X position after changning lane
        private readonly Dictionary<BoundingBoxKeys, BoundingBox> boundingBoxes = new Dictionary<BoundingBoxKeys, BoundingBox>();

        public bool IsDead => State == AgentState.Die;

        public void Start()
        {
            // Configure Gestures for controlling the agent
            if (!IsLiveReloading) // Live scripting: add the gesture only once (on first load)
                Input.Gestures.Add(new GestureConfigDrag(GestureShape.Free) { MinimumDragDistance = 0.02f, RequiredNumberOfFingers = 1 });

            // Setup Normal pose BoundingBox with that of obtained by ModelComponent
            boundingBoxes[BoundingBoxKeys.Normal] = Entity.Get<ModelComponent>().Model.BoundingBox;

            // Create a slide pose BoundingBox by substracting it with a threshold for making the box, smaller in Y axis
            var modelMinBB = boundingBoxes[BoundingBoxKeys.Normal].Minimum;
            var modelMaxBB = boundingBoxes[BoundingBoxKeys.Normal].Maximum;
            boundingBoxes[BoundingBoxKeys.Slide] = new BoundingBox(modelMinBB, new Vector3(modelMaxBB.X, modelMaxBB.Y - 0.7f, modelMaxBB.Z));

            started = true;
        }

        /// <summary>
        ///   Script function which awaits each frame and updates the CharacterScript.
        /// </summary>
        public override async Task Execute()
        {
            Start();

            laneHeight = Entity.Transform.Position.Y;

            Reset();

            while (Game.IsRunning)
            {
                await Script.NextFrame();

                // Get input state from gesture, if none check from the keyboard
                var inputState = GetInputFromGesture();

                if (inputState == InputState.None)
                    inputState = GetInputFromKeyboard();

                // Process obtained input in this frame
                ProcessInput(inputState);

                // Update the agent
                UpdateState();
            }
        }

        /// <summary>
        ///   Activates the character.
        /// </summary>
        public void Activate()
        {
            shouldProcessInput = true;
        }

        /// <summary>
        ///   Resets the internal state of the agent.
        /// </summary>
        public void Reset()
        {
            if (!started)
                return;

            shouldProcessInput = false;
            State = AgentState.Run;
            CurLane = MiddleLane;
            SetShadowTransparency(1);
            Entity.Transform.Position.Y = laneHeight;
            Entity.Transform.Position.X = GetXPosition(CurLane);
        }

        /// <summary>
        ///   This method is invoked from its user to indicate that the agent has died.
        /// </summary>
        public void OnDied(float floorHeight)
        {
            State = AgentState.Die;
            Entity.Transform.Position.Y = floorHeight;
            SetShadowTransparency(0);
        }

        /// <summary>
        ///   Calculates and returns the current bounding box of the character.
        /// </summary>
        /// <returns>The bounding box.</returns>
        public BoundingBox CalculateCurrentBoundingBox()
        {
            var agentWorldPosition = Entity.Transform.Position;

            // Calculate the CharacterScript bounding box
            var minVec = agentWorldPosition + activeBoundingBox.Minimum;
            var maxVec = agentWorldPosition + activeBoundingBox.Maximum;

            return new BoundingBox(minVec, maxVec);
        }

        private void SetShadowTransparency(float transparency)
        {
            CharacterShadow.Color = transparency * Color.White;
        }

        /// <summary>
        ///   Retrieves keyboard input from the user, and transitions to one of the agent's input state.
        /// </summary>
        /// <returns>The new input state.</returns>
        private InputState GetInputFromKeyboard()
        {
            if (Input.IsKeyPressed(Keys.Left))
                return InputState.Left;

            if (Input.IsKeyPressed(Keys.Right))
                return InputState.Right;

            if (Input.IsKeyPressed(Keys.Down))
                return InputState.Down;

            return InputState.None;
        }

        /// <summary>
        ///   Retrieves input from the user by Drag gesture, and determines the input state by
        ///   calculating the direction of drag by calling <see cref="ProcessInputFromDragGesture"/>.
        /// </summary>
        /// <returns>The new input state.</returns>
        private InputState GetInputFromGesture()
        {
            // Gesture recognition
            foreach (var gestureEvent in Input.GestureEvents)
            {
                // Select only Drag gesture with Began state
                if (gestureEvent.Type == GestureType.Drag && gestureEvent.State == GestureState.Began)
                    // From Draw gesture, determine the InputState from direction of the swipe
                    return ProcessInputFromDragGesture((GestureEventDrag) gestureEvent);
            }

            return InputState.None;
        }

        /// <summary>
        ///   Processes a gesture input event to determine the input state.
        /// </summary>
        /// <param name="gestureEvent">The input event.</param>
        /// <returns>The new input state.</returns>
        private InputState ProcessInputFromDragGesture(GestureEventDrag gestureEvent)
        {
            // Get drag vector and multiply by the screenRatio of the screen, also flip y (-screenRatio)
            var screenRatio = (float) GraphicsDevice.Presenter.BackBuffer.Height / GraphicsDevice.Presenter.BackBuffer.Width;
            var dragVector = (gestureEvent.CurrentPosition - gestureEvent.StartPosition) * new Vector2(1f, -screenRatio);
            var dragDirection = Vector2.Normalize(dragVector);

            Vector2 comparedAxis;
            float xDeg;
            float yDeg;

            // Head of dragDirection is in Quadrant 1
            if (dragDirection.X >= 0 && dragDirection.Y >= 0)
            {
                comparedAxis = Vector2.UnitX;
                xDeg = FindAngleBetweenVector(ref dragDirection, ref comparedAxis);
                comparedAxis = Vector2.UnitY;
                yDeg = FindAngleBetweenVector(ref dragDirection, ref comparedAxis);

                return xDeg <= yDeg ? InputState.Right : InputState.None;
            }

            // Head of dragDirection is in Quadrant 2
            if (dragDirection.X <= 0 && dragDirection.Y >= 0)
            {
                comparedAxis = -Vector2.UnitX;
                xDeg = FindAngleBetweenVector(ref dragDirection, ref comparedAxis);
                comparedAxis = Vector2.UnitY;
                yDeg = FindAngleBetweenVector(ref dragDirection, ref comparedAxis);

                return xDeg <= yDeg ? InputState.Left : InputState.None;
            }

            // Head of dragDirection is in Quadrant 3, check if the input is left or down
            if (dragDirection.X <= 0 && dragDirection.Y <= 0)
            {
                comparedAxis = -Vector2.UnitX;
                xDeg = FindAngleBetweenVector(ref dragDirection, ref comparedAxis);
                comparedAxis = -Vector2.UnitY;
                yDeg = FindAngleBetweenVector(ref dragDirection, ref comparedAxis);

                return xDeg <= yDeg ? InputState.Left : InputState.Down;
            }

            // Head of dragDirection is in Quadrant 4, check if the input is right or down
            comparedAxis = Vector2.UnitX;
            xDeg = FindAngleBetweenVector(ref dragDirection, ref comparedAxis);
            comparedAxis = -Vector2.UnitY;
            yDeg = FindAngleBetweenVector(ref dragDirection, ref comparedAxis);

            return xDeg <= yDeg ? InputState.Right : InputState.Down;
        }

        private static float FindAngleBetweenVector(ref Vector2 v1, ref Vector2 v2)
        {
            Vector2.Dot(ref v1, ref v2, out float dotProd);
            return (float) Math.Acos(dotProd);
        }

        /// <summary>
        ///   Processes the user's input, according to the current state.
        ///   It might change state of the agent.
        /// </summary>
        /// <param name="currentInputState">Current state of the agent.</param>
        private void ProcessInput(InputState currentInputState)
        {
            if (!shouldProcessInput)
                return;

            switch (currentInputState)
            {
                case InputState.Left:
                    if (CurLane != LeftLane && (State == AgentState.Run|| State == AgentState.Slide))
                        State = AgentState.ChangeLaneLeft;
                    break;

                case InputState.Right:
                    if (CurLane != RightLane && (State == AgentState.Run || State == AgentState.Slide))
                        State = AgentState.ChangeLaneRight;
                    break;

                case InputState.Down:
                    if (State == AgentState.Run)
                        State = AgentState.Slide;
                    break;
            }
        }

        /// <summary>
        ///   This method is invoked upon entering each state. It sets initial behaviour of the CharacterScript for
        ///   that state.
        /// </summary>
        /// <param name="agentState">Current state of the agent.</param>
        private void OnEnter(AgentState agentState)
        {
            activeBoundingBox = (agentState == AgentState.Slide) ?
                boundingBoxes[BoundingBoxKeys.Slide] :
                boundingBoxes[BoundingBoxKeys.Normal];

            switch (agentState)
            {
                case AgentState.Run:
                    PlayAnimation(AgentAnimationKeys.Active);
                    break;

                case AgentState.ChangeLaneLeft:
                    OnEnterChangeLane(true);
                    break;

                case AgentState.ChangeLaneRight:
                    OnEnterChangeLane(false);
                    break;

                case AgentState.Slide:
                    PlayAnimation(AgentAnimationKeys.Slide);
                    break;

                case AgentState.Die:
                    PlayAnimation(AgentAnimationKeys.Crash);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(agentState));
            }
        }

        /// <summary>
        ///   Method invoked upon entering ChangeLane state. It caches the start X position, and determines
        ///   X position that will arrive for interpolation and play an animation accordingly.
        /// </summary>
        /// <param name="isChangeLaneLeft">A value indicating if the change is to the left lane.</param>
        private void OnEnterChangeLane(bool isChangeLaneLeft)
        {
            if (isChangeLaneLeft)
            {
                startChangeLanePosX = GetXPosition(CurLane--);
                PlayAnimation(AgentAnimationKeys.DodgeLeft);
            }
            else
            {
                startChangeLanePosX = GetXPosition(CurLane++);
                PlayAnimation(AgentAnimationKeys.DodgeRight);
            }

            targetChangeLanePosX = GetXPosition(CurLane);
        }

        /// <summary>
        ///   Updates the agent according to its state.
        /// </summary>
        private void UpdateState()
        {
            switch (State)
            {
                case AgentState.ChangeLaneLeft:
                case AgentState.ChangeLaneRight:
                    UpdateChangeLane();
                    break;

                case AgentState.Slide:
                    if (playingAnimation.CurrentTime.TotalSeconds >= playingAnimation.Clip.Duration.TotalSeconds)
                        State = AgentState.Run;
                    break;
            }
        }
        /// <summary>
        ///   In ChangeLane state, the agent's X position is determined by Linear interpolation of the current
        ///   animation process.
        /// </summary>
        private void UpdateChangeLane()
        {
            var t = (float) (playingAnimation.CurrentTime.TotalSeconds / playingAnimation.Clip.Duration.TotalSeconds);

            // Interpolate new X position in World coordinate
            var newPosX = MathUtil.Lerp(startChangeLanePosX, targetChangeLanePosX, t);
            Entity.Transform.Position.X = newPosX;

            // Animation ends, changing state
            if (t >= 1.0f)
                State = AgentState.Run;
        }

        /// <summary>
        ///   Helper function for playing an animation given AgentAnimationKey.
        /// </summary>
        /// <param name="key">The animation to play.</param>
        private void PlayAnimation(AgentAnimationKeys key)
        {
            var animationComponent = Entity.Get<AnimationComponent>();

            animationComponent.Play(key.ToString());
            playingAnimation = animationComponent.PlayingAnimations[0];
        }

        /// <summary>
        ///   Returns world X position for the given lane index.
        /// </summary>
        /// <param name="lane">Index of the lane.</param>
        /// <returns>Position of the lane in the X axis.</returns>
        private static float GetXPosition(int lane)
        {
            return (1 - lane) * LaneLength;
        }
    }
}
