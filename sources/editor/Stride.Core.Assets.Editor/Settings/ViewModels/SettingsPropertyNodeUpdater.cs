// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;

using Stride.Core.Assets.Editor.Components.Properties;
using Stride.Core.Presentation.Quantum.Presenters;

namespace Stride.Core.Assets.Editor.Settings.ViewModels
{
    internal class SettingsPropertyNodeUpdater : NodePresenterUpdaterBase
    {
        public override void UpdateNode(INodePresenter node)
        {
            var settingsKey = node.Value as PackageSettingsWrapper.SettingsKeyWrapper;
            if (settingsKey != null)
            {
                var acceptableValues = settingsKey.Key.AcceptableValues.ToList();
                node.AttachedProperties.Add(SettingsData.HasAcceptableValuesKey, acceptableValues.Count > 0);
                node.AttachedProperties.Add(SettingsData.AcceptableValuesKey, acceptableValues);
            }
        }
    }
}
