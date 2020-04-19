// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;

using Xenko.Core.Annotations;
using Xenko.Core.Quantum;

namespace Xenko.Core.Presentation.Quantum.Presenters
{
    public interface INodePresenterFactoryInternal : INodePresenterFactory
    {
        IReadOnlyCollection<INodePresenterCommand> AvailableCommands { get; }

        void CreateChildren([NotNull] IInitializingNodePresenter parentPresenter, IObjectNode objectNode, [CanBeNull] IPropertyProviderViewModel propertyProvider);
    }
}
