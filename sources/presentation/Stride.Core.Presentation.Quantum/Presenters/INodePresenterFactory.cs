// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;

using Xenko.Core.Annotations;
using Xenko.Core.Quantum;

namespace Xenko.Core.Presentation.Quantum.Presenters
{
    public interface INodePresenterFactory
    {
        INodePresenter CreateNodeHierarchy([NotNull] IObjectNode rootNode, [NotNull] GraphNodePath rootNodePath, [CanBeNull] IPropertyProviderViewModel propertyProvider = null);

        bool IsPrimitiveType(Type type);
    }
}
