// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Threading.Tasks;

using Stride.Editor.EditorGame.Game;
using Stride.Engine;

namespace Stride.Assets.Presentation.AssetEditors.EntityHierarchyEditor.Game
{
    public interface IEditorGameComponentGizmoService : IEditorGameService
    {
        bool FixedSize { get; set; }

        float SceneUnit { get; }

        void UpdateGizmoEntitiesSelection(Entity entity, bool isSelected);

        Entity GetContentEntityUnderMouse();
    }
}
