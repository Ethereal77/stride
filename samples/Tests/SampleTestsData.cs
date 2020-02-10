// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using Xenko.Core;
using Xenko.Core.Assets;

using Xunit;

// We run test one by one (various things are not thread-safe)
[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace Xenko.Samples.Tests
{
    class SampleTestsData
    {
        public const PlatformType TestPlatform = PlatformType.Windows;
    }
}
