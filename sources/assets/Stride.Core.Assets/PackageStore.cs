// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Stride.Core.IO;
using Stride.Core.Packages;

namespace Stride.Core.Assets
{
    /// <summary>
    ///   Manages packages locally installed and accessible on the store.
    /// </summary>
    /// <remarks>
    ///   This class is the frontend to the packaging / distribution system. It is currently using NuGet for its
    ///   packaging but may change in the future.
    /// </remarks>
    public class PackageStore
    {
        private static readonly Lazy<PackageStore> DefaultPackageStore = new(() => new PackageStore());

        /// <summary>
        ///   Associated <see cref="NugetStore"/> for our packages. Cannot be null.
        /// </summary>
        private readonly NugetStore store;


        /// <summary>
        ///   Initializes a new instance of the <see cref="PackageStore"/> class.
        /// </summary>
        /// <exception cref="InvalidOperationException">Unable to find a valid Stride installation path.</exception>
        private PackageStore()
        {
            // Check if we are in a root directory with store / packages facilities
            store = new NugetStore(oldRootDirectory: null);
        }


        /// <summary>
        ///   Gets the packages available online.
        /// </summary>
        /// <returns>The collection of packages available in the store online.</returns>
        public async Task<IEnumerable<PackageMeta>> GetPackages()
        {
            var packages = await store.SourceSearch(searchTerm: null, allowPrereleaseVersions: false);

            // Order by download count and Id to allow collapsing
            var orderedPackages = packages.OrderByDescending(p => p.DownloadCount).ThenBy(p => p.Id);

            // For some unknown reasons, we can't select directly from IQueryable<IPackage> to IQueryable<PackageMeta>,
            // so we need to pass through a IEnumerable<PackageMeta> and translate it to IQueyable. Not sure it has
            // an implication on the original query behinds the scene
            return orderedPackages.Select(PackageMetaFromNugetPackage);
        }

        /// <summary>
        ///   Finds an installed local package using a version range or some constraints.
        /// </summary>
        /// <param name="packageName">Name of the package.</param>
        /// <param name="versionRange">The range of versions of the package to look for.</param>
        /// <param name="constraintProvider">Version constraints for the packages.</param>
        /// <param name="allowPrereleaseVersion">Value indicating whether to allow pre-release versions of the packages.</param>
        /// <param name="allowUnlisted">Value indicating whether to look for packages not listed publically.</param>
        /// <returns>A package matching the search criteria; or <c>null</c> if not found.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="packageName"/> is a <c>null</c> reference.</exception>
        /// <remarks>
        ///   If no constraints are specified, the first found entry, whatever it means for NuGet, is used.
        /// </remarks>
        public NugetLocalPackage FindLocalPackage(string packageName,
                                                  PackageVersionRange versionRange = null,
                                                  ConstraintProvider constraintProvider = null,
                                                  bool allowPrereleaseVersion = true,
                                                  bool allowUnlisted = false)
        {
            if (string.IsNullOrWhiteSpace(packageName))
                throw new ArgumentNullException(nameof(packageName));

            return store.FindLocalPackage(packageName, versionRange, constraintProvider, allowPrereleaseVersion, allowUnlisted);
        }

        /// <summary>
        ///   Gets the file name of a specific package using its package name.
        /// </summary>
        /// <param name="packageName">Name of the package.</param>
        /// <returns>The location on disk of specified package; or <c>null</c> if not found.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="packageName"/> is a <c>null</c> reference.</exception>
        public UFile GetPackageWithFileName(string packageName) => GetPackageFileName(packageName);

        /// <summary>
        ///   Gets the file name of a specific package using its version or some constraints.
        /// </summary>
        /// <param name="packageName">Name of the package.</param>
        /// <param name="versionRange">The range of versions of the package to look for.</param>
        /// <param name="constraintProvider">Version constraints for the packages.</param>
        /// <param name="allowPrereleaseVersion">Value indicating whether to allow pre-release versions of the packages.</param>
        /// <param name="allowUnlisted">Value indicating whether to look for packages not listed publically.</param>
        /// <returns>The location on disk of specified package; or <c>null</c> if not found.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="packageName"/> is a <c>null</c> reference.</exception>
        /// <remarks>
        ///   If no constraints are specified, the first found entry (if any) is used to get the filename.
        /// </remarks>
        public UFile GetPackageFileName(string packageName,
                                        PackageVersionRange versionRange = null,
                                        ConstraintProvider constraintProvider = null,
                                        bool allowPrereleaseVersion = true,
                                        bool allowUnlisted = false)
        {
            if (string.IsNullOrWhiteSpace(packageName))
                throw new ArgumentNullException(nameof(packageName));

            var package = store.FindLocalPackage(packageName, versionRange, constraintProvider, allowPrereleaseVersion, allowUnlisted);
            if (package is not null)
            {
                var packageRoot = (UDirectory) store.GetRealPath(package);
                var packageFilename = new UFile(packageName + Package.PackageFileExtension);

                // First look for .sdpkg at package root
                var packageFile = UPath.Combine(packageRoot, packageFilename);
                if (File.Exists(packageFile))
                    return packageFile;

                // Then look for sdpkg inside 'stride' subfolder
                packageFile = UPath.Combine(UPath.Combine(packageRoot, (UDirectory) "stride"), packageFilename);
                if (File.Exists(packageFile))
                    return packageFile;
            }

            // Not found
            return null;
        }

        /// <summary>
        ///   Gets the default package manager.
        /// </summary>
        public static PackageStore Instance => DefaultPackageStore.Value;

        private static PackageLoadParameters GetDefaultPackageLoadParameters() => new()
        {
            // By default, we are not loading assets for installed packages
            AutoLoadTemporaryAssets = false,
            LoadAssemblyReferences = false,
            AutoCompileProjects = false
        };

        /// <summary>
        ///   Gets the metadata of a package.
        /// </summary>
        /// <param name="metadata">The NuGet metadata used to initialized an instance of <see cref="PackageMeta"/>.</param>
        /// <returns>A <see cref="PackageMeta"/> containing the package metadata.</returns>
        public static PackageMeta PackageMetaFromNugetPackage(NugetPackage metadata)
        {
            var meta = new PackageMeta
            {
                Name = metadata.Id,
                Version = new PackageVersion(metadata.Version.ToString()),
                Title = metadata.Title,
                IconUrl = metadata.IconUrl,
                LicenseUrl = metadata.LicenseUrl,
                ProjectUrl = metadata.ProjectUrl,
                RequireLicenseAcceptance = metadata.RequireLicenseAcceptance,
                Description = metadata.Description,
                Summary = metadata.Summary,
                Tags = metadata.Tags,
                Listed = metadata.Listed,
                Published = metadata.Published,
                ReportAbuseUrl = metadata.ReportAbuseUrl,
                DownloadCount = metadata.DownloadCount
            };

            meta.Authors.AddRange(metadata.Authors);
            meta.Owners.AddRange(metadata.Owners);

            if (metadata.DependencySetsCount > 1)
                throw new InvalidOperationException("Metadata loaded from .nuspec cannot have more than one group of dependencies.");

            return meta;
        }

        public static void ToNugetManifest(PackageMeta meta, ManifestMetadata manifestMeta)
        {
            manifestMeta.Id = meta.Name;
            manifestMeta.Version = meta.Version.ToString();
            manifestMeta.Title = meta.Title.SafeTrim();
            manifestMeta.Authors = meta.Authors;
            manifestMeta.Owners = meta.Owners;
            manifestMeta.Tags = String.IsNullOrEmpty(meta.Tags) ? null : meta.Tags.SafeTrim();
            manifestMeta.LicenseUrl = ConvertUrlToStringSafe(meta.LicenseUrl);
            manifestMeta.ProjectUrl = ConvertUrlToStringSafe(meta.ProjectUrl);
            manifestMeta.IconUrl = ConvertUrlToStringSafe(meta.IconUrl);
            manifestMeta.RequireLicenseAcceptance = meta.RequireLicenseAcceptance;
            manifestMeta.DevelopmentDependency = false;
            manifestMeta.Description = meta.Description.SafeTrim();
            manifestMeta.Copyright = meta.Copyright.SafeTrim();
            manifestMeta.Summary = meta.Summary.SafeTrim();
            manifestMeta.ReleaseNotes = meta.ReleaseNotes.SafeTrim();
            manifestMeta.Language = meta.Language.SafeTrim();
        }

        private static string ConvertUrlToStringSafe(Uri url)
        {
            if (url is not null)
            {
                string originalString = url.OriginalString.SafeTrim();
                if (!string.IsNullOrEmpty(originalString))
                    return originalString;
            }

            return null;
        }
    }
}
