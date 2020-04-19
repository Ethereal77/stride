// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Threading.Tasks;

using Xenko.Editor.EditorGame.ViewModels;
using Xenko.Graphics;
using Xenko.Rendering.Compositing;

namespace Xenko.Assets.Presentation.AssetEditors.EntityHierarchyEditor.Services
{
    /// <summary>
    /// Cubemap capture service.
    /// </summary>
    public interface IEditorGameCubemapService : IEditorGameViewModelService
    {
        /// <summary>
        /// Captures a cubemap from the current camera position, using <see cref="GraphicsCompositor.SingleView"/>.
        /// </summary>
        /// <remarks>
        /// This cubemap can then be prefiltered to be used for diffuse or specular lighting.
        /// </remarks>
        /// <returns>The captured cubemap.</returns>
        Task<Image> CaptureCubemap();
    }
}
