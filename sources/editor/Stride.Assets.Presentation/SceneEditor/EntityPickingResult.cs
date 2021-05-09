// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Diagnostics;
using System.Linq;

using Stride.Core;
using Stride.Engine;

namespace Stride.Assets.Presentation.SceneEditor
{
    /// <summary>
    ///   Represents the result of trying to pick an <see cref="Engine.Entity"/> inside the editor.
    /// </summary>
    /// <remarks>
    ///   This is the result of picking retunrned by <see cref="PickingRenderFeature"/> and <see cref="PickingSceneRenderer"/>.
    /// </remarks>
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public struct EntityPickingResult
    {
        /// <summary>
        ///   The picked Entity.
        /// </summary>
        /// <remarks>
        ///   The value of this field may be <c>null</c> if no Entity was found.
        /// </remarks>
        public Entity Entity;

        /// <summary>
        ///   The component identifier of the picked Entity.
        /// </summary>
        public int ComponentId;

        /// <summary>
        ///   The mesh node index of the picked Entity.
        /// </summary>
        public int MeshNodeIndex;

        /// <summary>
        ///   The material index of the picked Entity.
        /// </summary>
        public int MaterialIndex;

        /// <summary>
        ///   The instance index of the picked Entity.
        /// </summary>
        public int InstanceId;

        /// <summary>
        ///   Gets the component of the picked Entity.
        /// </summary>
        public EntityComponent Component
        {
            get
            {
                if (Entity != null)
                {
                    int component = ComponentId;
                    return Entity.Components.First(x => component == RuntimeIdHelper.ToRuntimeId(x));
                }
                return null;
            }
        }

        public override string ToString()
        {
            return $"ComponentId: {ComponentId}, MeshNodeIndex: {MeshNodeIndex}, MaterialIndex: {MaterialIndex}";
        }

        private string GetDebuggerDisplay()
        {
            return ToString();
        }
    }
}
