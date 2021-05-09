// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Core.Presentation.Drawing
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
