// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using NuGet.Commands;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.PackageManagement;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.ProjectModel;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Resolver;
using NuGet.Versioning;

using Stride.Core.Extensions;
using Stride.Core.Windows;

using ISettings = NuGet.Configuration.ISettings;
using PackageSource = NuGet.Configuration.PackageSource;
using PackageSourceProvider = NuGet.Configuration.PackageSourceProvider;

namespace Stride.Core.Packages
{
    /// <summary>
    ///   Represents an abstraction of a store backed by the NuGet infrastructure.
    /// </summary>
    public class NugetStore : INugetDownloadProgress
    {
        public const string DefaultPackageSource = "https://packages.stride3d.net/nuget";

        private IPackagesLogger logger;
        private readonly ISettings settings;
        private ProgressReport currentProgressReport;

        private readonly string oldRootDirectory;

        private static Regex powerShellProgressRegex = new Regex(@".*\[ProgressReport:\s*(\d*)%\].*");

        /// <summary>
        ///   Initialize a new instance of the <see cref="NugetStore"/> class.
        /// </summary>
        /// <param name="oldRootDirectory">The location of the Nuget store.</param>
        public NugetStore(string oldRootDirectory)
        {
            // Used only for versions before 3.0
            this.oldRootDirectory = oldRootDirectory;

            settings = NuGet.Configuration.Settings.LoadDefaultSettings(null);

            // Remove obsolete sources
            RemoveDeletedSources(settings, "Xenko Dev");
            // Note the space: we want to keep "Stride Dev" but not "Stride Dev {PATH}\bin\packages" anymore
            RemoveSources(settings, "Stride Dev ");
            // Add Stride package store (still used for Xenko up to 3.0)
            CheckPackageSource("Stride", DefaultPackageSource);
            settings.SaveToDisk();

            InstallPath = SettingsUtility.GetGlobalPackagesFolder(settings);

            var pathContext = NuGetPathContext.Create(settings);
            InstalledPathResolver = new FallbackPackagePathResolver(pathContext.UserPackageFolder, oldRootDirectory != null ? pathContext.FallbackPackageFolders.Concat(new[] { oldRootDirectory }) : pathContext.FallbackPackageFolders);
            var packageSourceProvider = new PackageSourceProvider(settings);

            var availableSources = packageSourceProvider.LoadPackageSources().Where(source => source.IsEnabled);
            var packageSources = new List<PackageSource>();
            packageSources.AddRange(availableSources);
            PackageSources = packageSources;

            // Setup source provider as a V3 only
            sourceRepositoryProvider = new NugetSourceRepositoryProvider(packageSourceProvider, this);
        }

        private static void RemoveSources(ISettings settings, string prefixName)
        {
            var packageSources = settings.GetSection("packageSources");
            if (packageSources != null)
            {
                foreach (var packageSource in packageSources.Items.OfType<SourceItem>().ToList())
                {
                    var path = packageSource.GetValueAsPath();

                    if (packageSource.Key.StartsWith(prefixName))
                    {
                        // Remove entry from packageSources
                        settings.Remove("packageSources", packageSource);
                    }
                }
            }
        }

        private static void RemoveDeletedSources(ISettings settings, string prefixName)
        {
            var packageSources = settings.GetSection("packageSources");
            if (packageSources != null)
            {
                foreach (var packageSource in packageSources.Items.OfType<SourceItem>().ToList())
                {
                    var path = packageSource.GetValueAsPath();

                    if (packageSource.Key.StartsWith(prefixName) &&
                        Uri.TryCreate(path, UriKind.Absolute, out var uri) && uri.IsFile && // Make sure it's a valid file URI
                        !Directory.Exists(path))                                            // Detect if directory has been deleted
                    {
                        // Remove entry from packageSources
                        settings.Remove("packageSources", packageSource);
                    }
                }
            }
        }

        private void CheckPackageSource(string name, string url)
        {
            settings.AddOrUpdate("packageSources", new SourceItem(name, url));
        }

        private readonly NugetSourceRepositoryProvider sourceRepositoryProvider;

        /// <summary>
        ///   Gets the path where all packages are installed.
        ///   Usually it is <c>RootDirectory/RepositoryPath</c>.
        /// </summary>
        public string InstallPath { get; }

        /// <summary>
        ///   Gets a list of Package Ids under which the main package is known. Usually just one entry, but
        ///   we could have several in case there is a product name change.
        /// </summary>
        public IReadOnlyCollection<string> MainPackageIds { get; } = new[]
            {
                "Stride.GameStudio",
                "Xenko.GameStudio",
                "Xenko"
            };

        /// <summary>
        ///   Gets the Package Id of the Visual Studio Integration plugin.
        /// </summary>
        public string VsixPluginId { get; } = "Stride.VisualStudio.Package";

        /// <summary>
        ///   Gets or sets the logger for all operations of the package manager.
        /// </summary>
        public IPackagesLogger Logger
        {
            get => logger ?? NullPackagesLogger.Instance;
            set => logger = value;
        }

        private ILogger NativeLogger => new NugetLogger(Logger);

        private IEnumerable<PackageSource> PackageSources { get; }

        /// <summary>
        ///   Helper to locate packages.
        /// </summary>
        private FallbackPackagePathResolver InstalledPathResolver { get; }

        /// <summary>
        ///   Event executed when a package's installation has completed.
        /// </summary>
        public event EventHandler<PackageOperationEventArgs> NugetPackageInstalled;

        /// <summary>
        ///   Event executed when a package's uninstallation has completed.
        /// </summary>
        public event EventHandler<PackageOperationEventArgs> NugetPackageUninstalled;

        /// <summary>
        ///   Event executed when a package's uninstallation is in progress.
        /// </summary>
        public event EventHandler<PackageOperationEventArgs> NugetPackageUninstalling;

        /// <summary>
        ///   Gets the installation path of a package.
        /// </summary>
        /// <param name="id">Id of package to query.</param>
        /// <param name="version">Version of package to query.</param>
        /// <returns>The installation path if installed, <c>null</c> otherwise.</returns>
        public string GetInstalledPath(string id, PackageVersion version)
        {
            return InstalledPathResolver.GetPackageDirectory(id, version.ToNuGetVersion());
        }

        /// <summary>
        ///   Get the most recent version associated to a package.
        /// </summary>
        /// <param name="packageIds">
        ///   List of Package Ids representing a package name. It is assumed that all the Package Ids represent the same
        ///   package under a different name.
        /// </param>
        /// <returns>The most recent version of the package.</returns>
        public NugetLocalPackage GetLatestPackageInstalled(IEnumerable<string> packageIds)
        {
            return GetPackagesInstalled(packageIds).FirstOrDefault();
        }

        /// <summary>
        ///   Gets a list of all the packages represented by a Package Id. The list is ordered
        ///   from the most recent version to the oldest.
        /// </summary>
        /// <param name="packageIds">List of Ids representing the package names to retrieve.</param>
        /// <returns>The list of packages sorted from the most recent to the oldest.</returns>
        public IList<NugetLocalPackage> GetPackagesInstalled(IEnumerable<string> packageIds)
        {
            return packageIds.SelectMany(GetLocalPackages).OrderByDescending(p => p.Version).ToList();
        }

        /// <summary>
        ///   Gets a list of all installed packages.
        /// </summary>
        /// <returns>A list of packages.</returns>
        public IEnumerable<NugetLocalPackage> GetLocalPackages(string packageId)
        {
            var res = new List<NugetLocalPackage>();

            // We also scan rootDirectory for 1.x/2.x
            foreach (var installPath in new[] { InstallPath, oldRootDirectory })
            {
                // oldRootDirectory might be null
                if (installPath is null)
                    continue;

                var localResource = new FindLocalPackagesResourceV3(installPath);
                var packages = localResource.FindPackagesById(packageId, NativeLogger, CancellationToken.None);
                foreach (var package in packages)
                {
                    res.Add(new NugetLocalPackage(package));
                }
            }

            return res;
        }

        /// <summary>
        ///   Gets the name of the variable used to hold the version of a package.
        /// </summary>
        /// <param name="packageId">The package Id.</param>
        /// <returns>The name of the variable holding the version.</returns>
        public static string GetPackageVersionVariable(string packageId, string packageVariablePrefix = "StridePackage")
        {
            if (packageId is null)
                throw new ArgumentNullException(nameof(packageId));

            var newPackageId = packageId.Replace(".", string.Empty);
            return packageVariablePrefix + newPackageId + "Version";
        }

        /// <summary>
        ///   Gets the lock to ensure atomicity of updates to the local repository.
        /// </summary>
        /// <returns>A lock.</returns>
        private IDisposable GetLocalRepositoryLock()
        {
            return FileLock.Wait("nuget.lock");
        }

        #region Manager

        /// <summary>
        ///   Fetch, if not already downloaded, and installs the package represented by
        ///   (<paramref name="packageId"/>, <paramref name="version"/>).
        /// </summary>
        /// <param name="packageId">Name of package to install.</param>
        /// <param name="version">Version of package to install.</param>
        /// <remarks>
        ///   It is safe to call it concurrently because the operations are done by acquiring a lock.
        /// </remarks>
        public async Task<NugetLocalPackage> InstallPackage(string packageId, PackageVersion version, IEnumerable<string> targetFrameworks, ProgressReport progress)
        {
            using (GetLocalRepositoryLock())
            {
                currentProgressReport = progress;
                try
                {
                    var identity = new PackageIdentity(packageId, version.ToNuGetVersion());

                    var resolutionContext = new ResolutionContext(
                        DependencyBehavior.Lowest,
                        true,
                        true,
                        VersionConstraints.None);

                    var repositories = PackageSources.Select(sourceRepositoryProvider.CreateRepository).ToArray();

                    var projectContext = new EmptyNuGetProjectContext()
                    {
                        ActionType = NuGetActionType.Install,
                        PackageExtractionContext = new PackageExtractionContext(PackageSaveMode.Defaultv3, XmlDocFileSaveMode.Skip, null, NativeLogger)
                    };

                    ActivityCorrelationId.StartNew();

                    {
                        var installPath = SettingsUtility.GetGlobalPackagesFolder(settings);

                        // Old version expects to be installed in GamePackages
                        if (packageId == "Xenko" &&
                            version < new PackageVersion(3, 0, 0, 0) &&
                            oldRootDirectory != null)
                        {
                            installPath = oldRootDirectory;
                        }

                        var projectPath = Path.Combine("StrideLauncher.json");
                        var spec = new PackageSpec()
                        {
                            // Make sure this package never collides with a dependency
                            Name = Path.GetFileNameWithoutExtension(projectPath),
                            FilePath = projectPath,
                            Dependencies = new List<LibraryDependency>()
                            {
                                new LibraryDependency
                                {
                                    LibraryRange = new LibraryRange(packageId, new VersionRange(version.ToNuGetVersion()), LibraryDependencyTarget.Package)
                                }
                            },
                            RestoreMetadata = new ProjectRestoreMetadata
                            {
                                ProjectPath = projectPath,
                                ProjectName = Path.GetFileNameWithoutExtension(projectPath),
                                ProjectStyle = ProjectStyle.PackageReference,
                                ProjectUniqueName = projectPath,
                                OutputPath = Path.Combine(Path.GetTempPath(), $"StrideLauncher-{packageId}-{version.ToString()}"),
                                OriginalTargetFrameworks = targetFrameworks.ToList(),
                                ConfigFilePaths = settings.GetConfigFilePaths(),
                                PackagesPath = installPath,
                                Sources = SettingsUtility.GetEnabledSources(settings).ToList(),
                                FallbackFolders = SettingsUtility.GetFallbackPackageFolders(settings).ToList()
                            }
                        };
                        foreach (var targetFramework in targetFrameworks)
                        {
                            spec.TargetFrameworks.Add(new TargetFrameworkInformation { FrameworkName = NuGetFramework.Parse(targetFramework) });
                        }

                        using (var context = new SourceCacheContext { MaxAge = DateTimeOffset.UtcNow })
                        {
                            context.IgnoreFailedSources = true;

                            var dependencyGraphSpec = new DependencyGraphSpec();

                            dependencyGraphSpec.AddProject(spec);

                            dependencyGraphSpec.AddRestore(spec.RestoreMetadata.ProjectUniqueName);

                            IPreLoadedRestoreRequestProvider requestProvider = new DependencyGraphSpecRequestProvider(new RestoreCommandProvidersCache(), dependencyGraphSpec);

                            var restoreArgs = new RestoreArgs
                            {
                                AllowNoOp = true,
                                CacheContext = context,
                                CachingSourceProvider = new CachingSourceProvider(new PackageSourceProvider(settings)),
                                Log = NativeLogger
                            };

                            // Create requests from the arguments
                            var requests = requestProvider.CreateRequests(restoreArgs).Result;

                            foreach (var request in requests)
                            {
                                // Limit concurrency to avoid timeout
                                request.Request.MaxDegreeOfConcurrency = 4;

                                var command = new RestoreCommand(request.Request);

                                // Act
                                var result = await command.ExecuteAsync();

                                if (!result.Success)
                                    throw new InvalidOperationException($"Could not restore package {packageId}.");

                                foreach (var install in result.RestoreGraphs.Last().Install)
                                {
                                    var package = result.LockFile.Libraries.FirstOrDefault(x => x.Name == install.Library.Name && x.Version == install.Library.Version);
                                    if (package != null)
                                    {
                                        var packagePath = Path.Combine(installPath, package.Path);
                                        OnPackageInstalled(this, new PackageOperationEventArgs(new PackageName(install.Library.Name, install.Library.Version.ToPackageVersion()), packagePath));
                                    }
                                }
                            }
                        }

                        if (packageId == "Xenko" &&
                            version < new PackageVersion(3, 0, 0, 0))
                        {
                            UpdateTargetsHelper();
                        }
                    }

                    // Load the recently installed package
                    var installedPackages = GetPackagesInstalled(new[] { packageId });
                    return installedPackages.FirstOrDefault(p => p.Version == version);
                }
                finally
                {
                    currentProgressReport = null;
                }
            }
        }

        /// <summary>
        ///   Uninstalls a package, while still keeping the downloaded file in the cache.
        /// </summary>
        /// <param name="package">Package to uninstall.</param>
        /// <remarks>
        ///   It is safe to call it concurrently because the operations are done by acquiring a lock.
        /// </remarks>
        public async Task UninstallPackage(NugetPackage package, ProgressReport progress)
        {
#if DEBUG
            var installedPackages = GetPackagesInstalled(new [] {package.Id});
            Debug.Assert(installedPackages.FirstOrDefault(p => p.Equals(package)) != null);
#endif
            using (GetLocalRepositoryLock())
            {
                currentProgressReport = progress;
                try
                {
                    var identity = new PackageIdentity(package.Id, package.Version.ToNuGetVersion());

                    // Notify that uninstallation started
                    var installPath = GetInstalledPath(identity.Id, identity.Version.ToPackageVersion());
                    if (installPath is null)
                        throw new InvalidOperationException($"Could not find installation path for package {identity}.");

                    OnPackageUninstalling(this, new PackageOperationEventArgs(new PackageName(package.Id, package.Version), installPath));

                    var projectContext = new EmptyNuGetProjectContext()
                    {
                        ActionType = NuGetActionType.Uninstall,
                        PackageExtractionContext = new PackageExtractionContext(PackageSaveMode.Defaultv3, XmlDocFileSaveMode.Skip, null, NativeLogger)
                    };

                    // Simply delete the installed package and its .nupkg installed in it
                    await Task.Run(() => FileSystemUtility.DeleteDirectorySafe(installPath, recursive: true, projectContext));

                    // Notify that uninstallation completed
                    OnPackageUninstalled(this, new PackageOperationEventArgs(new PackageName(package.Id, package.Version), installPath));
                    //currentProgressReport = progress;
                    //try
                    //{
                    //    manager.UninstallPackage(package.IPackage);
                    //}
                    //finally
                    //{
                    //    currentProgressReport = null;
                    //}

                    if (package.Id == "Xenko" &&
                        package.Version < new PackageVersion(3, 0, 0, 0))
                    {
                        UpdateTargetsHelper();
                    }
                }
                finally
                {
                    currentProgressReport = null;
                }
            }
        }

        /// <summary>
        ///   Finds an installed package matching a version or some constraints.
        /// </summary>
        /// <param name="packageId">Name of the package.</param>
        /// <param name="version">The version range.</param>
        /// <param name="constraintProvider">The package constraint provider.</param>
        /// <param name="allowPrereleaseVersions">A value indicating whether to allow prerelease versions.</param>
        /// <param name="allowUnlisted">A value indicating whether to allow unlisted packages.</param>
        /// <returns>A package matching the search criteria; or <c>null</c> if not found.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="packageId"/> is a <c>null</c> reference.</exception>
        /// <remarks>
        ///   If no constraints are specified, the first found entry, whatever it means for NuGet, is used.
        /// </remarks>
        public NugetPackage FindLocalPackage(string packageId, PackageVersion version = null, ConstraintProvider constraintProvider = null, bool allowPrereleaseVersions = true, bool allowUnlisted = false)
        {
            var versionRange = new PackageVersionRange(version);

            return FindLocalPackage(packageId, versionRange, constraintProvider, allowPrereleaseVersions, allowUnlisted);
        }

        /// <summary>
        ///   Finds an installed local package using a version range or some constraints.
        /// </summary>
        /// <param name="packageId">The Id of the package.</param>
        /// <param name="versionRange">The range of versions of the package to look for.</param>
        /// <param name="constraintProvider">Version constraints for the packages.</param>
        /// <param name="allowPrereleaseVersions">Value indicating whether to allow pre-release versions of the packages.</param>
        /// <param name="allowUnlisted">Value indicating whether to look for packages not listed publically.</param>
        /// <returns>A package matching the search criteria; or <c>null</c> if not found.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="packageId"/> is a <c>null</c> reference.</exception>
        /// <remarks>
        ///   If no constraints are specified, the first found entry, whatever it means for NuGet, is used.
        /// </remarks>
        public NugetLocalPackage FindLocalPackage(string packageId, PackageVersionRange versionRange = null, ConstraintProvider constraintProvider = null, bool allowPrereleaseVersions = true, bool allowUnlisted = false)
        {
            // If an explicit version is specified, disregard the 'allowUnlisted' argument and always allow unlisted packages.
            if (versionRange is not null)
            {
                allowUnlisted = true;
            }
            else if (!allowUnlisted && ((constraintProvider is null) || !constraintProvider.HasConstraints))
            {
                // Simple case: We just get the most recent version based on `allowPrereleaseVersions`
                return GetPackagesInstalled(new[] { packageId })
                    .FirstOrDefault(p => allowPrereleaseVersions || string.IsNullOrEmpty(p.Version.SpecialVersion));
            }

            var packages = GetLocalPackages(packageId);

            if (!allowUnlisted)
                packages = packages.Where(p => p.Listed);

            if (constraintProvider is not null)
                versionRange = constraintProvider.GetConstraint(packageId) ?? versionRange;

            if (versionRange is not null)
                packages = packages.Where(p => versionRange.Contains(p.Version));

            return packages?.FirstOrDefault(p => allowPrereleaseVersions || string.IsNullOrEmpty(p.Version.SpecialVersion));
        }

        /// <summary>
        ///   Finds available packages from source with matching <paramref name="packageIds"/>.
        /// </summary>
        /// <param name="packageIds">List of package Ids we are looking for.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>
        ///   A list of packages matching <paramref name="packageIds"/>; or an empty list if none is found.
        /// </returns>
        public async Task<IEnumerable<NugetServerPackage>> FindSourcePackages(IReadOnlyCollection<string> packageIds, CancellationToken cancellationToken)
        {
            var repositories = PackageSources.Select(sourceRepositoryProvider.CreateRepository).ToArray();

            var res = new List<NugetServerPackage>();
            foreach (var packageId in packageIds)
            {
                await FindSourcePackagesByIdHelper(packageId, res, repositories, cancellationToken);
            }
            return res;
        }

        /// <summary>
        ///   Finds available packages from source with matching <paramref name="packageId"/>.
        /// </summary>
        /// <param name="packageId">Id of package we are looking for.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>
        ///   A list of packages matching <paramref name="packageIds"/>; or an empty list if none is found.
        /// </returns>
        public async Task<IEnumerable<NugetServerPackage>> FindSourcePackagesById(string packageId, CancellationToken cancellationToken)
        {
            var repositories = PackageSources.Select(sourceRepositoryProvider.CreateRepository).ToArray();

            var res = new List<NugetServerPackage>();
            await FindSourcePackagesByIdHelper(packageId, res, repositories, cancellationToken);
            return res;
        }

        private async Task FindSourcePackagesByIdHelper(string packageId, List<NugetServerPackage> resultList, SourceRepository [] repositories, CancellationToken cancellationToken)
        {
            using (var sourceCacheContext = new SourceCacheContext { MaxAge = DateTimeOffset.UtcNow })
            {
                foreach (var repo in repositories)
                {
                    try
                    {
                        var metadataResource = await repo.GetResourceAsync<PackageMetadataResource>(CancellationToken.None);
                        var metadataList = await metadataResource.GetMetadataAsync(packageId, true, true, sourceCacheContext, NativeLogger, cancellationToken);
                        foreach (var metadata in metadataList)
                        {
                            if (metadata.IsListed)
                                resultList.Add(new NugetServerPackage(metadata, repo.PackageSource.Source));
                        }
                    }
                    catch (FatalProtocolException)
                    {
                        // Ignore 404/403 etc... (invalid sources)
                    }
                }
            }
        }

        /// <summary>
        ///   Looks for available packages from source containing a <paramref name="searchTerm"/> in either the Id or description of the package.
        /// </summary>
        /// <param name="searchTerm">Term used for search.</param>
        /// <param name="allowPrereleaseVersions">A value indicating whether to allow prerelease versions.</param>
        /// <returns>A list of packages matching <paramref name="searchTerm"/>.</returns>
        public async Task<IQueryable<NugetPackage>> SourceSearch(string searchTerm, bool allowPrereleaseVersions)
        {
            var repositories = PackageSources.Select(sourceRepositoryProvider.CreateRepository).ToArray();

            var res = new List<NugetPackage>();
            foreach (var repo in repositories)
            {
                try
                {
                    var searchResource = await repo.GetResourceAsync<PackageSearchResource>(CancellationToken.None);

                    if (searchResource != null)
                    {
                        var searchResults = await searchResource.SearchAsync(searchTerm, new SearchFilter(includePrerelease: false), 0, 0, NativeLogger, CancellationToken.None);

                        if (searchResults != null)
                        {
                            var packages = searchResults.ToArray();

                            foreach (var package in packages)
                            {
                                if (package.IsListed)
                                    res.Add(new NugetServerPackage(package, repo.PackageSource.Source));
                            }
                        }
                    }
                }
                catch (FatalProtocolException)
                {
                    // Ignore 404/403 etc... (invalid sources)
                }
            }
            return res.AsQueryable();
        }

        /// <summary>
        ///   Returns updates for packages from the repository.
        /// </summary>
        /// <param name="packageName">Package to look for updates</param>
        /// <param name="includePrerelease">A value indicating whether to consider prerelease updates.</param>
        /// <param name="includeAllVersions">A value indicating whether to include all versions of an update as opposed to only including the latest version.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>List of package updates.</returns>
        public async Task<IEnumerable<NugetPackage>> GetUpdates(PackageName packageName, bool includePrerelease, bool includeAllVersions, CancellationToken cancellationToken)
        {
            var resolutionContext = new ResolutionContext(
               DependencyBehavior.Lowest,
               includePrerelease,
               true,
               includeAllVersions ? VersionConstraints.None : VersionConstraints.ExactMajor | VersionConstraints.ExactMinor);

            var repositories = PackageSources.Select(sourceRepositoryProvider.CreateRepository).ToArray();

            var res = new List<NugetPackage>();
            using (var context = new SourceCacheContext { MaxAge = DateTimeOffset.UtcNow })
            {
                foreach (var repo in repositories)
                {
                    try
                    {
                        var metadataResource = await repo.GetResourceAsync<PackageMetadataResource>(cancellationToken);
                        var metadataList = await metadataResource.GetMetadataAsync(packageName.Id, includePrerelease, includeAllVersions, context, NativeLogger, cancellationToken);
                        foreach (var metadata in metadataList)
                        {
                            if (metadata.IsListed)
                                res.Add(new NugetServerPackage(metadata, repo.PackageSource.Source));
                        }
                    }
                    catch (FatalProtocolException)
                    {
                        // Ignore 404/403 etc... (invalid sources)
                    }
                }
            }
            return res;
        }

        #endregion

        /// <summary>
        ///   Cleans all temporary files created thus far during store operations.
        /// </summary>
        public void PurgeCache() { }

        public string GetRealPath(NugetLocalPackage package)
        {
            if (IsDevRedirectPackage(package) &&
                package.Version < new PackageVersion(3, 1, 0, 0))
            {
                var realPath = File.ReadAllText(GetRedirectFile(package));
                if (!Directory.Exists(realPath))
                    throw new DirectoryNotFoundException();

                return realPath;
            }

            return package.Path;
        }

        public string GetRedirectFile(NugetLocalPackage package)
        {
            return Path.Combine(package.Path, $"{package.Id}.redirect");
        }

        public bool IsDevRedirectPackage(NugetLocalPackage package)
        {
            return package.Version < new PackageVersion(3, 1, 0, 0) ?
                File.Exists(GetRedirectFile(package)) :
                (package.Version.SpecialVersion != null && package.Version.SpecialVersion.StartsWith("dev") && !package.Version.SpecialVersion.Contains('.'));
        }

        public bool IsDevRedirectPackage(NugetServerPackage package)
        {
            return package.Version.SpecialVersion != null &&
                   package.Version.SpecialVersion.StartsWith("dev") &&
                   !package.Version.SpecialVersion.Contains('.');
        }

        private void OnPackageInstalled(object sender, PackageOperationEventArgs args)
        {
            var packageInstallPath = Path.Combine(args.InstallPath, "tools\\packageinstall.exe");
            if (File.Exists(packageInstallPath))
            {
                RunPackageInstall(packageInstallPath, "/install", currentProgressReport);
            }

            NugetPackageInstalled?.Invoke(sender, args);
        }

        private void OnPackageUninstalling(object sender, PackageOperationEventArgs args)
        {
            NugetPackageUninstalling?.Invoke(sender, args);

            try
            {
                var packageInstallPath = Path.Combine(args.InstallPath, "tools\\packageinstall.exe");
                if (File.Exists(packageInstallPath))
                {
                    RunPackageInstall(packageInstallPath, "/uninstall", currentProgressReport);
                }
            }
            catch
            {
                // We mute errors during uninstall since they are usually non-fatal
                //   (OTOH, if we don't catch the exception, the NuGet package isn't uninstalled, which is probably
                //   not what we want)
                //   If we really wanted to deal with them at some point, we should use another mechanism than
                //   exception (i.e. log)
            }
        }

        private void OnPackageUninstalled(object sender, PackageOperationEventArgs args)
        {
            NugetPackageUninstalled?.Invoke(sender, args);
        }

        void INugetDownloadProgress.DownloadProgress(long contentPosition, long contentLength)
        {
            currentProgressReport?.UpdateProgress(ProgressAction.Download, (int) (contentPosition * 100 / contentLength));
        }

        private static void RunPackageInstall(string packageInstall, string arguments, ProgressReport progress)
        {
            // Run packageinstall.exe
            using (var process = Process.Start(new ProcessStartInfo(packageInstall, arguments)
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                WorkingDirectory = Path.GetDirectoryName(packageInstall),
            }))
            {
                if (process is null)
                    throw new InvalidOperationException($"Could not start install package process [{packageInstall}] with options {arguments}.");

                var errorOutput = new StringBuilder();

                process.OutputDataReceived += (_, args) =>
                {
                    if (!string.IsNullOrEmpty(args.Data))
                    {
                        var matches = powerShellProgressRegex.Match(args.Data);
                        if (matches.Success && int.TryParse(matches.Groups[1].Value, out int percentageResult))
                        {
                            // Report progress
                            progress?.UpdateProgress(ProgressAction.Install, percentageResult);
                        }
                        else
                        {
                            lock (process)
                            {
                                errorOutput.AppendLine(args.Data);
                            }
                        }
                    }
                };
                process.ErrorDataReceived += (_, args) =>
                {
                    if (!string.IsNullOrEmpty(args.Data))
                    {
                        // Save errors
                        lock (process)
                        {
                            errorOutput.AppendLine(args.Data);
                        }
                    }
                };

                // Process output and wait for exit
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();

                // Check exit code
                var exitCode = process.ExitCode;
                if (exitCode != 0)
                    throw new InvalidOperationException($"Error code {exitCode} while running install package process [{packageInstall}].\n\n" + errorOutput);
            }
        }

        // Used only for Stride 1.x and 2.x
        private void UpdateTargetsHelper()
        {
            if (oldRootDirectory is null)
                return;

            // Get latest package only for each MainPackageIds (up to 2.x)
            var strideOldPackages = GetLocalPackages("Xenko").Where(package => !((package.Tags != null) && package.Tags.Contains("internal"))).Where(x => x.Version.Version.Major < 3).ToList();

            // Generate target file
            var targetGenerator = new TargetGenerator(this, strideOldPackages, "SiliconStudioPackage");
            var targetFileContent = targetGenerator.TransformText();
            var targetFile = Path.Combine(oldRootDirectory, @"Targets\SiliconStudio.Common.targets");

            var targetFilePath = Path.GetDirectoryName(targetFile);

            // Make sure directory exists
            if (targetFilePath != null && !Directory.Exists(targetFilePath))
                Directory.CreateDirectory(targetFilePath);

            File.WriteAllText(targetFile, targetFileContent, Encoding.UTF8);
        }

        private class PackagePathResolverV3 : PackagePathResolver
        {
            private readonly VersionFolderPathResolver pathResolver;

            public PackagePathResolverV3(string rootDirectory) : base(rootDirectory, true)
            {
                pathResolver = new VersionFolderPathResolver(rootDirectory);
            }

            public override string GetPackageDirectoryName(PackageIdentity packageIdentity)
            {
                return pathResolver.GetPackageDirectory(packageIdentity.Id, packageIdentity.Version);
            }

            public override string GetPackageFileName(PackageIdentity packageIdentity)
            {
                return pathResolver.GetPackageFileName(packageIdentity.Id, packageIdentity.Version);
            }

            public override string GetInstallPath(PackageIdentity packageIdentity)
            {
                return pathResolver.GetInstallPath(packageIdentity.Id, packageIdentity.Version);
            }

            public override string GetInstalledPath(PackageIdentity packageIdentity)
            {
                var installPath = GetInstallPath(packageIdentity);
                var installPackagePath = GetInstalledPackageFilePath(packageIdentity);
                return File.Exists(installPackagePath) ? installPath : null;
            }

            public override string GetInstalledPackageFilePath(PackageIdentity packageIdentity)
            {
                var installPath = GetInstallPath(packageIdentity);
                var installPackagePath = Path.Combine(installPath, packageIdentity.ToString().ToLower() + PackagingCoreConstants.NupkgExtension);
                return File.Exists(installPackagePath) ? installPackagePath : null;
            }
        }
    }
}
