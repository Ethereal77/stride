// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

using Stride.Core.Assets.Editor.Quantum.NodePresenters.Commands;
using Stride.Core.Presentation.Quantum.Presenters;
using Stride.Assets.Scripts;

namespace Stride.Assets.Presentation.NodePresenters.Commands
{
    public class SetSymbolReferenceCommand : ChangeValueCommandBase
    {
        /// <summary>
        /// The name of this command.
        /// </summary>
        public const string CommandName = "SetSymbolReference";

        /// <inheritdoc/>
        public override string Name => CommandName;

        /// <inheritdoc/>
        public override bool CanAttach(INodePresenter nodePresenter)
        {
            var member = nodePresenter as MemberNodePresenter;
            return nodePresenter.Type == typeof(string) && (member?.MemberAttributes.OfType<ScriptVariableReferenceAttribute>().Any() ?? false);
        }

        /// <inheritdoc/>
        protected override object ChangeValue(object currentValue, object parameter, object preExecuteResult)
        {
            var symbol = (Symbol)parameter;
            return symbol.Name;
        }
    }
}
