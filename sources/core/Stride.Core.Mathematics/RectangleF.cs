// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Stride.Core.Mathematics
{
    /// <summary>
    ///   Represents a two-dimensional rectangle with floating point coordinates.
    /// Define a RectangleF.
    /// </summary>
    [DataContract("RectangleF")]
    [DataStyle(DataStyle.Compact)]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct RectangleF : IEquatable<RectangleF>
    {
        /// <summary>
        ///   An empty rectangle.
        /// </summary>
        public static readonly RectangleF Empty;

        static RectangleF()
        {
            Empty = new RectangleF();
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="RectangleF"/> struct.
        /// </summary>
        /// <param name="x">The X position of the left edge.</param>
        /// <param name="y">The Y position of the top edge.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        public RectangleF(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        /// <summary>
        ///   Gets or sets the X position of the left edge.
        /// </summary>
        /// <value>The left.</value>
        [DataMemberIgnore]
        public float Left
        {
            get => X;
            set => X = value;
        }

        /// <summary>
        ///   Gets or sets the Y position of the top edge.
        /// </summary>
        /// <value>The top.</value>
        [DataMemberIgnore]
        public float Top
        {
            get => Y;
            set => Y = value;
        }

        /// <summary>
        ///   Gets the X position of the right edge.
        /// </summary>
        /// <value>The right.</value>
        [DataMemberIgnore]
        public float Right => X + Width;

        /// <summary>
        ///   Gets the Y position of the bottom edge.
        /// </summary>
        /// <value>The bottom.</value>
        public float Bottom => Y + Height;

        /// <summary>
        ///   Gets or sets the X position.
        /// </summary>
        /// <value>The X position.</value>
        /// <userdoc>The beginning of the rectangle along the X axis.</userdoc>
        [DataMember(0)]
        public float X;

        /// <summary>
        ///   Gets or sets the Y position.
        /// </summary>
        /// <value>The Y position.</value>
        /// <userdoc>The beginning of the rectangle along the Y axis.</userdoc>
        [DataMember(1)]
        public float Y;

        /// <summary>
        ///   Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        /// <userdoc>The width of the rectangle.</userdoc>
        [DataMember(2)]
        public float Width;

        /// <summary>
        ///   Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        /// <userdoc>The height of the rectangle.</userdoc>
        [DataMember(3)]
        public float Height;

        /// <summary>
        ///   Gets or sets the location of the top-left corner of the rectangle.
        /// </summary>
        /// <value>The location.</value>
        [DataMemberIgnore]
        public Vector2 Location
        {
            get => new Vector2(X, Y);

            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        /// <summary>
        ///   Gets a point that represents the center of the rectangle.
        /// </summary>
        /// <value>The center.</value>
        [DataMemberIgnore]
        public Vector2 Center => new Vector2(X + (Width / 2), Y + (Height / 2));

        /// <summary>
        ///   Gets a value that indicates whether the rectangle is empty.
        /// </summary>
        /// <value>
        ///   <c>true</c> if it is empty; otherwise, <c>false</c>.
        /// </value>
        public bool IsEmpty => (Width == 0) && (Height == 0) && (X == 0) && (Y == 0);

        /// <summary>
        ///   Gets or sets the size of the rectangle.
        /// </summary>
        /// <value>The size of the rectangle.</value>
        [DataMemberIgnore]
        public Size2F Size
        {
            get => new Size2F(Width, Height);

            set
            {
                Width = value.Width;
                Height = value.Height;
            }
        }

        /// <summary>
        ///   Gets the position of the top-left corner of the rectangle.
        /// </summary>
        /// <value>The top-left corner of the rectangle.</value>
        public Vector2 TopLeft => new Vector2(X, Y);

        /// <summary>
        ///   Gets the position of the top-right corner of the rectangle.
        /// </summary>
        /// <value>The top-right corner of the rectangle.</value>
        public Vector2 TopRight => new Vector2(Right, Y);

        /// <summary>
        ///   Gets the position of the bottom-left corner of the rectangle.
        /// </summary>
        /// <value>The bottom-left corner of the rectangle.</value>
        public Vector2 BottomLeft => new Vector2(X, Bottom);

        /// <summary>
        ///   Gets the position of the bottom-right corner of the rectangle.
        /// </summary>
        /// <value>The bottom-right corner of the rectangle.</value>
        public Vector2 BottomRight => new Vector2(Right, Bottom);

        /// <summary>
        ///   Changes the position of the rectangle.
        /// </summary>
        /// <param name="amount">The values to adjust the position of the rectangle by.</param>
        public void Offset(Point amount)
        {
            Offset(amount.X, amount.Y);
        }

        /// <summary>
        ///   Changes the position of the rectangle.
        /// </summary>
        /// <param name="amount">The values to adjust the position of the rectangle by.</param>
        public void Offset(Vector2 amount)
        {
            Offset(amount.X, amount.Y);
        }

        /// <summary>
        ///   Changes the position of the rectangle.
        /// </summary>
        /// <param name="offsetX">Change in the X-position.</param>
        /// <param name="offsetY">Change in the Y-position.</param>
        public void Offset(float offsetX, float offsetY)
        {
            X += offsetX;
            Y += offsetY;
        }

        /// <summary>
        ///   Pushes the edges of the rectangle out by the horizontal and vertical values specified.
        /// </summary>
        /// <param name="horizontalAmount">Value to push the sides out by.</param>
        /// <param name="verticalAmount">Value to push the top and bottom out by.</param>
        public void Inflate(float horizontalAmount, float verticalAmount)
        {
            X -= horizontalAmount;
            Y -= verticalAmount;
            Width += horizontalAmount * 2;
            Height += verticalAmount * 2;
        }

        /// <summary>
        ///   Determines whether this rectangle contains the specified point.
        /// </summary>
        /// <param name="value">The point to evaluate.</param>
        /// <param name="result">When the method returns, contains <c>true</c> if the specified point is contained within this rectangle; <c>false</c> otherwise.</param>
        public void Contains(ref Vector2 value, out bool result)
        {
            result = value.X >= this.X && value.X <= Right && value.Y >= this.Y && value.Y <= Bottom;
        }

        /// <summary>
        ///   Determines whether this rectangle entirely contains a specified rectangle.
        /// </summary>
        /// <param name="value">The rectangle to evaluate.</param>
        public bool Contains(Rectangle value)
        {
            return (X <= value.X) && (value.Right <= Right) && (Y <= value.Y) && (value.Bottom <= Bottom);
        }

        /// <summary>
        ///   Determines whether this rectangle entirely contains a specified rectangle.
        /// </summary>
        /// <param name="value">The rectangle to evaluate.</param>
        /// <param name="result">When the method returns, contains <c>true</c> if this rectangle entirely contains the specified rectangle, or <c>false</c> if not.</param>
        public void Contains(ref RectangleF value, out bool result)
        {
            result = (X <= value.X) && (value.Right <= Right) && (Y <= value.Y) && (value.Bottom <= Bottom);
        }

        /// <summary>
        ///   Determines whether this rectangle contains the specified point.
        /// </summary>
        /// <param name="x">Coordinate X of the point.</param>
        /// <param name="y">Coordinate Y of the point.</param>
        /// <returns><c>true</c> if the point is inside the rectangle, otherwise <c>false</c>.</returns>
        public bool Contains(float x, float y)
        {
            return x >= X && x <= Right && y >= Y && y <= Bottom;
        }

        /// <summary>
        ///   Determines whether this rectangle contains the specified point.
        /// </summary>
        /// <param name="value">The point to evaluate.</param>
        /// <returns><c>true</c> if the point is inside the rectangle, otherwise <c>false</c>.</returns>
        public bool Contains(Vector2 value)
        {
            return Contains(value.X, value.Y);
        }

        /// <summary>
        ///   Determines whether this rectangle contains the specified point.
        /// </summary>
        /// <param name="value">The point to evaluate.</param>
        /// <returns><c>true</c> if the point is inside the rectangle, otherwise <c>false</c>.</returns>
        public bool Contains(Int2 value)
        {
            return Contains(value.X, value.Y);
        }

        /// <summary>
        ///   Determines whether this rectangle contains the specified point.
        /// </summary>
        /// <param name="value">The point to evaluate.</param>
        /// <returns><c>true</c> if the point is inside the rectangle, otherwise <c>false</c>.</returns>
        public bool Contains(Point value)
        {
            return Contains(value.X, value.Y);
        }

        /// <summary>
        ///   Determines whether a specified rectangle intersects with this rectangle.
        /// </summary>
        /// <param name="value">The rectangle to evaluate.</param>
        public bool Intersects(RectangleF value)
        {
            Intersects(ref value, out bool result);
            return result;
        }

        /// <summary>
        ///   Determines whether a specified rectangle intersects with this rectangle.
        /// </summary>
        /// <param name="value">The rectangle to evaluate.</param>
        /// <param name="result">When the method returns, contains <c>true</c> if the specified rectangle intersects with this one; <c>false</c> otherwise.</param>
        public void Intersects(ref RectangleF value, out bool result)
        {
            result = (value.X < Right) && (X < value.Right) && (value.Y < Bottom) && (Y < value.Bottom);
        }

        /// <summary>
        ///   Creates a rectangle defining the area where one rectangle overlaps with another rectangle.
        /// </summary>
        /// <param name="value1">The first rectangle to compare.</param>
        /// <param name="value2">The second rectangle to compare.</param>
        /// <returns>The intersection rectangle.</returns>
        public static RectangleF Intersect(RectangleF value1, RectangleF value2)
        {
            Intersect(ref value1, ref value2, out RectangleF result);
            return result;
        }

        /// <summary>
        ///   Creates a rectangle defining the area where one rectangle overlaps with another rectangle.
        /// </summary>
        /// <param name="value1">The first rectangle to compare.</param>
        /// <param name="value2">The second rectangle to compare.</param>
        /// <param name="result">When the method returns, contains the area where the two rectangles overlap.</param>
        public static void Intersect(ref RectangleF value1, ref RectangleF value2, out RectangleF result)
        {
            float newLeft = (value1.X > value2.X) ? value1.X : value2.X;
            float newTop = (value1.Y > value2.Y) ? value1.Y : value2.Y;
            float newRight = (value1.Right < value2.Right) ? value1.Right : value2.Right;
            float newBottom = (value1.Bottom < value2.Bottom) ? value1.Bottom : value2.Bottom;

            if ((newRight > newLeft) && (newBottom > newTop))
            {
                result = new RectangleF(newLeft, newTop, newRight - newLeft, newBottom - newTop);
            }
            else
            {
                result = Empty;
            }
        }

        /// <summary>
        ///   Creates a new rectangle that exactly contains two other rectangles.
        /// </summary>
        /// <param name="value1">The first rectangle to contain.</param>
        /// <param name="value2">The second rectangle to contain.</param>
        /// <returns>The union rectangle.</returns>
        public static RectangleF Union(RectangleF value1, RectangleF value2)
        {
            Union(ref value1, ref value2, out RectangleF result);
            return result;
        }

        /// <summary>
        ///   Creates a new rectangle that exactly contains two other rectangles.
        /// </summary>
        /// <param name="value1">The first rectangle to contain.</param>
        /// <param name="value2">The second rectangle to contain.</param>
        /// <param name="result">When the method returns, contains the rectangle that must be the union of the first two rectangles.</param>
        public static void Union(ref RectangleF value1, ref RectangleF value2, out RectangleF result)
        {
            var left = Math.Min(value1.Left, value2.Left);
            var right = Math.Max(value1.Right, value2.Right);
            var top = Math.Min(value1.Top, value2.Top);
            var bottom = Math.Max(value1.Bottom, value2.Bottom);

            result = new RectangleF(left, top, right - left, bottom - top);
        }

        /// <summary>
        ///   Determines whether the specified <see cref="object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;

            return obj is RectangleF rectangle && Equals(rectangle);
        }

        /// <inheritdoc/>
        public bool Equals(RectangleF other)
        {
            return MathUtil.NearEqual(other.Left, Left) &&
                   MathUtil.NearEqual(other.Right, Right) &&
                   MathUtil.NearEqual(other.Top, Top) &&
                   MathUtil.NearEqual(other.Bottom, Bottom);
        }

        /// <summary>
        ///   Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        ///   A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = X.GetHashCode();
                result = (result * 397) ^ Y.GetHashCode();
                result = (result * 397) ^ Width.GetHashCode();
                result = (result * 397) ^ Height.GetHashCode();
                return result;
            }
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(RectangleF left, RectangleF right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(RectangleF left, RectangleF right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Performs an explicit conversion to <see cref="Rectangle"/> structure.
        /// </summary>
        /// <remarks>Performs direct float to int conversion, any fractional data is truncated.</remarks>
        /// <param name="value">The source <see cref="RectangleF"/> value.</param>
        /// <returns>A converted <see cref="Rectangle"/> structure.</returns>
        public static explicit operator Rectangle(RectangleF value)
        {
            return new Rectangle((int) value.X, (int) value.Y, (int) value.Width, (int) value.Height);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "X:{0} Y:{1} Width:{2} Height:{3}", X, Y, Width, Height);
        }
    }
}
