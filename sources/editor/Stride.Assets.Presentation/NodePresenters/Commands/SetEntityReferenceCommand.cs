// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;

using Stride.Core.Assets.Editor.Quantum.NodePresenters.Commands;
using Stride.Core.Extensions;
using Stride.Core.Presentation.Quantum.Presenters;
using Stride.Assets.Presentation.AssetEditors.EntityHierarchyEditor.ViewModels;
using Stride.Engine;

namespace Stride.Assets.Presentation.NodePresenters.Commands
{
    public class SetEntityReferenceCommand : ChangeValueCommandBase
    {
        /// <summary>
        /// The name of this command.
        /// </summary>
        public const string CommandName = "SetEntityReference";

        /// <inheritdoc/>
        public override string Name => CommandName;

        /// <inheritdoc/>
        public override bool CanAttach(INodePresenter nodePresenter)
        {
            return typeof(Entity).IsAssignableFrom(nodePresenter.Type);
        }

        /// <inheritdoc/>
        protected override object ChangeValue(object currentValue, object parameter, object preExecuteResult)
        {
            var subEntity = (EntityViewModel)parameter;
            return subEntity?.AssetSideEntity;
        }
    }

}
