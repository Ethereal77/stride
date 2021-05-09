// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Linq;

using Xunit;

using Stride.Games;

namespace Stride.Graphics.Regression
{
    public class TestGraphicsDeviceManager : GraphicsDeviceManager
    {
        public TestGraphicsDeviceManager(GameBase game)
            : base(game)
        {
        }

        protected override bool IsPreferredProfileAvailable(GraphicsProfile[] preferredProfiles, out GraphicsProfile availableProfile)
        {
            Assert.True(base.IsPreferredProfileAvailable(preferredProfiles, out availableProfile), $"This test requires the '{preferredProfiles.Min()}' graphic profile. It has been ignored");
            return true;
        }
    }
}
