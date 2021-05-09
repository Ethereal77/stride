// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Assets.Presentation.AssetEditors.GameEditor.Game;
using Stride.Editor.EditorGame.Game;
using Stride.Engine;

namespace Stride.Assets.Presentation.AssetEditors.EntityHierarchyEditor.Game
{
    public interface IEditorGameMaterialHighlightService : IEditorGameService
    {
        /// <summary>
        /// The last entity which has one if its material highlighted (null when nothing is highlighted) 
        /// </summary>/// 
        Entity HighlightedEntity { get; }

        /// <summary>
        /// The index of the material in the last entity which has one if its material highlighted (-1 when nothing is highlighted) 
        /// </summary>
        int HighlightedMaterialIndex { get; }

        /// <summary>
        /// The index of the mesh node in the last entity which has one if its material highlighted (-1 when nothing is highlighted) 
        /// </summary>
        int HighlightedMeshNodeIndex { get; }
    }
}
