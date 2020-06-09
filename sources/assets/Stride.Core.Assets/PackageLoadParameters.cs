// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading;

using Stride.Core.IO;
using Stride.Core.Reflection;
using Stride.Core.Diagnostics;

namespace Stride.Core.Assets
{
    /// <summary>
    ///   Parameters used for loading a <see cref="Package"/>.
    /// </summary>
    public sealed class PackageLoadParameters
    {
        private static readonly PackageLoadParameters DefaultParameters = new PackageLoadParameters();

        /// <summary>
        ///   Gets the default parameters.
        /// </summary>
        public static PackageLoadParameters Default() => DefaultParameters.Clone();

        /// <summary>
        ///   Indicates if the given value of <see cref="PackageUpgradeRequestedAnswer"/> should trigger an upgrade.
        /// </summary>
        /// <param name="answer">The value to evaluate.</param>
        /// <returns><c>true</c> if it should trigger an upgrade, <c>false</c> otherwise.</returns>
        public static bool ShouldUpgrade(PackageUpgradeRequestedAnswer answer) =>
            answer == PackageUpgradeRequestedAnswer.Upgrade ||
            answer == PackageUpgradeRequestedAnswer.UpgradeAll;

        /// <summary>
        ///   Initializes a new instance of the <see cref="PackageLoadParameters"/> class.
        /// </summary>
        public PackageLoadParameters()
        {
            LoadMissingDependencies = true;
            LoadAssemblyReferences = true;
            AutoCompileProjects = true;
            AutoLoadTemporaryAssets = true;
            ConvertUPathToAbsolute = true;
            BuildConfiguration = "Debug";
        }

        /// <summary>
        ///   Gets or sets a value indicating whether we should load the package's missing dependencies.
        /// </summary>
        /// <value><c>true</c> to load missing dependencies; otherwise, <c>false</c>.</value>
        public bool LoadMissingDependencies { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether we should load assembly references.
        /// </summary>
        /// <value><c>true</c> to load assembly references; otherwise, <c>false</c>.</value>
        public bool LoadAssemblyReferences { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether to automatically compile projects that don't have their assembly generated.
        /// </summary>
        /// <value><c>true</c> to auto-compile projects; otherwise, <c>false</c>.</value>
        public bool AutoCompileProjects { get; set; }

        /// <summary>
        ///   Gets or sets the build configuration to use to automatically compile projects, when <see cref="AutoCompileProjects"/> is <c>true</c>.
        /// </summary>
        /// <value>The build configuration.</value>
        public string BuildConfiguration { get; set; }

        /// <summary>
        ///   Gets or sets the extra compilation properties, used when <see cref="AutoCompileProjects"/> is <c>true</c>.
        /// </summary>
        /// <value>The extra compile parameters.</value>
        public Dictionary<string, string> ExtraCompileProperties { get; set; }

        /// <summary>
        ///   Gets or sets the list of assets to load, if you want to not rely on the default <see cref="Package.ListAssetFiles"/>.
        /// </summary>
        /// <value>The list of asset files to load.</value>
        public List<PackageLoadingAssetFile> AssetFiles { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether to automatically load assets. Default is <c>true</c>.
        /// </summary>
        /// <value><c>true</c> to auto-load assets; otherwise, <c>false</c>.</value>
        public bool AutoLoadTemporaryAssets { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether to convert all <see cref="UPath"/> to absolute paths when loading.
        ///   Default is <c>true</c>.
        /// </summary>
        /// <value><c>true</c> to convert paths to absolute paths; otherwise, <c>false</c>.</value>
        public bool ConvertUPathToAbsolute { get; set; }

        /// <summary>
        ///   Gets or sets the cancellation token used to cancel the loading of the package.
        /// </summary>
        /// <value>The cancellation token.</value>
        public CancellationToken? CancelToken { get; set; }

        /// <summary>
        ///   Gets or sets the assembly container used to load assemblies referenced by the package.
        ///   If <c>null</c>, will use <see cref="AssemblyContainer.Default"/>.
        /// </summary>
        /// <value>The assembly container.</value>
        public AssemblyContainer AssemblyContainer { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether to generate new asset ids.
        /// </summary>
        /// <value><c>true</c> to generate new asset ids; <c>false</c> otherwise.</value>
        /// <remarks>Only makes sense for <see cref="PackageSession.AddExistingProject(UFile, ILogger, PackageLoadParameters)"/>.</remarks>
        public bool GenerateNewAssetIds { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether to remove unloadable objects from the package.
        /// </summary>
        /// <value><c>true</c> to remove the objects that can't be loaded; <c>false</c> otherwise.</value>
        public bool RemoveUnloadableObjects { get; set; }

        /// <summary>
        ///   Function to call when one or more package upgrades are required for a single package.
        ///   Returning <c>false</c> will cancel upgrades on this package.
        /// </summary>
        public Func<Package, IList<PackageSession.PendingPackageUpgrade>, PackageUpgradeRequestedAnswer> PackageUpgradeRequested;

        /// <summary>
        ///   Function to call when an asset is about to be loaded.
        ///   Returning <c>false</c> will ignore the asset so it won't be loaded.
        /// </summary>
        public Func<PackageLoadingAssetFile, bool> TemporaryAssetFilter;

        /// <summary>
        ///   Gets or sets a value indicating whether MSBuild files should be evaluated when listing assets.
        /// </summary>
        /// <value><c>true</c> to include MSBuild files; <c>false</c> otherwise.</value>
        public bool TemporaryAssetsInMsBuild { get; set; } = true;

        /// <summary>
        ///   Clones this instance.
        /// </summary>
        /// <returns>An exact clone of this <see cref="PackageLoadParameters"/>.</returns>
        public PackageLoadParameters Clone() => (PackageLoadParameters) MemberwiseClone();
    }
}
