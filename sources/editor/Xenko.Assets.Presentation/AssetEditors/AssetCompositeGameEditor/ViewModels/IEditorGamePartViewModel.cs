// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Xenko.Core;

namespace Xenko.Assets.Presentation.AssetEditors.AssetCompositeGameEditor.ViewModels
{
    /// <summary>
    /// An interface for view models that represents asset parts in an editor of <see cref="Xenko.Core.Assets.AssetComposite"/>.
    /// </summary>
    /// // TODO: replace with IIdentifiable
    public interface IEditorGamePartViewModel : IDestroyable
    {
        /// <summary>
        /// Gets the id of this part.
        /// </summary>
        AbsoluteId Id { get; }
    }
}
