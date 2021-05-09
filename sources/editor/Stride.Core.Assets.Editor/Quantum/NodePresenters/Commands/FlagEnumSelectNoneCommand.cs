// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Linq;

using Stride.Core.Extensions;
using Stride.Core.Presentation.Extensions;

namespace Stride.Core.Assets.Editor.Quantum.NodePresenters.Commands
{
    public class FlagEnumSelectNoneCommand : FlagEnumSelectionCommandBase
    {
        /// <summary>
        /// The name of this command.
        /// </summary>
        public const string CommandName = "FlagEnumSelectNone";

        /// inheritdoc/>
        public override string Name => CommandName;

        /// inheritdoc/>
        protected override Enum UpdateSelection(Enum currentValue)
        {
            return EnumExtensions.GetEnum(currentValue.GetType(), Enumerable.Empty<Enum>());
        }
    }
}
