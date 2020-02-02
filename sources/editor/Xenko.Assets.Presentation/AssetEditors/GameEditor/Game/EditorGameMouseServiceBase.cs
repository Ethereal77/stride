// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;
using System.Linq;

using Xenko.Core.Annotations;
using Xenko.Editor.EditorGame.Game;

namespace Xenko.Assets.Presentation.AssetEditors.GameEditor.Game
{
    /// <summary>
    /// Base class for editor game service that can take control of the mouse.
    /// </summary>
    public abstract class EditorGameMouseServiceBase : EditorGameServiceBase, IEditorGameMouseService
    {
        private readonly List<IEditorGameMouseService> mouseServices = new List<IEditorGameMouseService>();

        /// <inheritdoc/>
        public abstract bool IsControllingMouse { get; protected set; }

        /// <summary>
        /// Gets or sets whether the material selection mode is currently active.
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
