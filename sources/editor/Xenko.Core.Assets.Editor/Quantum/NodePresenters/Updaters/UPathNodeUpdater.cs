// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Assets.Editor.Components.Properties;
using Xenko.Core.Assets.Editor.Quantum.NodePresenters.Keys;
using Xenko.Core.IO;
using Xenko.Core.Presentation.Quantum.Presenters;

namespace Xenko.Core.Assets.Editor.Quantum.NodePresenters.Updaters
{
    public sealed class UPathNodeUpdater : NodePresenterUpdaterBase
    {
        public override void UpdateNode(INodePresenter node)
        {
            if (typeof(UPath).IsAssignableFrom(node.Type))
            {
                node.AttachedProperties.Add(ReferenceData.Key, new UPathReferenceViewModel());
            }
        }
    }
}
