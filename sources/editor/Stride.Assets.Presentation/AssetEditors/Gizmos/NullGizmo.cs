// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Linq;
using System.Reflection;

using Stride.Assets.Presentation.SceneEditor;
using Stride.Engine;
using Stride.Rendering;
using Stride.Rendering.Compositing;

namespace Stride.Assets.Presentation.AssetEditors.Gizmos
{
    public class NullGizmo<T> : EntityGizmo<T> where T : EntityComponent, new()
    {
        protected override Entity Create()
        {
            return null;
        }

        public NullGizmo(EntityComponent component) : base(component)
        {
        }
    }
}
