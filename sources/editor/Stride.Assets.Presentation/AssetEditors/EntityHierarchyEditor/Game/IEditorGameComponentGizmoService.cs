// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

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
