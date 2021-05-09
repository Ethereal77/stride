// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.ObjectModel;

using Stride.Core;

namespace Stride.Core.Assets
{
    /// <summary>
    /// A collection of <see cref="SolutionPlatform"/>.
    /// </summary>
    public sealed class SolutionPlatformCollection : KeyedCollection<PlatformType, SolutionPlatform>
    {
        protected override PlatformType GetKeyForItem(SolutionPlatform item)
        {
            return item.Type;
        }
    }
}
