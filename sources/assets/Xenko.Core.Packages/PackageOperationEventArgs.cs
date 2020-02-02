// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Core.Packages
{
    public class PackageOperationEventArgs
    {
        /// <summary>
        /// Initialize a new instance of <see cref="PackageOperationEventArgs"/> using the corresponding NuGet abstraction.
        /// </summary>
        internal PackageOperationEventArgs(PackageName name, string installPath)
        {
            Name = name;
            InstallPath = installPath;
        }

        /// <summary>
        /// Name of package being installed/uninstalled.
        /// </summary>
        public PackageName Name { get; }

        /// <summary>
        /// Id of <see cref="Name"/>.
        /// </summary>
        public string Id => Name.Id;

        /// <summary>
        /// Location where package is installed to/uninstalled from.
        /// </summary>
        public string InstallPath { get; }
    }
}
