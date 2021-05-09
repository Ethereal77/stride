// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core;

namespace Stride.Core.Assets
{
    public static class PackageVersionRangeExtensions
    {
        public static Func<Package, bool> ToFilter(this PackageVersionRange versionInfo)
        {
            if (versionInfo == null)
            {
                throw new ArgumentNullException("versionInfo");
            }
            return versionInfo.ToFilter<Package>(p => p.Meta.Version);
        }

    }
}
