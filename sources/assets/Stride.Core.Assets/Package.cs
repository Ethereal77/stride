// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Stride.Core.IO;
using Stride.Core.Annotations;
using Stride.Core.Diagnostics;
using Stride.Core.Extensions;
using Stride.Core.Reflection;
using Stride.Core.Serialization;
using Stride.Core.Yaml;
using Stride.Core.Assets.Analysis;
using Stride.Core.Assets.Diagnostics;
using Stride.Core.Assets.Templates;
using Stride.Core.Assets.Yaml;

namespace Stride.Core.Assets
{
    /// <summary>
    ///   Defines the different states a <see cref="Package"/> can be in during package loading.
    /// </summary>
    public enum PackageState
    {
        /// <summary>
        ///   Package has been deserialized. References and assets are not ready.
        /// </summary>
        Raw,

        /// <summary>
        ///   Dependencies have all been resolved and are also in <see cref="DependenciesReady"/> state.
        /// </summary>
        DependenciesReady,

        /// <summary>
        ///   Package upgrade has failed either by error or denied by the user.
        ///   Dependencies are ready, but not assets.
        ///   Should be manually switched back to <see cref="DependenciesReady"/> to try the upgrade again.
        /// </summary>
        UpgradeFailed,

        /// <summary>
        ///   Assembly references and assets have all been loaded.
        /// </summary>
        AssetsReady,
    }

    /// <summary>
    ///   Represents a package, a collection of assets and their dependencies.
    /// </summary>
    [DataContract("Package")]
    [NonIdentifiableCollectionItems]
    [AssetDescription(PackageFileExtension)]
    [DebuggerDisplay("Name: {Meta.Name}, Version: {Meta.Version}, Assets [{Assets.Count}]")]
    [AssetFormatVersion("Assets", PackageFileVersion, "0.0.0.4")]
    [AssetUpgrader("Assets", "0.0.0.4", "3.1.0.0", typeof(MovePackageInsideProject))]
    public sealed partial class Package : IFileSynchronizable, IAssetFinder
    {
        private const string PackageFileVersion = "3.1.0.0";

        private UFile packagePath;
        internal UFile PreviousPackagePath;

        private readonly Lazy<PackageUserSettings> settings;

        private PackageSession session;
        private bool isDirty;

        internal readonly List<UFile> FilesToDelete = new List<UFile>();

        // NOTE: Please keep this code in sync with Asset class.

        /// <summary>
        ///   Gets or sets the version number for this package. Used internally when migrating assets.
        /// </summary>
        /// <value>The version of this package.</value>
        [DataMember(-8000, DataMemberMode.Assign)]
        [DataStyle(DataStyle.Compact)]
        [Display(Browsable = false)]
        [DefaultValue(null)]
        [NonOverridable]
        [NonIdentifiableCollectionItems]
        public Dictionary<string, PackageVersion> SerializedVersion { get; set; }

        /// <summary>
        ///   Gets a value indicating whether this package is a system (non-local) package.
        /// </summary>
        /// <value><c>true</c> if this package is a system package; otherwise, <c>false</c>.</value>
        [DataMemberIgnore]
        public bool IsSystem => !(Container is SolutionProject);

        /// <summary>
        ///   Gets or sets the metadata associated with this package.
        /// </summary>
        /// <value>A <see cref="PackageMeta"/> containing the metadata associated with this package.</value>
        [DataMember(10)]
        public PackageMeta Meta { get; set; } = new PackageMeta();

        /// <summary>
        ///   Gets or sets the asset directories to lookup.
        /// </summary>
        /// <value>The asset directories.</value>
        [DataMember(40, DataMemberMode.Assign)]
        public AssetFolderCollection AssetFolders { get; set; } = new AssetFolderCollection();

        /// <summary>
        ///   Gets or sets the resource directories to lookup.
        /// </summary>
        /// <value>The resource directories.</value>
        [DataMember(45, DataMemberMode.Assign)]
        public List<UDirectory> ResourceFolders { get; set; } = new List<UDirectory>();

        /// <summary>
        ///   Gets or sets the output group directories.
        /// </summary>
        /// <value>The output group directories.</value>
        [DataMember(50, DataMemberMode.Assign)]
        public Dictionary<string, UDirectory> OutputGroupDirectories { get; set; } = new Dictionary<string, UDirectory>();

        /// <summary>
        ///   Gets the list of folders that are explicitly created but contains no assets.
        /// </summary>
        [DataMember(70)]
        public List<UDirectory> ExplicitFolders { get; } = new List<UDirectory>();

        /// <summary>
        ///   Gets the bundles defined for this package.
        /// </summary>
        /// <value>The bundles defined for this package.</value>
        [DataMember(80)]
        public BundleCollection Bundles { get; private set; }

        /// <summary>
        ///   Gets the template folders.
        /// </summary>
        /// <value>The template folders.</value>
        [DataMember(90)]
        public List<TemplateFolder> TemplateFolders { get; } = new List<TemplateFolder>();

        /// <summary>
        ///   Gets the asset references that needs to be compiled even if not directly or indirectly referenced
        ///   (useful for explicit code references).
        /// </summary>
        [DataMember(100)]
        public RootAssetCollection RootAssets { get; private set; } = new RootAssetCollection();

        /// <summary>
        ///   Gets the loaded templates from the <see cref="TemplateFolders"/>.
        /// </summary>
        /// <value>The templates.</value>
        [DataMemberIgnore]
        public List<TemplateDescription> Templates { get; } = new List<TemplateDescription>();

        /// <summary>
        ///   Gets the assets stored in this package.
        /// </summary>
        /// <value>A <see cref="PackageAssetCollection"/> with the assets stored in this package.</value>
        [DataMemberIgnore]
        public PackageAssetCollection Assets { get; }

        /// <summary>
        ///   Gets the temporary assets loaded from disk before they are inserted into <see cref="Assets"/>.
        /// </summary>
        /// <value>List of temporary <see cref="AssetItem"/>.</value>
        [DataMemberIgnore]
        // TODO: Turn that internal!
        public List<AssetItem> TemporaryAssets { get; } = new List<AssetItem>();

        /// <summary>
        ///   Gets or sets the full path to the package file. May be <c>null</c> if the package was not loaded or saved.
        /// </summary>
        /// <value>The package path.</value>
        [DataMemberIgnore]
        public UFile FullPath
        {
            get => packagePath;
            set => SetPackagePath(value, true);
        }

        /// <summary>
        ///   Gets or sets a value indicating whether this instance has been modified since the last time it was saved.
        /// </summary>
        /// <value><c>true</c> if this instance is modified; otherwise, <c>false</c>.</value>
        [DataMemberIgnore]
        public bool IsDirty
        {
            get => isDirty;
            set
            {
                var oldValue = isDirty;
                isDirty = value;
                OnPackageDirtyChanged(this, oldValue, value);
            }
        }

        /// <summary>
        ///   Gets or sets the current loading state of this package.
        /// </summary>
        /// <value>A <see cref="PackageState"/> indicating the current loading state.</value>
        [DataMemberIgnore]
        public PackageState State { get; set; }

        /// <summary>
        ///   Gets the top directory of this package on the local disk.
        /// </summary>
        /// <value>The top directory.</value>
        [DataMemberIgnore]
        public UDirectory RootDirectory => FullPath?.GetParent();

        [DataMemberIgnore]
        public PackageContainer Container { get; internal set; }

        /// <summary>
        ///   Gets the session this package is currently attached to.
        /// </summary>
        /// <value>The <see cref="PackageSession"/> this package is attached to.</value>
        [DataMemberIgnore]
        public PackageSession Session => Container?.Session;

        /// <summary>
        ///   Gets the package user settings. Usually stored in a <c>.user</c> file alongside the package. Lazily loaded on first time.
        /// </summary>
        /// <value>A <see cref="PackageUserSettings"/> containing the package user settings.</value>
        [DataMemberIgnore]
        public PackageUserSettings UserSettings => settings.Value;

        /// <summary>
        ///   Gets the list of assemblies loaded by this package.
        /// </summary>
        /// <value>A collection of <see cref="PackageLoadedAssembly"/> representing the loaded assemblies.</value>
        [DataMemberIgnore]
        public List<PackageLoadedAssembly> LoadedAssemblies { get; } = new List<PackageLoadedAssembly>();

        [DataMemberIgnore]
        public string RootNamespace { get; private set; }

        [DataMemberIgnore]
        public bool IsImplicitProject
        {
            // Keep in sync with LoadProject() .csproj
            // NOTE: Meta is ignored since it is supposedly "read-only" from csproj
            get => AssetFolders.Count == 1 && AssetFolders.First().Path == "Assets" &&
                   ResourceFolders.Count == 1 && ResourceFolders.First() == "Resources" &&
                   OutputGroupDirectories.Count == 0 &&
                   ExplicitFolders.Count == 0 &&
                   Bundles.Count == 0 &&
                   RootAssets.Count == 0 &&
                   TemplateFolders.Count == 0;
        }


        /// <summary>
        ///   Occurs when the value of the property <see cref="IsDirty"/> of this package has changed.
        /// </summary>
        public event DirtyFlagChangedDelegate<Package> PackageDirtyChanged;

        /// <summary>
        ///   Occurs when the value of the property <see cref="AssetItem.IsDirty"/> of any of the assets in this package has changed.
        /// </summary>
        public event DirtyFlagChangedDelegate<AssetItem> AssetDirtyChanged;


        /// <summary>
        ///   Initializes a new instance of the <see cref="Package"/> class.
        /// </summary>
        public Package()
        {
            // Initializse package with default versions (same code as in Asset constructor)
            var defaultPackageVersion = AssetRegistry.GetCurrentFormatVersions(GetType());
            if (defaultPackageVersion != null)
            {
                SerializedVersion = new Dictionary<string, PackageVersion>(defaultPackageVersion);
            }

            Assets = new PackageAssetCollection(this);
            Bundles = new BundleCollection(this);

            IsDirty = true;

            settings = new Lazy<PackageUserSettings>(() => new PackageUserSettings(this));
        }


        /// <summary>
        ///   Adds an existing project to this package.
        /// </summary>
        /// <param name="pathToProj">The path to the project to add.</param>
        /// <returns>A <see cref="LoggerResult"/> with the results of the operation.</returns>
        public LoggerResult AddExistingProject(UFile pathToProj)
        {
            var logger = new LoggerResult();
            AddExistingProject(pathToProj, logger);
            return logger;
        }

        /// <summary>
        ///   Adds an existing project to this package.
        /// </summary>
        /// <param name="pathToProj">The path to the project to add.</param>
        /// <param name="logger">A <see cref="LoggerResult"/> in which to log the results of the operation.</param>
        /// <exception cref="ArgumentNullException"><paramref name="pathToProj"/> is a <c>null</c> reference.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="logger"/> is a <c>null</c> reference.</exception>
        /// <exception cref="ArgumentException"><paramref name="pathToProj"/> is an absolute path. Expected a relative path instead.</exception>
        public void AddExistingProject(UFile pathToProj, LoggerResult logger)
        {
            if (pathToProj is null)
                throw new ArgumentNullException(nameof(pathToProj));
            if (logger is null)
                throw new ArgumentNullException(nameof(logger));
            if (!pathToProj.IsAbsolute)
                throw new ArgumentException(@"Expected a relative path.", nameof(pathToProj));

            try
            {
                // Load a project without specifying a platform to make sure we get the correct platform type
                var msProject = VSProjectHelper.LoadProject(pathToProj, platform: "NoPlatform");
                try
                {
                    var projectType = VSProjectHelper.GetProjectTypeFromProject(msProject);
                    if (!projectType.HasValue)
                    {
                        logger.Error("This project is not a project created with the editor.");
                    }
                    else
                    {
                        var platformType = VSProjectHelper.GetPlatformTypeFromProject(msProject) ?? PlatformType.Shared;
                        var projectReference = new ProjectReference(VSProjectHelper.GetProjectGuid(msProject), pathToProj.MakeRelative(RootDirectory), projectType.Value);

                        // TODO: CSPROJ=SDPKG
                        throw new NotImplementedException();
                        // Add the ProjectReference only for the compatible profiles (same platform or no platform)
                        //foreach (var profile in Profiles.Where(profile => platformType == profile.Platform))
                        //{
                        //    profile.ProjectReferences.Add(projectReference);
                        //}
                    }
                }
                finally
                {
                    msProject.ProjectCollection.UnloadAllProjects();
                    msProject.ProjectCollection.Dispose();
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Unexpected exception while loading project [{pathToProj}].", ex);
            }
        }

        /// <inheritdoc />
        /// <remarks>Looks for the asset amongst the current package and its dependencies.</remarks>
        public AssetItem FindAsset(AssetId assetId)
        {
            return this.GetPackagesWithDependencies().Select(p => p.Assets.Find(assetId)).NotNull().FirstOrDefault();
        }

        /// <inheritdoc />
        /// <remarks>Looks for the asset amongst the current package and its dependencies.</remarks>
        public AssetItem FindAsset(UFile location)
        {
            return this.GetPackagesWithDependencies().Select(p => p.Assets.Find(location)).NotNull().FirstOrDefault();
        }

        /// <inheritdoc />
        /// <remarks>Looks for the asset amongst the current package and its dependencies.</remarks>
        public AssetItem FindAssetFromProxyObject(object proxyObject)
        {
            var attachedReference = AttachedReferenceManager.GetAttachedReference(proxyObject);
            return attachedReference != null ? this.FindAsset(attachedReference) : null;
        }

        public UDirectory GetDefaultAssetFolder()
        {
            var folder = AssetFolders.FirstOrDefault();
            return folder?.Path ?? ("Assets");
        }

        /// <summary>
        ///   Returns a deep clone of this package.
        /// </summary>
        /// <returns>A <see cref="Package"/> that is a deep clone of this instance.</returns>
        public Package Clone()
        {
            // Use a new ShadowRegistry to copy override parameters
            // Clone this asset
            var package = AssetCloner.Clone(this);
            package.FullPath = FullPath;
            foreach (var asset in Assets)
            {
                var newAsset = asset.Asset;
                var assetItem = new AssetItem(asset.Location, newAsset)
                {
                    SourceFolder = asset.SourceFolder,
                    AlternativePath = asset.AlternativePath,
                };
                package.Assets.Add(assetItem);
            }
            return package;
        }

        /// <summary>
        ///   Sets the path for this package.
        /// </summary>
        /// <param name="newPath">The new path for this package.</param>
        /// <param name="copyAssets">
        ///   Value to indicate whether to copy the assets to a path relative to the new location (<c>true</c>) or
        ///   keep them on their current location (<c>false</c>).
        ///   Default is <c>true</c>.
        /// </param>
        public void SetPackagePath(UFile newPath, bool copyAssets = true)
        {
            var previousPath = packagePath;
            var previousRootDirectory = RootDirectory;
            packagePath = newPath;
            if (packagePath != null && !packagePath.IsAbsolute)
            {
                packagePath = UPath.Combine(Environment.CurrentDirectory, packagePath);
            }

            if (copyAssets && packagePath != previousPath)
            {
                // Update source folders
                var currentRootDirectory = RootDirectory;
                if (previousRootDirectory != null && currentRootDirectory != null)
                {
                    foreach (var sourceFolder in AssetFolders)
                    {
                        if (sourceFolder.Path.IsAbsolute)
                        {
                            var relativePath = sourceFolder.Path.MakeRelative(previousRootDirectory);
                            sourceFolder.Path = UPath.Combine(currentRootDirectory, relativePath);
                        }
                    }
                }

                foreach (var asset in Assets)
                {
                    asset.IsDirty = true;
                }
                IsDirty = true;
            }
        }

        internal void OnPackageDirtyChanged(Package package, bool oldValue, bool newValue)
        {
            if (package is null)
                throw new ArgumentNullException(nameof(package));

            PackageDirtyChanged?.Invoke(package, oldValue, newValue);
        }

        internal void OnAssetDirtyChanged(AssetItem asset, bool oldValue, bool newValue)
        {
            if (asset is null)
                throw new ArgumentNullException(nameof(asset));

            AssetDirtyChanged?.Invoke(asset, oldValue, newValue);
        }

        public static bool SaveSingleAsset(AssetItem asset, ILogger log)
        {
            // Make sure AssetItem.SourceFolder/Project are generated if they were null
            asset.UpdateSourceFolders();
            return SaveSingleAsset_NoUpdateSourceFolder(asset, log);
        }

        internal static bool SaveSingleAsset_NoUpdateSourceFolder(AssetItem asset, ILogger log)
        {
            var assetPath = asset.FullPath;

            try
            {
                // Handle the ProjectSourceCodeAsset differently then regular assets in regards of Path
                if (asset.Asset is IProjectAsset)
                {
                    assetPath = asset.FullPath;
                }

                // Inject a copy of the base into the current asset when saving
                AssetFileSerializer.Save(assetPath, asset.Asset, asset.YamlMetadata, log);

                // Save generated asset (if necessary)
                if (asset.Asset is IProjectFileGeneratorAsset codeGeneratorAsset)
                {
                    codeGeneratorAsset.SaveGeneratedAsset(asset);
                }

                asset.IsDirty = false;
            }
            catch (Exception ex)
            {
                log.Error(asset.Package, asset.ToReference(), AssetMessageCode.AssetCannotSave, ex, assetPath);
                return false;
            }
            return true;
        }

        /// <summary>
        ///   Gets the package identifier from a file.
        /// </summary>
        /// <param name="filePath">The file path to the package.</param>
        /// <returns>A <see cref="Guid"/> that represents the package unique identifier.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="filePath"/> is a <c>null</c> reference.</exception>
        /// <exception cref="IOException">The file doesn't appear to be a valid package.</exception>
        public static Guid GetPackageIdFromFile(string filePath)
        {
            if (filePath is null)
                throw new ArgumentNullException(nameof(filePath));

            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            using (var reader = new StreamReader(stream))
            {
                string line;
                bool hasPackage = false;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("!Package"))
                    {
                        hasPackage = true;
                    }

                    if (hasPackage && line.StartsWith("Id:"))
                    {
                        var id = line.Substring("Id:".Length).Trim();
                        return Guid.Parse(id);
                    }
                }
            }

            throw new IOException($"File {filePath} doesn't appear to be a valid package.");
        }

        /// <summary>
        ///   Loads only the package description but not assets or plugins.
        /// </summary>
        /// <param name="log">The <see cref="ILogger"/> in which to log the results of the operation.</param>
        /// <param name="filePath">The file path of the package to load.</param>
        /// <param name="loadParameters">The parameters to use to load the package.</param>
        /// <returns>The loaded <see cref="Package"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="log"/> is a <c>null</c> reference.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="filePath"/> is a <c>null</c> reference.</exception>
        public static Package Load(ILogger log, string filePath, PackageLoadParameters loadParameters = null)
        {
            var package = LoadProject(log, filePath)?.Package;

            if (package != null)
            {
                if (!package.LoadAssembliesAndAssets(log, loadParameters))
                    package = null;
            }

            return package;
        }

        /// <summary>
        ///   Performs the first part of the loading sequence by deserializing the package but without processing anything yet.
        /// </summary>
        /// <param name="log">The <see cref="ILogger"/> in which to log the results of the operation.</param>
        /// <param name="filePath">The file path of the package to load.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="log"/> is a <c>null</c> reference.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="filePath"/> is a <c>null</c> reference.</exception>
        /// <exception cref="InvalidOperationException">An error occurred while pre-loading the package.</exception>
        internal static Package LoadRaw(ILogger log, string filePath)
        {
            if (log is null)
                throw new ArgumentNullException(nameof(log));
            if (filePath is null)
                throw new ArgumentNullException(nameof(filePath));

            filePath = FileUtility.GetAbsolutePath(filePath);

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Package file [{filePath}] was not found");

            try
            {
                var packageFile = new PackageLoadingAssetFile(filePath, Path.GetDirectoryName(filePath)) { CachedFileSize = filePath.Length };
                var context = new AssetMigrationContext(null, null, filePath, log);
                AssetMigration.MigrateAssetIfNeeded(context, packageFile, "Assets");

                var loadResult = packageFile.AssetContent != null
                    ? AssetFileSerializer.Load<Package>(new MemoryStream(packageFile.AssetContent), filePath, log)
                    : AssetFileSerializer.Load<Package>(filePath, log);
                var package = loadResult.Asset;
                package.FullPath = packageFile.FilePath;
                package.PreviousPackagePath = packageFile.OriginalFilePath;
                package.IsDirty = packageFile.AssetContent != null || loadResult.AliasOccurred;

                return package;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error while pre-loading package [{filePath}].", ex);
            }
        }

        public static PackageContainer LoadProject(ILogger log, string filePath)
        {
            if (Path.GetExtension(filePath).ToLowerInvariant() == ".csproj")
            {
                var projectPath = filePath;
                var packagePath = Path.ChangeExtension(filePath, Package.PackageFileExtension);
                var packageExists = File.Exists(packagePath);

                // Xenko to Stride migration
                if (!packageExists)
                {
                    var oldPackagePath = Path.ChangeExtension(filePath, ".xkpkg");
                    if (File.Exists(oldPackagePath))
                    {
                        packageExists = true;
                        XenkoToStrideRenameHelper.RenameStrideFile(oldPackagePath, XenkoToStrideRenameHelper.StrideContentType.Package);
                    }
                }

                var package = packageExists
                    ? LoadRaw(log, packagePath)
                    : new Package
                    {
                        Meta = { Name = Path.GetFileNameWithoutExtension(packagePath) },
                        AssetFolders = { new AssetFolder("Assets") },
                        ResourceFolders = { "Resources" },
                        FullPath = packagePath,
                        IsDirty = false,
                    };
                return new SolutionProject(package, Guid.NewGuid(), projectPath) { IsImplicitProject = !packageExists };
            }
            else
            {
                var package = LoadRaw(log, filePath);

                // Find the .csproj next to .sdpkg (if any)
                // Note that we use package.FullPath since we must first perform package upgrade from 3.0 to 3.1+ (might move package in .csproj folder)
                var projectPath = Path.ChangeExtension(package.FullPath.ToWindowsPath(), ".csproj");
                if (File.Exists(projectPath))
                {
                    return new SolutionProject(package, Guid.NewGuid(), projectPath);
                }
                else
                {
                    // Try to get version from NuGet folder
                    var path = new UFile(filePath);
                    var nuspecPath = UPath.Combine(path.GetFullDirectory().GetParent(), new UFile(path.GetFileNameWithoutExtension() + ".nuspec"));
                    if (path.GetFullDirectory().GetDirectoryName() == "stride" && File.Exists(nuspecPath)
                        && PackageVersion.TryParse(path.GetFullDirectory().GetParent().GetDirectoryName(), out var packageVersion))
                    {
                        package.Meta.Version = packageVersion;
                    }
                    return new StandalonePackage(package);
                }
            }
        }

        private static PackageVersion TryGetPackageVersion(string projectPath)
        {
            try
            {
                // Load a project without specifying a platform to make sure we get the correct platform type
                var msProject = VSProjectHelper.LoadProject(projectPath, platform: "NoPlatform");
                try
                {
                    var packageVersion = msProject.GetPropertyValue("PackageVersion");
                    return !string.IsNullOrEmpty(packageVersion) ? new PackageVersion(packageVersion) : null;
                }
                finally
                {
                    msProject.ProjectCollection.UnloadAllProjects();
                    msProject.ProjectCollection.Dispose();
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        ///   Performs the second part of the package loading process, when references, assets and package analysis is done.
        /// </summary>
        /// <param name="log">The <see cref="ILogger"/> in which to log the results of the operation.</param>
        /// <param name="loadParameters">The parameters to use to load the package.</param>
        /// <returns></returns>
        internal bool LoadAssembliesAndAssets(ILogger log, PackageLoadParameters loadParameters)
        {
            return LoadAssemblies(log, loadParameters) &&
                   LoadAssets(log, loadParameters);
        }

        /// <summary>
        ///   Loads only the assembly references for this package.
        /// </summary>
        /// <param name="log">The <see cref="ILogger"/> in which to log the results of the operation.</param>
        /// <param name="loadParameters">The parameters to use to load the package.</param>
        /// <returns>Value indicating whether the assembly references have been loaded.</returns>
        internal bool LoadAssemblies(ILogger log, PackageLoadParameters loadParameters)
        {
            var loadParams = loadParameters ?? PackageLoadParameters.Default();

            try
            {
                // Load assembly references
                if (loadParams.LoadAssemblyReferences)
                {
                    LoadAssemblyReferencesForPackage(log, loadParams);
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Error($"Error while pre-loading package [{FullPath}].", ex);

                return false;
            }
        }

        /// <summary>
        ///   Load assets and perform package analysis.
        /// </summary>
        /// <param name="log">The <see cref="ILogger"/> in which to log the results of the operation.</param>
        /// <param name="loadParameters">The parameters to use to load the package.</param>
        /// <returns>Value indicating whether the assets are loaded and the analysis is done.</returns>
        internal bool LoadAssets(ILogger log, PackageLoadParameters loadParameters)
        {
            var loadParams = loadParameters ?? PackageLoadParameters.Default();

            try
            {
                // Load assets
                if (loadParams.AutoLoadTemporaryAssets)
                {
                    LoadTemporaryAssets(log, loadParams.AssetFiles, loadParams.CancelToken, loadParams.TemporaryAssetsInMsBuild, loadParams.TemporaryAssetFilter);
                }

                // Convert UPath to absolute
                if (loadParams.ConvertUPathToAbsolute)
                {
                    var analysis = new PackageAnalysis(this, new PackageAnalysisParameters()
                    {
                        ConvertUPathTo = UPathType.Absolute,
                        IsProcessingUPaths = true, // This is done already by Package.Load
                        SetDirtyFlagOnAssetWhenFixingAbsoluteUFile = true // When loading tag attributes that have an absolute file
                    });
                    analysis.Run(log);
                }

                // Load templates
                LoadTemplates(log);

                return true;
            }
            catch (Exception ex)
            {
                log.Error($"Error while pre-loading package [{FullPath}].", ex);

                return false;
            }
        }

        public void ValidateAssets(bool alwaysGenerateNewAssetId, bool removeUnloadableObjects, ILogger log)
        {
            if (TemporaryAssets.Count == 0)
                return;

            try
            {
                // Make sure we are suspending notifications before updating all assets
                Assets.SuspendCollectionChanged();

                Assets.Clear();

                // Get generated output items
                var outputItems = new List<AssetItem>();

                // Create a resolver from the package
                var resolver = AssetResolver.FromPackage(this);
                resolver.AlwaysCreateNewId = alwaysGenerateNewAssetId;

                // Clean assets
                AssetCollision.Clean(this, TemporaryAssets, outputItems, resolver, cloneInput: true, removeUnloadableObjects);

                // Add them back to the package
                foreach (var item in outputItems)
                {
                    Assets.Add(item);

                    // Fix collection item ids
                    AssetCollectionItemIdHelper.GenerateMissingItemIds(item.Asset);
                    CollectionItemIdsAnalysis.FixupItemIds(item, log);

                    // Fix duplicate identifiable objects
                    var hasBeenModified = IdentifiableObjectAnalysis.Visit(item.Asset, true, log);
                    if (hasBeenModified)
                        item.IsDirty = true;
                }

                // Don't delete SourceCodeAssets as their files are handled by the package upgrader
                var dirtyAssets = outputItems
                    .Where(o => o.IsDirty && !(o.Asset is SourceCodeAsset))
                    .Join(TemporaryAssets, o => o.Id, t => t.Id, (o, t) => t)
                    .ToList();
                // Dirty assets (except in system package) should be mark as deleted so that are properly saved again later.
                if (!IsSystem && dirtyAssets.Count > 0)
                {
                    IsDirty = true;

                    lock (FilesToDelete)
                    {
                        FilesToDelete.AddRange(dirtyAssets.Select(a => a.FullPath));
                    }
                }

                TemporaryAssets.Clear();
            }
            finally
            {
                // Restore notification on assets
                Assets.ResumeCollectionChanged();
            }
        }

        /// <summary>
        ///   Refreshes this package from the disk by loading or reloading all assets.
        /// </summary>
        /// <param name="log">The <see cref="ILogger"/> in which to log the results of the operation.</param>
        /// <param name="assetFiles">The asset files to load. If <c>null</c>, the ones returned by <see cref="ListAssetFiles"/> will be loaded.</param>
        /// <param name="cancelToken">The cancellation token that can be used to cancel the operation.</param>
        /// <param name="listAssetsInMsBuild">Specifies if we need to evaluate MSBuild files for assets.</param>
        /// <param name="filterFunc">A function that will filter the assets to load.</param>
        /// <exception cref="ArgumentNullException"><paramref name="log"/> is a <c>null</c> reference.</exception>
        public void LoadTemporaryAssets(ILogger log, List<PackageLoadingAssetFile> assetFiles = null, CancellationToken? cancelToken = null,
                                        bool listAssetsInMsBuild = true, Func<PackageLoadingAssetFile, bool> filterFunc = null)
        {
            if (log is null)
                throw new ArgumentNullException(nameof(log));

            // If FullPath is null, then we can't load assets from disk, just return
            if (FullPath is null)
            {
                log.Warning("FullPath is not set on this package.");
                return;
            }

            // Clears the assets already loaded and reload them
            TemporaryAssets.Clear();

            // List all package files on disk
            if (assetFiles is null)
            {
                assetFiles = ListAssetFiles(package: this, listAssetsInMsBuild, listUnregisteredAssets: false);
                // Sort them by size (to improve concurrency during load)
                assetFiles.Sort(PackageLoadingAssetFile.FileSizeComparer.Default);
            }

            var progressMessage = $"Loading Assets from Package [{FullPath.GetFileName()}].";

            // Display this message at least once if the logger does not log progress (And it shouldn't in this case)
            var loggerResult = log as LoggerResult;
            if (loggerResult is null || !loggerResult.IsLoggingProgressAsInfo)
            {
                log.Verbose(progressMessage);
            }

            // Update step counter for log progress
            var tasks = new List<Task>();
            for (int i = 0; i < assetFiles.Count; i++)
            {
                var assetFile = assetFiles[i];

                if (filterFunc != null && !filterFunc(assetFile))
                    continue;

                // Update the loading progress
                loggerResult?.Progress(progressMessage, i, assetFiles.Count);

                var task = cancelToken.HasValue ?
                    Task.Factory.StartNew(() => LoadAsset(new AssetMigrationContext(this, assetFile.ToReference(), assetFile.FilePath.ToWindowsPath(), log), assetFile), cancelToken.Value) :
                    Task.Factory.StartNew(() => LoadAsset(new AssetMigrationContext(this, assetFile.ToReference(), assetFile.FilePath.ToWindowsPath(), log), assetFile));

                tasks.Add(task);
            }

            if (cancelToken.HasValue)
            {
                Task.WaitAll(tasks.ToArray(), cancelToken.Value);
            }
            else
            {
                Task.WaitAll(tasks.ToArray());
            }

            // DEBUG
            // StaticLog.Info("[{0}] Assets files loaded in {1}", assetFiles.Count, clock.ElapsedMilliseconds);

            if (cancelToken.HasValue && cancelToken.Value.IsCancellationRequested)
            {
                log.Warning("Skipping loading assets. PackageSession.Load cancelled.");
            }
        }

        private void LoadAsset(AssetMigrationContext context, PackageLoadingAssetFile assetFile)
        {
            var fileUPath = assetFile.FilePath;
            var sourceFolder = assetFile.SourceFolder;

            // Check if asset has been deleted by an upgrader
            if (assetFile.Deleted)
            {
                IsDirty = true;

                lock (FilesToDelete)
                {
                    FilesToDelete.Add(assetFile.FilePath);
                }

                // Don't create temporary assets for files deleted during package upgrading
                return;
            }

            // An exception can occur here, so we make sure that loading a single asset is not going to break the loop
            try
            {
                AssetMigration.MigrateAssetIfNeeded(context, assetFile, "Stride");

                // Try to load only if asset is not already in the package or assetRef.Asset is null
                var assetPath = assetFile.AssetLocation;

                var assetFullPath = fileUPath.ToWindowsPath();
                var assetContent = assetFile.AssetContent;

                var asset = LoadAsset(context.Log, Meta.Name, assetFullPath, assetPath.ToWindowsPath(), assetContent,
                                      out bool aliasOccurred, out AttachedYamlAssetMetadata yamlMetadata);

                // Create asset item
                var assetItem = new AssetItem(assetPath, asset, this)
                {
                    IsDirty = assetContent != null || aliasOccurred,
                    SourceFolder = sourceFolder.MakeRelative(RootDirectory),
                    AlternativePath = assetFile.Link != null ? assetFullPath : null,
                };
                yamlMetadata.CopyInto(assetItem.YamlMetadata);

                // Set the modified time to the time loaded from disk
                if (!assetItem.IsDirty)
                    assetItem.ModifiedTime = File.GetLastWriteTime(assetFullPath);

                // TODO: Let's review that when we rework import process
                // Not fixing asset import anymore, as it was only meant for upgrade
                // However, it started to make asset dirty, for ex. when we create a new texture, choose a file and reload the scene later
                // since there was no importer id and base.
                //FixAssetImport(assetItem);

                // Add to temporary assets
                lock (TemporaryAssets)
                {
                    TemporaryAssets.Add(assetItem);
                }
            }
            catch (Exception ex)
            {
                var assetReference = new AssetReference(AssetId.Empty, fileUPath.FullPath);
                context.Log.Error(this, assetReference, AssetMessageCode.AssetLoadingFailed, ex, fileUPath, ex.Message);
            }
        }

        /// <summary>
        ///   Loads the assembly references that were not loaded before.
        /// </summary>
        /// <param name="log">The <see cref="ILogger"/> in which to log the results of the operation.</param>
        /// <param name="loadParameters">The parameters to use to load the package.</param>
        public void UpdateAssemblyReferences(ILogger log, PackageLoadParameters loadParameters = null)
        {
            if (State < PackageState.DependenciesReady)
                return;

            var loadParams = loadParameters ?? PackageLoadParameters.Default();
            LoadAssemblyReferencesForPackage(log, loadParams);
        }

        private static Asset LoadAsset(ILogger log, string packageName, string assetFullPath, string assetPath, byte[] assetContent, out bool assetDirty, out AttachedYamlAssetMetadata yamlMetadata)
        {
            var loadResult = assetContent != null
                ? AssetFileSerializer.Load<Asset>(new MemoryStream(assetContent), assetFullPath, log)
                : AssetFileSerializer.Load<Asset>(assetFullPath, log);

            assetDirty = loadResult.AliasOccurred;
            yamlMetadata = loadResult.YamlMetadata;

            // Set location on source code asset
            if (loadResult.Asset is SourceCodeAsset sourceCodeAsset)
            {
                // Use an id generated from the location instead of the default id
                sourceCodeAsset.Id = SourceCodeAsset.GenerateIdFromLocation(packageName, assetPath);
            }

            return loadResult.Asset;
        }

        private void LoadAssemblyReferencesForPackage(ILogger log, PackageLoadParameters loadParameters)
        {
            if (log is null)
                throw new ArgumentNullException(nameof(log));
            if (loadParameters is null)
                throw new ArgumentNullException(nameof(loadParameters));

            var assemblyContainer = loadParameters.AssemblyContainer ?? AssemblyContainer.Default;

            // Load from package
            if (Container is StandalonePackage standalonePackage)
            {
                foreach (var assemblyPath in standalonePackage.Assemblies)
                {
                    LoadAssemblyReferenceInternal(log, loadParameters, assemblyContainer, null, assemblyPath);
                }
            }

            // Load from .csproj
            if (Container is SolutionProject project && project.FullPath != null && project.Type == ProjectType.Library)
            {
                // Check if already loaded
                // TODO: More advanced cases: unload removed references, etc...
                var projectReference = new ProjectReference(project.Id, project.FullPath, ProjectType.Library);

                LoadAssemblyReferenceInternal(log, loadParameters, assemblyContainer, projectReference, project.TargetPath);
            }
        }

        private void LoadAssemblyReferenceInternal(ILogger log, PackageLoadParameters loadParameters, AssemblyContainer assemblyContainer, ProjectReference projectReference, string assemblyPath)
        {
            try
            {
                // Check if already loaded
                if (projectReference != null && LoadedAssemblies.Any(x => x.ProjectReference == projectReference))
                    return;
                else if (LoadedAssemblies.Any(x => string.Compare(x.Path, assemblyPath, true) == 0))
                    return;

                var forwardingLogger = new ForwardingLoggerResult(log);

                // If .csproj, we might need to compile it
                if (projectReference != null)
                {
                    var fullProjectLocation = projectReference.Location.ToWindowsPath();
                    if (loadParameters.AutoCompileProjects || string.IsNullOrWhiteSpace(assemblyPath))
                    {
                        assemblyPath = VSProjectHelper.GetOrCompileProjectAssembly(fullProjectLocation, forwardingLogger, "Build", loadParameters.AutoCompileProjects, loadParameters.BuildConfiguration, extraProperties: loadParameters.ExtraCompileProperties, onlyErrors: true);
                        if (string.IsNullOrWhiteSpace(assemblyPath))
                        {
                            log.Error($"Unable to locate assembly reference for project [{fullProjectLocation}].");
                            return;
                        }
                    }
                }

                var loadedAssembly = new PackageLoadedAssembly(projectReference, assemblyPath);
                LoadedAssemblies.Add(loadedAssembly);

                if (!File.Exists(assemblyPath) || forwardingLogger.HasErrors)
                {
                    log.Error($"Unable to build assembly reference [{assemblyPath}]");
                    return;
                }

                // Check if assembly is already loaded in AppDomain (for Stride core assemblies that are not plugins)
                var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => string.Compare(x.GetName().Name, Path.GetFileNameWithoutExtension(assemblyPath), StringComparison.InvariantCultureIgnoreCase) == 0);

                // Otherwise, load assembly from its file
                if (assembly == null)
                {
                    assembly = assemblyContainer.LoadAssemblyFromPath(assemblyPath, log);

                    if (assembly == null)
                    {
                        log.Error($"Unable to load assembly reference [{assemblyPath}]");
                    }

                    // NOTE: We should investigate so that this can also be done for Stride core assemblies (right now they use module initializers)
                    if (assembly != null)
                    {
                        // Register assembly in the registry
                        AssemblyRegistry.Register(assembly, AssemblyCommonCategories.Assets);
                    }
                }

                loadedAssembly.Assembly = assembly;
            }
            catch (Exception ex)
            {
                log.Error($"Unexpected error while loading assembly reference [{assemblyPath}].", ex);
            }
        }

        /// <summary>
        ///   In case <see cref="AssetItem.SourceFolder"/> was <c>null</c>, generates it.
        /// </summary>
        internal void UpdateSourceFolders(IReadOnlyCollection<AssetItem> assets)
        {
            // If there are not assets, we don't need to update or create an asset folder
            if (assets.Count == 0)
                return;

            // Use by default the first asset folders if not defined on the asset item
            var defaultFolder = AssetFolders.Count > 0 ? AssetFolders.First().Path : UDirectory.This;
            var assetFolders = new HashSet<UDirectory>(GetDistinctAssetFolderPaths());
            foreach (var asset in assets)
            {
                if (asset.Asset is IProjectAsset)
                {
                    if (asset.SourceFolder is null)
                    {
                        asset.SourceFolder = string.Empty;
                        asset.IsDirty = true;
                    }
                }
                else
                {
                    if (asset.SourceFolder is null)
                    {
                        asset.SourceFolder = defaultFolder.IsAbsolute ? defaultFolder.MakeRelative(RootDirectory) : defaultFolder;
                        asset.IsDirty = true;
                    }

                    var assetFolderAbsolute = UPath.Combine(RootDirectory, asset.SourceFolder);
                    if (!assetFolders.Contains(assetFolderAbsolute))
                    {
                        assetFolders.Add(assetFolderAbsolute);
                        AssetFolders.Add(new AssetFolder(assetFolderAbsolute));
                        IsDirty = true;
                    }
                }
            }
        }

        /// <summary>
        ///   Loads the templates.
        /// </summary>
        /// <param name="log">The <see cref="ILogger"/> in which to log the results of the operation.</param>
        private void LoadTemplates(ILogger log)
        {
            foreach (var templateDir in TemplateFolders)
            {
                foreach (var filePath in templateDir.Files)
                {
                    try
                    {
                        var file = new FileInfo(filePath);
                        if (!file.Exists)
                        {
                            log.Warning($"Template [{file}] does not exist.");
                            continue;
                        }

                        var templateDescription = YamlSerializer.Default.Load<TemplateDescription>(file.FullName);
                        templateDescription.FullPath = file.FullName;
                        Templates.Add(templateDescription);
                    }
                    catch (Exception ex)
                    {
                        log.Error($"Error while loading template from [{filePath}].", ex);
                    }
                }
            }
        }

        private List<UDirectory> GetDistinctAssetFolderPaths()
        {
            var existingAssetFolders = new List<UDirectory>();
            foreach (var folder in AssetFolders)
            {
                var folderPath = RootDirectory != null ? UPath.Combine(RootDirectory, folder.Path) : folder.Path;
                if (!existingAssetFolders.Contains(folderPath))
                {
                    existingAssetFolders.Add(folderPath);
                }
            }
            return existingAssetFolders;
        }

        public static List<PackageLoadingAssetFile> ListAssetFiles(Package package, bool listAssetsInMsBuild, bool listUnregisteredAssets)
        {
            // TODO: Check how to handle refresh correctly as a public API
            if (package.RootDirectory is null)
                throw new InvalidOperationException("Package RootDirectory is null");

            var listFiles = new List<PackageLoadingAssetFile>();

            if (!Directory.Exists(package.RootDirectory))
            {
                return listFiles;
            }

            // Iterate on each source folders
            foreach (var sourceFolder in package.GetDistinctAssetFolderPaths())
            {
                // Lookup all files
                foreach (var directory in FileUtility.EnumerateDirectories(sourceFolder, SearchDirection.Down))
                {
                    var files = directory.GetFiles();

                    foreach (var filePath in files)
                    {
                        // Don't load package via this method
                        if (filePath.FullName.EndsWith(PackageFileExtension))
                            continue;

                        // Make an absolute path from the root of this package
                        var fileUPath = new UFile(filePath.FullName);
                        if (fileUPath.GetFileExtension() == null)
                            continue;

                        // If this kind of file an asset file?
                        var ext = fileUPath.GetFileExtension();
                        // Adjust extensions for Stride rename
                        ext = ext.Replace(".xk", ".sd");

                        // Make sure to add default shaders in this case, since we don't have a csproj for them
                        if (AssetRegistry.IsProjectCodeGeneratorAssetFileExtension(ext) && (!(package.Container is SolutionProject) || package.IsSystem))
                        {
                            listFiles.Add(new PackageLoadingAssetFile(fileUPath, sourceFolder) { CachedFileSize = filePath.Length });
                            continue;
                        }

                        // Project source code assets follow the .csproj pipeline
                        var isAsset = listUnregisteredAssets
                            ? ext.StartsWith(".sd", StringComparison.InvariantCultureIgnoreCase)
                            : AssetRegistry.IsAssetFileExtension(ext);
                        if (!isAsset || AssetRegistry.IsProjectAssetFileExtension(ext))
                            continue;

                        listFiles.Add(new PackageLoadingAssetFile(fileUPath, sourceFolder) { CachedFileSize = filePath.Length });
                    }
                }
            }

            // Find also assets in the .csproj
            if (listAssetsInMsBuild)
            {
                FindAssetsInProject(listFiles, package);
            }

            // Adjust extensions for Stride rename
            foreach (var loadingAsset in listFiles)
            {
                var originalExt = loadingAsset.FilePath.GetFileExtension() ?? "";
                var ext = originalExt.Replace(".xk", ".sd");
                if (ext != originalExt)
                {
                    loadingAsset.FilePath = new UFile(loadingAsset.FilePath.FullPath.Replace(".xk", ".sd"));
                }
            }

            return listFiles;
        }

        public static List<(UFile FilePath, UFile Link)> FindAssetsInProject(string projectFullPath, out string nameSpace)
        {
            var realFullPath = new UFile(projectFullPath);
            var project = VSProjectHelper.LoadProject(realFullPath);
            var dir = new UDirectory(realFullPath.GetFullDirectory());

            nameSpace = project.GetPropertyValue("RootNamespace");
            if (nameSpace == string.Empty)
                nameSpace = null;

            var result = project.Items
                .Where(x => (x.ItemType == "Compile" || x.ItemType == "None") && string.IsNullOrEmpty(x.GetMetadataValue("AutoGen")))
                // Build full path for Include and Link
                .Select(x => (FilePath: UPath.Combine(dir, new UFile(x.EvaluatedInclude)), Link: x.HasMetadata("Link") ? UPath.Combine(dir, new UFile(x.GetMetadataValue("Link"))) : null))
                // For items outside project, let's pretend they are link
                .Select(x => (x.FilePath, Link: x.Link ?? (!dir.Contains(x.FilePath) ? x.FilePath.GetFileName() : null)))
                // Test both Stride and Xenko extensions
                .Where(x =>
                    AssetRegistry.IsProjectAssetFileExtension(x.FilePath.GetFileExtension()) ||
                    AssetRegistry.IsProjectAssetFileExtension(x.FilePath.GetFileExtension().Replace(".xk", ".sd")))
                // Avoid duplicates otherwise it might save a single file as separte file with renaming
                // Had issues with case such as Effect.sdsl being registered twice (with glob pattern) and being saved as Effect.sdsl and Effect (2).sdsl
                .Distinct()
                .ToList();

            project.ProjectCollection.UnloadAllProjects();
            project.ProjectCollection.Dispose();

            return result;
        }

        private static void FindAssetsInProject(ICollection<PackageLoadingAssetFile> list, Package package)
        {
            if (package.IsSystem)
                return;

            var project = package.Container as SolutionProject;
            if (project is null || project.FullPath is null)
                return;

            var projectAssets = FindAssetsInProject(project.FullPath, out string defaultNamespace);
            package.RootNamespace = defaultNamespace;
            var projectDirectory = new UDirectory(project.FullPath.GetFullDirectory());

            foreach (var projectAsset in projectAssets)
            {
                list.Add(new PackageLoadingAssetFile(projectAsset.FilePath, projectDirectory) { Link = projectAsset.Link });
            }
        }

        private class MovePackageInsideProject : AssetUpgraderBase
        {
            protected override void UpgradeAsset(AssetMigrationContext context, PackageVersion currentVersion, PackageVersion targetVersion, dynamic asset, PackageLoadingAssetFile assetFile, OverrideUpgraderHint overrideHint)
            {
                if (asset.Profiles != null)
                {
                    var profiles = asset.Profiles;

                    foreach (var profile in profiles)
                    {
                        if (profile.Platform == "Shared")
                        {
                            if (profile.ProjectReferences != null && profile.ProjectReferences.Count == 1)
                            {
                                var projectLocation = (UFile)(string)profile.ProjectReferences[0].Location;
                                assetFile.FilePath = UPath.Combine(assetFile.OriginalFilePath.GetFullDirectory(), (UFile)(projectLocation.GetFullPathWithoutExtension() + PackageFileExtension));
                                asset.Meta.Name = projectLocation.GetFileNameWithoutExtension();
                            }

                            if (profile.AssetFolders != null)
                            {
                                for (int i = 0; i < profile.AssetFolders.Count; ++i)
                                {
                                    var assetPath = UPath.Combine(assetFile.OriginalFilePath.GetFullDirectory(), (UDirectory)(string)profile.AssetFolders[i].Path);
                                    assetPath = assetPath.MakeRelative(assetFile.FilePath.GetFullDirectory());
                                    profile.AssetFolders[i].Path = (string)assetPath;
                                }
                            }

                            if (profile.ResourceFolders != null)
                            {
                                for (int i = 0; i < profile.ResourceFolders.Count; ++i)
                                {
                                    var resourcePath = UPath.Combine(assetFile.OriginalFilePath.GetFullDirectory(), (UDirectory)(string)profile.ResourceFolders[i]);
                                    resourcePath = resourcePath.MakeRelative(assetFile.FilePath.GetFullDirectory());
                                    profile.ResourceFolders[i] = (string)resourcePath;
                                }
                            }

                            asset.AssetFolders = profile.AssetFolders;
                            asset.ResourceFolders = profile.ResourceFolders;
                            asset.OutputGroupDirectories = profile.OutputGroupDirectories;
                        }
                    }

                    asset.Profiles = DynamicYamlEmpty.Default;
                }
            }
        }
    }
}
