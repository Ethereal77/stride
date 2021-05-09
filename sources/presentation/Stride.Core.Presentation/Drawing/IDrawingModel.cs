// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Core.Presentation.Drawing
{
    public interface IDrawingModel
    {
        /// <summary>
        /// Attaches this item with the specified drawing view.
        /// </summary>
        /// <param name="view"></param>
        void Attach(IDrawingView view);

        /// <summary> 
        /// Detaches this item with the specified drawing view.
        /// </summary>
        /// <param name="view"></param>
        void Detach(IDrawingView view);

        /// <summary>
        /// Renders the canvas with the specified renderer.
        /// </summary>
        /// <param name="context">The drawing context.</param>
        /// <param name="width">The available width.</param>
        /// <param name="height">The available height.</param>
        void Render(IDrawingContext context, double width, double height);

        /// <summary>
        /// Updates.
        /// </summary>
        /// <param name="updateData">If set to <c>true</c> all data collections will be updated.</param>
        void Update(bool updateData);
    }
}
