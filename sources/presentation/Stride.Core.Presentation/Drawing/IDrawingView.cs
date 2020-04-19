// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Core.Presentation.Drawing
{
    public interface IDrawingView
    {
        /// <summary>
        /// Gets the model.
        /// </summary>
        IDrawingModel Model { get; }
        
        /// <summary>
        /// Invalidates the drawing (not blocking the UI thread)
        /// </summary>
        void InvalidateDrawing();
    }
}
