// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Xenko.Core.Assets.Editor.Quantum;
using Xenko.Core.Assets.Editor.Services;
using Xenko.Core.Assets.Quantum;
using Xenko.Core.Extensions;

namespace Xenko.Core.Assets.Editor.ViewModel
{
    public class SessionNodeContainer : AssetNodeContainer
    {
        public SessionNodeContainer(SessionViewModel session)
        {
            if (session == null) throw new ArgumentNullException(nameof(session));

            // Apply primitive types, commands and associated data providers that comes from plugins
            var pluginService = session.ServiceProvider.Get<IAssetsPluginService>();
            pluginService.GetPrimitiveTypes(session).ForEach(x => NodeBuilder.RegisterPrimitiveType(x));
        }
    }
}
