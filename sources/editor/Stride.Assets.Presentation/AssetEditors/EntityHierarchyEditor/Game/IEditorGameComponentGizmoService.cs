// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Threading.Tasks;

using Xenko.Editor.EditorGame.Game;
using Xenko.Engine;

namespace Xenko.Assets.Presentation.AssetEditors.EntityHierarchyEditor.Game
{
    public interface IEditorGameComponentGizmoService : IEditorGameService
    {
        bool FixedSize { get; set; }

        float SceneUnit { get; }

        void UpdateGizmoEntitiesSelection(Entity entity, bool isSelected);

        Entity GetContentEntityUnderMouse();
    }
}
