// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;

using Stride.Core;
using Stride.Engine;
using Stride.Games;

namespace Stride.Navigation.Processors
{
    internal class BoundingBoxProcessor : EntityProcessor<NavigationBoundingBoxComponent>
    {
        public ICollection<NavigationBoundingBoxComponent> BoundingBoxes => ComponentDatas.Keys;

        protected override void OnSystemAdd()
        {
            // TODO Plugins
            // This is the same kind of entry point as used in PhysicsProcessor
            var gameSystems = Services.GetSafeServiceAs<IGameSystemCollection>();
            var navigationSystem = gameSystems.OfType<DynamicNavigationMeshSystem>().FirstOrDefault();
            if (navigationSystem == null)
            {
                navigationSystem = new DynamicNavigationMeshSystem(Services);
                gameSystems.Add(navigationSystem);
            }
        }
    }
}
