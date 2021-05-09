// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Core.Assets
{
    /// <summary>
    /// Defines a context for overrides when upgrading an asset.
    /// </summary>
    public enum OverrideUpgraderHint
    {
        /// <summary>
        /// The upgrader is performed on an asset that may be used as the base for another asset
        /// </summary>
        Unknown,

        /// <summary>
        /// The upgrader is performed on an asset that has at least one base asset (for asset templating)
        /// </summary>
        Derived,

        /// <summary>
        /// The upgrader is performed on the base asset of an asset being upgraded (<see cref="Asset.Base"/> or <see cref="Asset.BaseParts"/>)
        /// </summary>
        Base
    }
}
