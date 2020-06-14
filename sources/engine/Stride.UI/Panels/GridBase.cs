// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Diagnostics;

using Stride.Core;
using Stride.Core.Annotations;
using Stride.Core.Mathematics;

namespace Stride.UI.Panels
{
    /// <summary>
    ///   Represents the base class for all the grid-like controls.
    /// </summary>
    [DataContract(nameof(GridBase))]
    [DebuggerDisplay("GridBase - Name={Name}")]
    public abstract class GridBase : Panel
    {
        /// <summary>
        ///   The key to the Row attached dependency property. This defines the row an item is inserted into.
        /// </summary>
        /// <remarks>The value is coerced to be in the range [0, <see cref="int.MaxValue"/>].</remarks>
        /// <remarks>The first row has index 0.</remarks>
        [DataMemberRange(0, 0)]
        [Display(category: LayoutCategory)]
        public static readonly PropertyKey<int> RowPropertyKey = DependencyPropertyFactory.RegisterAttached(nameof(RowPropertyKey), typeof(GridBase), 0, CoerceGridPositionsValue, InvalidateParentGridMeasure);

        /// <summary>
        ///   The key to the RowSpan attached dependency property. This defines the number of rows an item takes.
        /// </summary>
        /// <remarks>The value is coerced to be in the range [1, <see cref="int.MaxValue"/>].</remarks>
        [DataMemberRange(1, 0)]
        [Display(category: LayoutCategory)]
        public static readonly PropertyKey<int> RowSpanPropertyKey = DependencyPropertyFactory.RegisterAttached(nameof(RowSpanPropertyKey), typeof(GridBase), 1, CoerceSpanValue, InvalidateParentGridMeasure);

        /// <summary>
        ///   The key to the Column attached dependency property. This defines the column an item is inserted into.
        /// </summary>
        /// <remarks>The value is coerced to be in the range [0, <see cref="int.MaxValue"/>].</remarks>
        /// <remarks>The first column has index 0.</remarks>
        [DataMemberRange(0, 0)]
        [Display(category: LayoutCategory)]
        public static readonly PropertyKey<int> ColumnPropertyKey = DependencyPropertyFactory.RegisterAttached(nameof(ColumnPropertyKey), typeof(GridBase), 0, CoerceGridPositionsValue, InvalidateParentGridMeasure);

        /// <summary>
        ///   The key to the ColumnSpan attached dependency property. This defines the number of columns an item takes.
        /// </summary>
        /// <remarks>The value is coerced to be in the range [1, <see cref="int.MaxValue"/>].</remarks>
        [DataMemberRange(1, 0)]
        [Display(category: LayoutCategory)]
        public static readonly PropertyKey<int> ColumnSpanPropertyKey = DependencyPropertyFactory.RegisterAttached(nameof(ColumnSpanPropertyKey), typeof(GridBase), 1, CoerceSpanValue, InvalidateParentGridMeasure);

        /// <summary>
        ///   The key to the Layer attached dependency property. This defines the layer an item is inserted into.
        /// </summary>
        /// <remarks>The value is coerced to be in the range [0, <see cref="int.MaxValue"/>].</remarks>
        /// <remarks>The first layer has index 0.</remarks>
        [DataMemberRange(0, 0)]
        [Display(category: LayoutCategory)]
        public static readonly PropertyKey<int> LayerPropertyKey = DependencyPropertyFactory.RegisterAttached(nameof(LayerPropertyKey), typeof(GridBase), 0, CoerceGridPositionsValue, InvalidateParentGridMeasure);

        /// <summary>
        ///   The key to the LayerSpan attached dependency property. This defines the number of layers an item takes.
        /// </summary>
        /// <remarks>The value is coerced to be in the range [1, <see cref="int.MaxValue"/>].</remarks>
        [DataMemberRange(1, 0)]
        [Display(category: LayoutCategory)]
        public static readonly PropertyKey<int> LayerSpanPropertyKey = DependencyPropertyFactory.RegisterAttached(nameof(LayerSpanPropertyKey), typeof(GridBase), 1, CoerceSpanValue, InvalidateParentGridMeasure);


        private static void InvalidateParentGridMeasure(object propertyowner, PropertyKey<int> propertykey, int propertyoldvalue)
        {
            var element = (UIElement) propertyowner;
            var parentGridBase = element.Parent as GridBase;

            parentGridBase?.InvalidateMeasure();
        }

        /// <summary>
        ///   Coerces the value of <see cref="RowPropertyKey"/>, <see cref="ColumnPropertyKey"/>, or <see cref="LayerPropertyKey"/> to
        ///   be between 0 and <see cref="int.MaxValue"/>.
        /// </summary>
        /// <param name="value">By reference, the value to be coerced.</param>
        private static void CoerceGridPositionsValue(ref int value)
        {
            value = MathUtil.Clamp(value, 0, int.MaxValue);
        }

        /// <summary>
        ///   Coerces the value of <see cref="RowPropertyKey"/>, <see cref="ColumnPropertyKey"/>, or <see cref="LayerPropertyKey"/> to
        ///   be between 1 and <see cref="int.MaxValue"/>.
        /// </summary>
        /// <param name="value"></param>
        private static void CoerceSpanValue(ref int value)
        {
            value = MathUtil.Clamp(value, 1, int.MaxValue);
        }

        /// <summary>
        ///   Gets an element span values as an <see cref="Int3"/>.
        /// </summary>
        /// <param name="element">The element from which to extract the span values.</param>
        /// <returns>The span values of the <paramref name="element"/>.</returns>
        protected virtual Int3 GetElementSpanValues(UIElement element)
        {
            return new Int3(element.DependencyProperties.Get(ColumnSpanPropertyKey),
                            element.DependencyProperties.Get(RowSpanPropertyKey),
                            element.DependencyProperties.Get(LayerSpanPropertyKey));
        }

        /// <summary>
        ///   Get the positions of an element in the grid as an <see cref="Int3"/>.
        /// </summary>
        /// <param name="element">The element from which to extract the position values.</param>
        /// <returns>The position of the <paramref name="element"/>.</returns>
        protected virtual Int3 GetElementGridPositions(UIElement element)
        {
            return new Int3(element.DependencyProperties.Get(ColumnPropertyKey),
                            element.DependencyProperties.Get(RowPropertyKey),
                            element.DependencyProperties.Get(LayerPropertyKey));
        }
    }
}
