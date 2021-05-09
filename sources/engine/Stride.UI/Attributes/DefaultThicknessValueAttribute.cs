// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// See the LICENSE.md file in the project root for full license information.

using System.ComponentModel;

namespace Stride.UI.Attributes
{
    /// <summary>
    ///   Specifies the default value for a thickness property.
    /// </summary>
    public class DefaultThicknessValueAttribute : DefaultValueAttribute
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref="DefaultThicknessValueAttribute"/> attribute with specific
        ///   lengths applied to each side of the rectangle.
        /// </summary>
        /// <param name="left">The thickness for the left side of the cuboid.</param>
        /// <param name="top">The thickness for the upper side of the cuboid.</param>
        /// <param name="right">The thickness for the right side of the cuboid</param>
        /// <param name="bottom">The thickness for the lower side of the cuboid.</param>
        public DefaultThicknessValueAttribute(float left, float top, float right, float bottom)
           : base(null)
        {
            Bottom = bottom;
            Left = left;
            Right = right;
            Top = top;
            Front = 0;
            Back = 0;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="DefaultThicknessValueAttribute"/> attribute with specific
        ///   lengths applied to each side of the cuboid.
        /// </summary>
        /// <param name="left">The thickness for the left side of the cuboid.</param>
        /// <param name="top">The thickness for the upper side of the cuboid.</param>
        /// <param name="back">The thickness for the Back side of the cuboid.</param>
        /// <param name="right">The thickness for the right side of the cuboid</param>
        /// <param name="bottom">The thickness for the lower side of the cuboid.</param>
        /// <param name="front">The thickness for the front side of the cuboid.</param>
        public DefaultThicknessValueAttribute(float left, float top, float back, float right, float bottom, float front)
             : base(null)
        {
            Bottom = bottom;
            Left = left;
            Right = right;
            Top = top;
            Front = front;
            Back = back;
        }


        /// <summary>
        ///   Gets the length of the back side of the bounding cuboid.
        /// </summary>
        public float Back { get; }

        /// <summary>
        ///   Gets the length of the bottom side of the bounding rectangle or cuboid.
        /// </summary>
        public float Bottom;

        /// <summary>
        ///   Gets the length of the front side of the bounding cuboid.
        /// </summary>
        public float Front { get; }

        /// <summary>
        ///   Gets the length of the left side of the bounding rectangle or cuboid.
        /// </summary>
        public float Left { get; }

        /// <summary>
        ///   Gets the length of the right side of the bounding rectangle or cuboid.
        /// </summary>
        public float Right { get; }

        /// <summary>
        ///   Gets the length of the upper side of the bounding rectangle or cuboid.
        /// </summary>
        public float Top { get; }


        /// <summary>
        ///   Gets the <see cref="Thickness"/> value.
        /// </summary>
        public override object Value => new Thickness(Left, Top, Back, Right, Bottom, Front);
    }
}
