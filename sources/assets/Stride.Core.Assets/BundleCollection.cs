// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

using Stride.Core;

namespace Stride.Core.Assets
{
    /// <summary>
    /// A collection of bundles.
    /// </summary>
    [DataContract("!Bundles")]
    public class BundleCollection : List<Bundle>
    {
        private readonly Package package;

        /// <summary>
        /// Initializes a new instance of the <see cref="BundleCollection"/> class.
        /// </summary>
        /// <param name="package">The package.</param>
        internal BundleCollection(Package package)
        {
            this.package = package;
        }

        /// <summary>
        /// Gets the package this bundle collection is defined for.
        /// </summary>
        /// <value>The package.</value>
        [DataMemberIgnore]
        private Package Package
        {
            get
            {
                return package;
            }
        }
    }
}
