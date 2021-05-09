// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;

using Stride.Core.Presentation.Quantum;
using Stride.Core.Quantum;

namespace Stride.Assets.Presentation.AssetEditors.VisualScriptEditor
{
    public class SinglePropertyProvider : IPropertyProviderViewModel
    {
        private readonly IObjectNode rootNode;

        public SinglePropertyProvider(IObjectNode rootNode)
        {
            this.rootNode = rootNode;
        }


        /// <inheritdoc/>
        public bool CanProvidePropertiesViewModel => true;

        /// <inheritdoc/>
        public IObjectNode GetRootNode() => rootNode;


        /// <inheritdoc/>
        bool IPropertyProviderViewModel.ShouldConstructMember(IMemberNode member) => true;

        bool IPropertyProviderViewModel.ShouldConstructItem(IObjectNode collection, NodeIndex index) => true;
    }
}
