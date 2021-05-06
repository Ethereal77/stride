// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

using Stride.Core.Annotations;

using Constants = NuGet.ProjectManagement.Constants;

namespace Stride.Core.Packages
{
    /// <summary>
    ///   Represents the NuGet abstraction of a package.
    /// </summary>
    public abstract class NugetPackage : IEquatable<NugetPackage>
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref="NugetPackage"/> class.
        /// </summary>
        /// <param name="package">The NuGet metadata.</param>
        internal NugetPackage([NotNull] IPackageSearchMetadata package)
        {
            packageMetadata = package ?? throw new ArgumentNullException(nameof(package));
        }


        /// <summary>
        ///   Storage for the NuGet metadata.
        /// </summary>
        private readonly IPackageSearchMetadata packageMetadata;


        /// <inheritdoc />
        public bool Equals(NugetPackage other)
        {
            return packageMetadata.Identity.Equals(other.packageMetadata.Identity);
        }

        /// <inheritdoc />
        public override bool Equals(object other)
        {
            if (other is null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return other is NugetPackage nugetPackage && Equals(nugetPackage);
        }

        /// <inheritdoc />
        public override int GetHashCode() => packageMetadata.GetHashCode();

        /// <summary>
        ///   Determines whether two specified <see cref="NugetPackage"/> objects are equal.
        /// </summary>
        /// <param name="left">The first <see cref="NugetPackage"/>object.</param>
        /// <param name="right">The second <see cref="NugetPackage"/>object.</param>
        /// <returns><c>true</c> if <paramref name="left"/> is equal to <paramref name="right"/>, <c>false</c> otherwise.</returns>
        public static bool operator ==(NugetPackage left, NugetPackage right) => Equals(left, right);

        /// <summary>
        ///   Determines whether two specified <see cref="NugetPackage"/> objects are not equal.
        /// </summary>
        /// <param name="left">The first <see cref="NugetPackage"/>object.</param>
        /// <param name="right">The second <see cref="NugetPackage"/>object.</param>
        /// <returns><c>true</c> if <paramref name="left"/> is not equal to <paramref name="right"/>, <c>false</c> otherwise.</returns>
        public static bool operator !=(NugetPackage left, NugetPackage right) => !Equals(left, right);


        /// <summary>
        ///   Gets the version of current package.
        /// </summary>
        public PackageVersion Version => packageMetadata.Identity.Version.ToPackageVersion();

        /// <summary>
        ///   Gets the <see cref="NuGet.Versioning.NuGetVersion"/> of this package's version.
        /// </summary>
        /// <remarks>Internal since it exposes a NuGet type.</remarks>
        internal NuGetVersion NuGetVersion => packageMetadata.Identity.Version;

        /// <summary>
        ///   Gets the <see cref="PackageIdentity"/> of the package.
        /// </summary>
        /// <remarks>Internal since it exposes a NuGet type.</remarks>
        internal PackageIdentity Identity => packageMetadata.Identity;

        /// <summary>
        ///   Gets the package name (Id) of the package.
        /// </summary>
        public string Id => packageMetadata.Identity.Id;

        /// <summary>
        ///   Gets a value indicating whether the package is published and public (listed).
        /// </summary>
        public bool Listed => !Published.HasValue || Published > Constants.Unpublished;

        /// <summary>
        ///   Gets the date of publication of the package.
        /// </summary>
        /// <value>The date of publication, if present.</value>
        public DateTimeOffset? Published => packageMetadata.Published;

        /// <summary>
        ///   Gets the title of the package.
        /// </summary>
        public string Title => packageMetadata.Title;

        /// <summary>
        ///   Gets the authors of the package.
        /// </summary>
        public IEnumerable<string> Authors => new List<string>(1) { packageMetadata.Authors };

        /// <summary>
        ///   Gets the owners of the package.
        /// </summary>
        public IEnumerable<string> Owners => new List<string>(1) { packageMetadata.Owners };

        /// <summary>
        ///   Gets the URL of the package's icon.
        /// </summary>
        public Uri IconUrl => packageMetadata.IconUrl;

        /// <summary>
        ///   Gets the URL of the package's license.
        /// </summary>
        public Uri LicenseUrl => packageMetadata.LicenseUrl;

        /// <summary>
        ///   Gets the URL of the package's project.
        /// </summary>
        public Uri ProjectUrl => packageMetadata.ProjectUrl;

        /// <summary>
        ///   Gets a value indicating whether the package requires accepting a license.
        /// </summary>
        public bool RequireLicenseAcceptance => packageMetadata.RequireLicenseAcceptance;

        /// <summary>
        ///   Gets the description of the package.
        /// </summary>
        public string Description => packageMetadata.Description;

        /// <summary>
        ///   Gets the summary description of the package.
        /// </summary>
        public string Summary => packageMetadata.Summary;

        /// <summary>
        ///   Gets the tags of the package.
        /// </summary>
        /// <value>The tags of the package separated by spaces.</value>
        public string Tags => packageMetadata.Tags;

        /// <summary>
        ///   Gets the list of dependencies of the package.
        /// </summary>
        /// <remarks>Internal since it exposes a NuGet type.</remarks>
        internal IEnumerable<NuGet.Packaging.PackageDependencyGroup> DependencySets => packageMetadata.DependencySets;

        /// <summary>
        ///   Gets the number of downloads for the package, specific to the version of the package.
        /// </summary>
        public long DownloadCount => VersionInfo.DownloadCount ?? 0;

        /// <summary>
        ///   Gets the URL to report abuse for the package.
        /// </summary>
        public Uri ReportAbuseUrl => packageMetadata.ReportAbuseUrl;

        /// <summary>
        ///   Gets the number of dependency sets.
        /// </summary>
        public int DependencySetsCount => DependencySets?.Count() ?? 0;

        /// <summary>
        ///   Gets a computed list of dependencies for the package.
        /// </summary>
        public IEnumerable<(string id, PackageVersionRange version)>  Dependencies
        {
            get
            {
                var dependencies = new List<(string, PackageVersionRange)>();

                var set = DependencySets.FirstOrDefault();
                if (set != null)
                {
                    foreach (var dependency in set.Packages)
                    {
                        dependencies.Add((dependency.Id, dependency.VersionRange.ToPackageVersionRange()));
                    }
                }

                return dependencies;
            }
        }

        /// <summary>
        ///   Gets the <see cref="VersionInfo"/> associated with the package.
        /// </summary>
        private VersionInfo VersionInfo
        {
            get
            {
                if (versionInfo is null)
                {
                    // Get all versions of the current package and filter on the current package's version.
                    versionInfo = packageMetadata.GetVersionsAsync().Result.First(v =>  v.Version.Equals(Version.ToNuGetVersion()));
                }
                return versionInfo;
            }
        }

        private VersionInfo versionInfo;
    }
}
