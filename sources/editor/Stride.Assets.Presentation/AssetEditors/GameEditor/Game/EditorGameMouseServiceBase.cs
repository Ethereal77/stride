// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;

using Stride.Core.Annotations;
using Stride.Editor.EditorGame.Game;

namespace Stride.Assets.Presentation.AssetEditors.GameEditor.Game
{
    /// <summary>
    ///   Represents the base class for <see cref="IEditorGameService"/> that can take control of the mouse.
    /// </summary>
    public abstract class EditorGameMouseServiceBase : EditorGameServiceBase, IEditorGameMouseService
    {
        private readonly List<IEditorGameMouseService> mouseServices = new List<IEditorGameMouseService>();

        /// <inheritdoc/>
        public abstract bool IsControllingMouse { get; protected set; }

        /// <summary>
        ///   Gets or sets whether the material selection mode is currently active.
        /// </summary>
        public override bool IsActive { get; set; } = true;

        /// <inheritdoc/>
        public bool IsMouseAvailable => mouseServices.All(x => x == this || !x.IsControllingMouse);

        internal void RegisterMouseServices([NotNull] EditorGameServiceRegistry serviceRegistry)
        {
            foreach (var service in serviceRegistry.Services.OfType<IEditorGameMouseService>())
            {
                mouseServices.Add(service);
            }
        }
    }
}
