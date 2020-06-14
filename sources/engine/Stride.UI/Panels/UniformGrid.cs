// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.ComponentModel;
using System.Diagnostics;

using Stride.Core;
using Stride.Core.Annotations;
using Stride.Core.Mathematics;

namespace Stride.UI.Panels
{
    /// <summary>
    ///   Represents a grid where all the rows and columns have uniform size.
    /// </summary>
    [DataContract(nameof(UniformGrid))]
    [DebuggerDisplay("UniformGrid - Name={Name}")]
    public class UniformGrid : GridBase
    {
        /// <summary>
        ///   The final size of one cell
        /// </summary>
        private Vector3 finalForOneCell;

        private int rows = 1;
        private int columns = 1;
        private int layers = 1;

        /// <summary>
        ///   Gets or sets the number of rows that this <see cref="UniformGrid"/> has.
        /// </summary>
        /// <remarks>The value is coerced to be in the range [1, <see cref="int.MaxValue"/>].</remarks>
        /// <userdoc>The number of rows.</userdoc>
        [DataMember]
        [DataMemberRange(1, 0)]
        [Display(category: LayoutCategory)]
        [DefaultValue(1)]
        public int Rows
        {
            get => rows;
            set
            {
                rows = MathUtil.Clamp(value, 1, int.MaxValue);
                InvalidateMeasure();
            }
        }

        /// <summary>
        ///   Gets or sets the number of columns that the <see cref="UniformGrid"/> has.
        /// </summary>
        /// <remarks>The value is coerced to be in the range [1, <see cref="int.MaxValue"/>].</remarks>
        /// <userdoc>The number of columns.</userdoc>
        [DataMember]
        [DataMemberRange(1, 0)]
        [Display(category: LayoutCategory)]
        [DefaultValue(1)]
        public int Columns
        {
            get => columns;
            set
            {
                columns = MathUtil.Clamp(value, 1, int.MaxValue);
                InvalidateMeasure();
            }
        }

        /// <summary>
        ///   Gets or sets the number of layers that the <see cref="UniformGrid"/> has.
        /// </summary>
        /// <remarks>The value is coerced to be in the range [1, <see cref="int.MaxValue"/>].</remarks>
        /// <userdoc>The number of layers.</userdoc>
        [DataMember]
        [DataMemberRange(1, 0)]
        [Display(category: LayoutCategory)]
        [DefaultValue(1)]
        public int Layers
        {
            get => layers;
            set
            {
                layers = MathUtil.Clamp(value, 1, int.MaxValue);
                InvalidateMeasure();
            }
        }

        protected override Vector3 MeasureOverride(Vector3 availableSizeWithoutMargins)
        {
            // Compute the size available for one cell
            var gridSize = new Vector3(Columns, Rows, Layers);
            var availableForOneCell = new Vector3(availableSizeWithoutMargins.X / gridSize.X,
                                                  availableSizeWithoutMargins.Y / gridSize.Y,
                                                  availableSizeWithoutMargins.Z / gridSize.Z);

            // Measure all the children
            var neededForOneCell = Vector3.Zero;
            foreach (var child in VisualChildrenCollection)
            {
                // Compute the size available for the child depending on its spans values
                var childSpans = GetElementSpanValuesAsFloat(child);
                var availableForChildWithMargin = Vector3.Modulate(childSpans, availableForOneCell);

                child.Measure(availableForChildWithMargin);

                neededForOneCell = new Vector3(
                    Math.Max(neededForOneCell.X, child.DesiredSizeWithMargins.X / childSpans.X),
                    Math.Max(neededForOneCell.Y, child.DesiredSizeWithMargins.Y / childSpans.Y),
                    Math.Max(neededForOneCell.Z, child.DesiredSizeWithMargins.Z / childSpans.Z));
            }

            return Vector3.Modulate(gridSize, neededForOneCell);
        }

        protected override Vector3 ArrangeOverride(Vector3 finalSizeWithoutMargins)
        {
            // Compute the size available for one cell
            var gridSize = new Vector3(Columns, Rows, Layers);
            finalForOneCell = new Vector3(finalSizeWithoutMargins.X / gridSize.X,
                                          finalSizeWithoutMargins.Y / gridSize.Y,
                                          finalSizeWithoutMargins.Z / gridSize.Z);

            // Arrange all the children
            foreach (var child in VisualChildrenCollection)
            {
                // Compute the final size of the child depending on its spans values
                var childSpans = GetElementSpanValuesAsFloat(child);
                var finalForChildWithMargin = Vector3.Modulate(childSpans, finalForOneCell);

                // Set the arrange matrix of the child
                var childOffsets = GetElementGridPositionsAsFloat(child);
                child.DependencyProperties.Set(PanelArrangeMatrixPropertyKey, Matrix.Translation(Vector3.Modulate(childOffsets, finalForOneCell) - finalSizeWithoutMargins / 2));

                // Arrange the child
                child.Arrange(finalForChildWithMargin, IsCollapsed);
            }

            return finalSizeWithoutMargins;
        }

        private void CalculateDistanceToSurroundingModulo(float position, float modulo, float elementCount, out Vector2 distances)
        {
            if (modulo <= 0)
            {
                distances = Vector2.Zero;
                return;
            }

            var validPosition = Math.Max(0, Math.Min(position, elementCount * modulo));
            var inferiorQuotient = Math.Min(elementCount - 1, (float) Math.Floor(validPosition / modulo));

            distances.X = (inferiorQuotient+0) * modulo - validPosition;
            distances.Y = (inferiorQuotient+1) * modulo - validPosition;
        }

        public override Vector2 GetSurroudingAnchorDistances(Orientation direction, float position)
        {
            var gridElements = new Vector3(Columns, Rows, Layers);

            CalculateDistanceToSurroundingModulo(position, finalForOneCell[(int) direction], gridElements[(int) direction], out Vector2 distances);

            return distances;
        }

        /// <summary>
        ///   Gets an element span values as an <see cref="Vector3"/>.
        /// </summary>
        /// <param name="element">The element from which to extract the span values.</param>
        /// <returns>The span values of the element.</returns>
        protected Vector3 GetElementSpanValuesAsFloat(UIElement element)
        {
            var intValues = GetElementSpanValues(element);

            return new Vector3(intValues.X, intValues.Y, intValues.Z);
        }

        /// <summary>
        ///   Gets the positions of an element in the grid as an <see cref="Vector3"/>.
        /// </summary>
        /// <param name="element">The element from which to extract the position values.</param>
        /// <returns>The position of the element.</returns>
        protected Vector3 GetElementGridPositionsAsFloat(UIElement element)
        {
            var intValues = GetElementGridPositions(element);

            return new Vector3(intValues.X, intValues.Y, intValues.Z);
        }
    }
}
