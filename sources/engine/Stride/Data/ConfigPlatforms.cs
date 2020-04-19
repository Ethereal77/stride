// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Xenko.Core;

namespace Xenko.Data
{
    [Flags]
    public enum ConfigPlatforms
    {
        None = 0,
        Windows = 1 << PlatformType.Windows
    }
}
