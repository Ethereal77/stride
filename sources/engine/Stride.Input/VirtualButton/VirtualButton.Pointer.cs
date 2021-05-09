// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Input
{
    /// <summary>
    ///   Represents a virtual button (a key from a keyboard, a mouse button, an axis of a joystick, etc.).
    /// </summary>
    public partial class VirtualButton
    {
        /// <summary>
        ///   Represents a mouse virtual button.
        /// </summary>
        public class Pointer : VirtualButton
        {
            private Pointer(string name, int id, bool isPositiveAndNegative)
                : this(name, id, -1, isPositiveAndNegative)
            {
            }

            private Pointer(Pointer parent, int pointerId)
                : this(parent.ShortName, parent.Id, pointerId, parent.IsPositiveAndNegative)
            {
            }

            protected Pointer(string name, int id, int pointerId, bool isPositiveAndNegative)
                : base(name, VirtualButtonType.Pointer, id, isPositiveAndNegative)
            {
                PointerId = pointerId;
            }

            /// <summary>
            ///   The pointer Id.
            /// </summary>
            public readonly int PointerId;

            /// <summary>
            ///   Returns a pointer button for the given pointer Id.
            /// </summary>
            /// <param name="pointerId">the Id of the pointer.</param>
            /// <returns>An pointer button for the given pointer Id.</returns>
            public Pointer WithId(int pointerId)
            {
                return new Pointer(this, pointerId);
            }

            /// <summary>
            ///   Gets the current state of pointers.
            /// </summary>
            public static readonly Pointer State = new Pointer("State", 0, false);

            /// <summary>
            ///   Gets the X component of the Pointer <see cref="PointerPoint.Position"/>.
            /// </summary>
            public static readonly Pointer PositionX = new Pointer("PositionX", 1, true);

            /// <summary>
            ///   Gets the Y component of the Pointer <see cref="PointerPoint.Position"/>.
            /// </summary>
            public static readonly Pointer PositionY = new Pointer("PositionY", 2, true);

            /// <summary>
            ///   Gets the X component of the Pointer <see cref="PointerPoint.Delta"/>.
            /// </summary>
            public static readonly Pointer DeltaX = new Pointer("DeltaX", 3, true);

            /// <summary>
            ///   Gets the Y component of the Pointer <see cref="PointerPoint.Delta"/>.
            /// </summary>
            public static readonly Pointer DeltaY = new Pointer("DeltaY", 4, true);

            protected override string BuildButtonName()
            {
                return PointerId < 0 ? base.BuildButtonName() : Type.ToString() + PointerId + "." + ShortName;
            }

            public override float GetValue(InputManager manager)
            {
                int index = Id & TypeIdMask;
                switch (index)
                {
                    case 0:
                        return IsDown(manager) ? 1.0f : 0.0f;
                    case 1:
                        return FromFirstMatchingEvent(manager, GetPositionX);
                    case 2:
                        return FromFirstMatchingEvent(manager, GetPositionY);
                    case 3:
                        return FromFirstMatchingEvent(manager, GetDeltaX);
                    case 4:
                        return FromFirstMatchingEvent(manager, GetDeltaY);
                }

                return 0.0f;
            }

            public override bool IsDown(InputManager manager)
            {
                return Index == 0 && AnyPointerInState(manager, GetDownPointers);
            }

            public override bool IsPressed(InputManager manager)
            {
                return Index == 0 && AnyPointerInState(manager, GetPressedPointers);
            }

            public override bool IsReleased(InputManager manager)
            {
                return Index == 0 && AnyPointerInState(manager, GetReleasedPointers);
            }

            private float FromFirstMatchingEvent(InputManager manager, Func<PointerEvent, float> valueGetter)
            {
                foreach (var pointerEvent in manager.PointerEvents)
                {
                    if (PointerId < 0 || pointerEvent.PointerId == PointerId)
                        return valueGetter(pointerEvent);
                }
                return 0.0f;
            }

            private bool AnyPointerInState(InputManager manager, Func<IPointerDevice, Core.Collections.IReadOnlySet<PointerPoint>> stateGetter)
            {
                foreach (var pointerDevice in manager.Pointers)
                {
                    foreach (var pointerPoint in stateGetter(pointerDevice))
                    {
                        if (PointerId < 0 || pointerPoint.Id == PointerId)
                            return true;
                    }
                }
                return false;
            }

            private Core.Collections.IReadOnlySet<PointerPoint> GetDownPointers(IPointerDevice device)
            {
                return device.DownPointers;
            }

            private Core.Collections.IReadOnlySet<PointerPoint> GetPressedPointers(IPointerDevice device)
            {
                return device.DownPointers;
            }

            private Core.Collections.IReadOnlySet<PointerPoint> GetReleasedPointers(IPointerDevice device)
            {
                return device.DownPointers;
            }

            private float GetPositionX(PointerEvent pointerEvent)
            {
                return pointerEvent.Position.X;
            }

            private float GetPositionY(PointerEvent pointerEvent)
            {
                return pointerEvent.Position.Y;
            }

            private float GetDeltaX(PointerEvent pointerEvent)
            {
                return pointerEvent.DeltaPosition.X;
            }

            private float GetDeltaY(PointerEvent pointerEvent)
            {
                return pointerEvent.DeltaPosition.Y;
            }
        }
    }
}
