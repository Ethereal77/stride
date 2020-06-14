// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Core.Mathematics;
using Stride.Games;

namespace Stride.UI
{
    /// <summary>
    ///   Defines the interface for updating the state of an <see cref="UIElement"/>.
    /// </summary>
    public interface IUIElementUpdate
    {
        /// <summary>
        ///   Updates the time-dependent state of the <see cref="UIElement"/>.
        /// </summary>
        /// <param name="time">The current time of the game.</param>
        void Update(GameTime time);

        /// <summary>
        ///   Updates the world matrix of the <see cref="UIElement"/>, recursing into its children.
        /// </summary>
        /// <param name="parentWorldMatrix">The world matrix of the parent.</param>
        /// <param name="parentWorldChanged">A value indicating if the world matrix provided by the parent has changed.</param>
        void UpdateWorldMatrix(ref Matrix parentWorldMatrix, bool parentWorldChanged);

        /// <summary>
        ///   Updates the <see cref="UIElement.RenderOpacity"/>, <see cref="UIElement.DepthBias"/> and <see cref="UIElement.IsHierarchyEnabled"/>
        ///   state fields of the <see cref="UIElement"/>, recursing into its children.
        /// </summary>
        /// <param name="elementBias">The depth bias value for the current element computed by the parent.</param>
        void UpdateElementState(int elementBias);
    }
}
