// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Core.Assets
{
    /// <summary>
    /// An enum representing the user answer to a package upgrade request.
    /// </summary>
    public enum PackageUpgradeRequestedAnswer
    {
        /// <summary>
        /// The related package should be upgraded.
        /// </summary>
        Upgrade,
        /// <summary>
        /// The related package and all following packages should be upgraded.
        /// </summary>
        UpgradeAll,
        /// <summary>
        /// The related package should not be upgraded.
        /// </summary>
        DoNotUpgrade,
        /// <summary>
        /// The related package and all following packages should not be upgraded.
        /// </summary>
        DoNotUpgradeAny,
    }
}
