// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading;

using Stride.Core.IO;
using Stride.Core.Annotations;
using Stride.Core.Assets.Yaml;
using Stride.Core.Assets.Tracking;

namespace Stride.Core.Assets
{
    /// <summary>
    ///   An asset item part of a <see cref="Assets.Package"/> accessible through <see cref="Package.Assets"/>.
    /// </summary>
    [DataContract("AssetItem")]
    public sealed class AssetItem : IFileSynchronizable
    {
        private Asset asset;
        private UFile location;
        private HashSet<UFile> sourceFiles;

        private bool isDirty;

        /// <summary>
        ///   Default comparer that matches <see cref="AssetItem"/> based only on their <see cref="AssetId"/>.
        /// </summary>
        public static readonly IEqualityComparer<AssetItem> DefaultComparerById = new AssetItemComparerById();

        /// <summary>
        ///   Initializes a new instance of the <see cref="AssetItem" /> class.
        /// </summary>
        /// <param name="location">The location inside the package.</param>
        /// <param name="asset">The asset.</param>
        /// <exception cref="ArgumentNullException"><paramref name="location"/> is a <c>null</c> reference.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="asset"/> is a <c>null</c> reference.</exception>
        public AssetItem([NotNull] UFile location, [NotNull] Asset asset) : this(location, asset, null) { }

        /// <summary>
        ///   Initializes a new instance of the <see cref="AssetItem" /> class.
        /// </summary>
        /// <param name="location">The location inside the package.</param>
        /// <param name="asset">The asset.</param>
        /// <param name="package">The package.</param>
        /// <exception cref="ArgumentNullException"><paramref name="location"/> is a <c>null</c> reference.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="asset"/> is a <c>null</c> reference.</exception>
        internal AssetItem([NotNull] UFile location, [NotNull] Asset asset, Package package)
        {
            this.location = location ?? throw new ArgumentNullException(nameof(location));
            this.asset = asset ?? throw new ArgumentNullException(nameof(asset));

            Package = package;
            isDirty = true;
        }

        /// <summary>
        ///   Gets the location of this asset.
        /// </summary>
        /// <value>An <see cref="UFile"/> representing the location of this asset.</value>
        [NotNull]
        public UFile Location
        {
            get => location;
            internal set => location = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        ///   Gets or sets the real location of this asset if it is overriden (similar to <c>Link</c> in C# project files).
        /// </summary>
        [CanBeNull]
        public UFile AlternativePath { get; set; }

        /// <summary>
        ///   Gets or sets the directory where the assets will be stored on the disk relative to the <see cref="Package"/>.
        ///   The directory will update the list found in <see cref="Package.AssetFolders"/>.
        /// </summary>
        /// <value>The directory where the assets will be stored on the disk.</value>
        public UDirectory SourceFolder { get; set; }

        /// <summary>
        ///   Gets the unique identifier of this asset.
        /// </summary>
        /// <value>An <see cref="AssetId"/> that uniquely identifies this asset.</value>
        public AssetId Id => asset.Id;

        /// <summary>
        ///   Gets the <see cref="Assets.Package"/> where this asset is stored.
        /// </summary>
        /// <value>The package where this asset is stored.</value>
        public Package Package { get; internal set; }

        /// <summary>
        ///   Gets the attached metadata for YAML serialization.
        /// </summary>
        [DataMemberIgnore]
        public AttachedYamlAssetMetadata YamlMetadata { get; } = new AttachedYamlAssetMetadata();

        /// <summary>
        ///   Converts this asset to an <see cref="AssetReference"/>.
        /// </summary>
        /// <returns>A <see cref="AssetReference"/> representing a reference to this asset.</returns>
        [NotNull]
        public AssetReference ToReference() => new AssetReference(Id, Location);

        /// <summary>
        ///   Clones this instance without the attached package.
        /// </summary>
        /// <param name="newLocation">
        ///   The new location that will be used in the cloned <see cref="AssetItem"/>. If <c>null</c>, it keeps the original location of the asset.
        /// </param>
        /// <param name="newAsset">
        ///   The new asset that will be used in the cloned <see cref="AssetItem"/>. If <c>null</c>, it clones the original asset. otherwise, the
        ///   specified asset is used as-is in the new <see cref="AssetItem"/> (no clone on <paramref name="newAsset"/> is performed).
        /// </param>
        /// <param name="flags">Flags used to clone assets if needed.</param>
        /// <returns>A clone of this instance.</returns>
        [NotNull]
        public AssetItem Clone(UFile newLocation = null, Asset newAsset = null, AssetClonerFlags flags = AssetClonerFlags.None)
        {
            return Clone(false, newLocation, newAsset, flags);
        }

        /// <summary>
        ///   Clones this instance without the attached package.
        /// </summary>
        /// <param name="keepPackage">
        ///   Value indicating whether to copy package information. Only used by the <see cref="Analysis.AssetDependencyManager" />.
        /// </param>
        /// <param name="newLocation">
        ///   The new location that will be used in the cloned <see cref="AssetItem"/>. If <c>null</c>, it keeps the original location of the asset.
        /// </param>
        /// <param name="newAsset">
        ///   The new asset that will be used in the cloned <see cref="AssetItem"/>. If <c>null</c>, it clones the original asset. otherwise, the
        ///   specified asset is used as-is in the new <see cref="AssetItem"/> (no clone on <paramref name="newAsset"/> is performed).
        /// </param>
        /// <param name="flags">Flags used to clone assets if needed.</param>
        /// <returns>A clone of this instance.</returns>
        [NotNull]
        public AssetItem Clone(bool keepPackage, UFile newLocation = null, Asset newAsset = null, AssetClonerFlags flags = AssetClonerFlags.None)
        {
            // Set the package after the new AssetItem(), to make sure that isDirty is not going to call a notification on the package
            var item = new AssetItem(newLocation ?? location, newAsset ?? AssetCloner.Clone(Asset, flags), keepPackage ? Package : null)
            {
                isDirty = isDirty,
                SourceFolder = SourceFolder,
                version = Version,
                AlternativePath = AlternativePath,
            };
            YamlMetadata.CopyInto(item.YamlMetadata);
            return item;
        }

        /// <summary>
        ///   Gets the full absolute path of this asset on the disk, taking into account the <see cref="SourceFolder"/>, and the
        ///   <see cref="Package.RootDirectory"/>.
        /// </summary>
        /// <value>The full absolute path of this asset on disk.</value>
        /// <remarks>
        ///   This value is only valid if this instance is attached to a <see cref="Assets.Package"/>, and that package has a
        ///   non-<c>null</c> <see cref="Package.RootDirectory"/>.
        /// </remarks>
        [NotNull]
        public UFile FullPath
        {
            get
            {
                var localSourceFolder = SourceFolder ?? (Package != null
                    ? Package.GetDefaultAssetFolder()
                    : UDirectory.This );

                // Root directory of package
                var rootDirectory = Package != null && Package.RootDirectory != null ? Package.RootDirectory : null;

                // If the source folder is absolute, make it relative to the root directory
                if (localSourceFolder.IsAbsolute)
                {
                    if (rootDirectory != null)
                    {
                        localSourceFolder = localSourceFolder.MakeRelative(rootDirectory);
                    }
                }

                rootDirectory = rootDirectory != null ? UPath.Combine(rootDirectory, localSourceFolder) : localSourceFolder;

                var locationAndExtension = AlternativePath ?? new UFile(Location + AssetRegistry.GetDefaultExtension(Asset.GetType()));
                return rootDirectory != null ? UPath.Combine(rootDirectory, locationAndExtension) : locationAndExtension;
            }
        }

        /// <summary>
        ///   Gets or sets the asset.
        /// </summary>
        /// <value>The asset.</value>
        [NotNull]
        public Asset Asset
        {
            get => asset;
            internal set => asset = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        ///   Gets the last modified time of the asset.
        /// </summary>
        /// <value>The last modified time of the asset.</value>
        /// <remarks>
        ///   By default, contains the last modified time of the asset from the disk. If <see cref="IsDirty"/> is set to
        ///   <c>true</c>, this time will get current time of modification.
        /// </remarks>
        public DateTime ModifiedTime { get; internal set; }

        /// <summary>
        ///   Gets the asset version incremental counter, increased everytime the asset is edited.
        /// </summary>
        public long Version
        {
            get => Interlocked.Read(ref version);
            internal set => Interlocked.Exchange(ref version, value);
        }
        private long version;

        /// <summary>
        ///   Gets or sets a value indicating whether this instance has been modified.
        /// </summary>
        /// <value><c>true</c> if this instance has been modified; otherwise, <c>false</c>.</value>
        /// <remarks>
        ///   When an asset is modified, this property must be set to true in order to track assets changes.
        /// </remarks>
        public bool IsDirty
        {
            get => isDirty;
            set
            {
                if (value && !isDirty)
                {
                    ModifiedTime = DateTime.Now;
                }

                Interlocked.Increment(ref version);
                sourceFiles?.Clear();

                var oldValue = isDirty;
                isDirty = value;
                Package?.OnAssetDirtyChanged(this, oldValue, value);
            }
        }

        public bool IsDeleted { get; set; }

        public override string ToString() => $"[{Asset.GetType().Name}] {location}";

        /// <summary>
        ///   Creates a child <see cref="Assets.Asset"/> that is inheriting the values of this asset.
        /// </summary>
        /// <returns>A new asset inheriting the values of this asset.</returns>
        [NotNull]
        public Asset CreateDerivedAsset() => Asset.CreateDerivedAsset(Location, out _);

        /// <summary>
        ///   Finds the base <see cref="AssetItem"/> referenced by this item from the current session
        ///   (using the <see cref="Assets.Package"/> setup on this instance).
        /// </summary>
        /// <returns>The base <see cref="AssetItem"/>, or <c>null</c> if not found.</returns>
        [CanBeNull]
        public AssetItem FindBase()
        {
            if (Package?.Session is null || Asset.Archetype is null)
            {
                return null;
            }
            var session = Package.Session;
            return session.FindAsset(Asset.Archetype.Id);
        }

        /// <summary>
        ///   Crreates the <see cref="SourceFolder"/> in case it was <c>null</c>.
        /// </summary>
        public void UpdateSourceFolders()
        {
            Package.UpdateSourceFolders(new[] { this });
        }

        public ISet<UFile> RetrieveCompilationInputFiles()
        {
            if (sourceFiles is null)
            {
                var collector = new SourceFilesCollector();
                sourceFiles = collector.GetCompilationInputFiles(Asset);
            }

            return sourceFiles;
        }

        private class AssetItemComparerById : IEqualityComparer<AssetItem>
        {
            public bool Equals(AssetItem x, AssetItem y)
            {
                if (ReferenceEquals(x, y))
                    return true;

                if (x is null || y is null)
                    return false;

                if (ReferenceEquals(x.Asset, y.Asset))
                    return true;

                return x.Id == y.Id;
            }

            public int GetHashCode(AssetItem obj) => obj.Id.GetHashCode();
        }
    }
}
