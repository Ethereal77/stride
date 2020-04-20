// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using Stride.Core;
using Stride.Core.Assets;

using Xunit;

// We run test one by one (various things are not thread-safe)
[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace Stride.Samples.Tests
{
    class SampleTestsData
    {
        public const PlatformType TestPlatform = PlatformType.Windows;
    }
}
