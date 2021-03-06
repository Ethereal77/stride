// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Stride.Core.Diagnostics;
using Stride.Core.IO;
using Stride.Core.Serialization;
using Stride.Core.Serialization.Contents;
using Stride.Core.Storage;

namespace Stride.Core.Assets.CompilerApp
{
    /// <summary>
    ///   Represents an object that can generate package bundles.
    /// </summary>
    internal class BundlePacker
    {
        /// <summary>
        ///   Builds the bundles for a session. It will automatically analyze assets and chunks to determine dependencies and what
        ///   should be embedded in which bundle.
        ///   Bundle descriptions will be loaded from <see cref="Package.Bundles" /> provided by the <see cref="packageSession"/>, and
        ///   copied to the <see cref="outputDirectory" />.
        /// </summary>
        /// <param name="logger">The builder logger.</param>
        /// <param name="packageSession">The project session.</param>
        /// <param name="rootPackage">The root package.</param>
        /// <param name="indexName">Name of the index file.</param>
        /// <param name="outputDirectory">The output directory.</param>
        /// <param name="disableCompressionIds">
        ///   The set of <see cref="ObjectId"/> that should be kept uncompressed in the bundle. Everything else will be compressed using LZ4).
        /// </param>
        /// <param name="useIncrementalBundles">Specifies if incremental bundles should be used, or writing a complete new one.</param>
        /// <exception cref="ArgumentNullException"><paramref name="logger"/> is a <c>null</c> reference.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="packageSession"/> is a <c>null</c> reference.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="indexName"/> is a <c>null</c> reference.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="outputDirectory"/> is a <c>null</c> reference.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="disableCompressionIds"/> is a <c>null</c> reference.</exception>
        /// <exception cref="InvalidOperationException">Can't exist two bundles with the same name.</exception>
        /// <exception cref="InvalidOperationException">Could not find a dependency when processing a bundle.</exception>
        public void Build(Logger logger, PackageSession packageSession, Package rootPackage, string indexName, string outputDirectory, ISet<ObjectId> disableCompressionIds, bool useIncrementalBundles, List<string> bundleFiles)
        {
            if (logger is null)
                throw new ArgumentNullException(nameof(logger));
            if (packageSession is null)
                throw new ArgumentNullException(nameof(packageSession));
            if (indexName is null)
                throw new ArgumentNullException(nameof(indexName));
            if (outputDirectory is null)
                throw new ArgumentNullException(nameof(outputDirectory));
            if (disableCompressionIds is null)
                throw new ArgumentNullException(nameof(disableCompressionIds));

            // Load index maps and mount databases
            using (var objDatabase = new ObjectDatabase(VirtualFileSystem.ApplicationDatabasePath, indexName, null, false))
            {
                logger.Info("Generating bundles: Scanning Assets and their dependencies...");

                // Prepare list of bundles gathered from all projects
                var bundles = new List<Bundle>();

                foreach (var project in packageSession.Packages)
                {
                    bundles.AddRange(project.Bundles);
                }

                var databaseFileProvider = new DatabaseFileProvider(objDatabase.ContentIndexMap, objDatabase);

                // Pass 1: Create ResolvedBundle from user Bundle
                var resolvedBundles = new Dictionary<string, ResolvedBundle>();
                foreach (var bundle in bundles)
                {
                    if (resolvedBundles.ContainsKey(bundle.Name))
                        throw new InvalidOperationException($"Two bundles are using the same name {bundle.Name}.");

                    resolvedBundles.Add(bundle.Name, new ResolvedBundle(bundle));
                }

                // Pass 2: Enumerate all Assets which directly or indirectly belong to an bundle
                var bundleAssets = new HashSet<string>();

                foreach (var bundle in resolvedBundles)
                {
                    // For each project, we apply asset selectors of current bundle
                    // This will give us a list of "root assets".
                    foreach (var assetSelector in bundle.Value.Source.Selectors)
                    {
                        foreach (var assetLocation in assetSelector.Select(packageSession, objDatabase.ContentIndexMap))
                        {
                            bundle.Value.AssetUrls.Add(assetLocation);
                        }
                    }

                    // Compute asset dependencies, and fill bundleAssets with list of all assets contained in bundles (directly or indirectly).
                    foreach (var assetUrl in bundle.Value.AssetUrls)
                    {
                        CollectReferences(databaseFileProvider, bundle.Value.Source, bundleAssets, assetUrl);
                    }
                }

                // Pass 3: Create a default bundle that contains all assets not contained in any bundle (directly or indirectly)
                var defaultBundle = new Bundle { Name = "default" };
                var resolvedDefaultBundle = new ResolvedBundle(defaultBundle);
                bundles.Add(defaultBundle);
                resolvedBundles.Add(defaultBundle.Name, resolvedDefaultBundle);
                foreach (var asset in objDatabase.ContentIndexMap.GetMergedIdMap())
                {
                    if (!bundleAssets.Contains(asset.Key))
                    {
                        resolvedDefaultBundle.AssetUrls.Add(asset.Key);
                    }
                }

                // Pass 4: Resolve dependencies
                foreach (var bundle in resolvedBundles)
                {
                    // Every bundle depends implicitely on default bundle
                    if (bundle.Key != "default")
                    {
                        bundle.Value.Dependencies.Add(resolvedBundles["default"]);
                    }

                    // Add other explicit dependencies
                    foreach (var dependencyName in bundle.Value.Source.Dependencies)
                    {
                        if (!resolvedBundles.TryGetValue(dependencyName, out ResolvedBundle dependency))
                            throw new InvalidOperationException($"Could not find dependency {dependencyName} when processing bundle {bundle.Value.Name}.");

                        bundle.Value.Dependencies.Add(dependency);
                    }
                }

                logger.Info("Generating bundles: Assigning Assets to bundles...");

                // Pass 5: Topological sort (a.k.a. build order)
                // If there is a cyclic dependency, an exception will be thrown.
                var sortedBundles = TopologicalSort(resolvedBundles.Values, assetBundle => assetBundle.Dependencies);

                // Pass 6: Find which ObjectId belongs to which bundle
                foreach (var bundle in sortedBundles)
                {
                    // Add objects created by dependencies
                    foreach (var dep in bundle.Dependencies)
                    {
                        // ObjectIds
                        bundle.DependencyObjectIds.UnionWith(dep.DependencyObjectIds);
                        bundle.DependencyObjectIds.UnionWith(dep.ObjectIds);

                        // IndexMap
                        foreach (var asset in dep.DependencyIndexMap.Concat(dep.IndexMap))
                        {
                            if (!bundle.DependencyIndexMap.ContainsKey(asset.Key))
                                bundle.DependencyIndexMap.Add(asset.Key, asset.Value);
                        }
                    }

                    // Collect assets (object ids and partial index map) from given asset urls
                    // Those not present in dependencies will be added to this bundle
                    foreach (var assetUrl in bundle.AssetUrls)
                    {
                        CollectBundle(databaseFileProvider, bundle, assetUrl);
                    }
                }

                logger.Info("Generating bundles: Compressing and saving bundles to disk...");

                var vfsToDisposeList = new List<IVirtualFileProvider>();
                // Mount VFS for output database (currently disabled because already done in ProjectBuilder.CopyBuildToOutput)
                using (var provider = VirtualFileSystem.MountFileSystem("/data_output", outputDirectory))
                {
                    VirtualFileSystem.CreateDirectory("/data_output/db");

                    // Mount output database and delete previous bundles that shouldn't exist anymore (others should be overwritten)
                    using (var outputDatabase = new ObjectDatabase("/data_output/db", VirtualFileSystem.ApplicationDatabaseIndexName, loadDefaultBundle: false))
                    {
                        try
                        {
                            outputDatabase.LoadBundle("default").GetAwaiter().GetResult();
                        }
                        catch (Exception)
                        {
                            logger.Info("Generating bundles: Tried to load previous 'default' bundle but it was invalid. Deleting it...");
                            outputDatabase.BundleBackend.DeleteBundles(x => Path.GetFileNameWithoutExtension(x) == "default");
                        }
                        var outputBundleBackend = outputDatabase.BundleBackend;

                        var outputGroupBundleBackends = new Dictionary<string, BundleOdbBackend>();

                        if (rootPackage.OutputGroupDirectories != null)
                        {
                            foreach (var item in rootPackage.OutputGroupDirectories)
                            {
                                var path = Path.Combine(rootPackage.RootDirectory, item.Value);
                                var vfsPath = "/data_group_" + item.Key;
                                var vfsDatabasePath = vfsPath + "/db";

                                // Mount VFS for output database (currently disabled because already done in ProjectBuilder.CopyBuildToOutput)
                                vfsToDisposeList.Add(VirtualFileSystem.MountFileSystem(vfsPath, path));
                                VirtualFileSystem.CreateDirectory(vfsDatabasePath);

                                outputGroupBundleBackends.Add(item.Key, new BundleOdbBackend(vfsDatabasePath));
                            }
                        }

                        // Pass 7: Assign bundle backends
                        foreach (var bundle in sortedBundles)
                        {
                            BundleOdbBackend bundleBackend;
                            if (bundle.Source.OutputGroup is null)
                            {
                                // No output group, use OutputDirectory
                                bundleBackend = outputBundleBackend;
                            }
                            else if (!outputGroupBundleBackends.TryGetValue(bundle.Source.OutputGroup, out bundleBackend))
                            {
                                // Output group not found in OutputGroupDirectories, let's issue a warning and fallback to OutputDirectory
                                logger.Warning($"Generate bundles: Could not find OutputGroup {bundle.Source.OutputGroup} for bundle {bundle.Name} in ProjectBuildProfile.OutputGroupDirectories");
                                bundleBackend = outputBundleBackend;
                            }

                            bundle.BundleBackend = bundleBackend;
                        }

                        CleanUnknownBundles(outputBundleBackend, resolvedBundles);

                        foreach (var bundleBackend in outputGroupBundleBackends)
                        {
                            CleanUnknownBundles(bundleBackend.Value, resolvedBundles);
                        }

                        // Pass 8: Pack actual data
                        foreach (var bundle in sortedBundles)
                        {
                            // Compute dependencies (by bundle names)
                            var dependencies = bundle.Dependencies.Select(x => x.Name).Distinct().ToList();

                            BundleOdbBackend bundleBackend;
                            if (bundle.Source.OutputGroup is null)
                            {
                                // No output group, use OutputDirectory
                                bundleBackend = outputBundleBackend;
                            }
                            else if (!outputGroupBundleBackends.TryGetValue(bundle.Source.OutputGroup, out bundleBackend))
                            {
                                // Output group not found in OutputGroupDirectories, let's issue a warning and fallback to OutputDirectory
                                logger.Warning($"Generating bundles: Could not find OutputGroup {bundle.Source.OutputGroup} for bundle {bundle.Name} in ProjectBuildProfile.OutputGroupDirectories.");
                                bundleBackend = outputBundleBackend;
                            }

                            var topBundleUrl = objDatabase.CreateBundle(bundle.ObjectIds.ToArray(), bundle.Name, bundleBackend, disableCompressionIds, bundle.IndexMap, dependencies, useIncrementalBundles);
                            // Expand list of incremental bundles
                            BundleOdbBackend.ReadBundleHeader(topBundleUrl, out var bundleUrls);
                            foreach (var bundleUrl in bundleUrls)
                            {
                                bundleFiles.Add(VirtualFileSystem.GetAbsolutePath(bundleUrl));
                            }
                        }

                        // Dispose VFS created for groups
                        foreach (var vfsToDispose in vfsToDisposeList)
                        {
                            try
                            {
                                vfsToDispose.Dispose();
                            }
                            catch (Exception ex)
                            {
                                logger.Error($"Unable to dispose VFS [{vfsToDispose.RootPath}]", ex);
                            }
                        }
                    }
                }
            }

            logger.Info("Generating bundles: Done");
        }

        private static void CleanUnknownBundles(BundleOdbBackend outputBundleBackend, Dictionary<string, ResolvedBundle> resolvedBundles)
        {
            // Delete previous bundles
            outputBundleBackend.DeleteBundles(bundleFile =>
            {
                // Ensure we have proper extension, otherwise delete
                if (Path.GetExtension(bundleFile) != BundleOdbBackend.BundleExtension)
                    return true;

                // Only keep bundles that are supposed to be output with same BundleBackend
                var bundleName = Path.GetFileNameWithoutExtension(bundleFile);
                return !resolvedBundles.TryGetValue(Path.GetFileNameWithoutExtension(bundleFile), out ResolvedBundle bundle) ||
                       bundle.BundleBackend != outputBundleBackend;
            });
        }

        private readonly Dictionary<ObjectId, List<string>> referencesByObjectId = new Dictionary<ObjectId, List<string>>();

        /// <summary>
        ///   Gets and caches the Asset URL referenced by the chunk with the given identifier.
        /// </summary>
        /// <param name="objectId">The object identifier.</param>
        /// <returns>The list of Asset URLs referenced.</returns>
        private List<string> GetChunkReferences(DatabaseFileProvider databaseFileProvider, ref ObjectId objectId)
        {
            // Check the cache
            if (!referencesByObjectId.TryGetValue(objectId, out List<string> references))
            {
                // First time, need to scan it
                referencesByObjectId[objectId] = references = new List<string>();

                // Open stream to read list of chunk references
                using (var stream = databaseFileProvider.OpenStream(DatabaseFileProvider.ObjectIdUrl + objectId, VirtualFileMode.Open, VirtualFileAccess.Read))
                {
                    // Read chunk header
                    var streamReader = new BinarySerializationReader(stream);
                    var header = ChunkHeader.Read(streamReader);

                    // Only process chunks
                    if (header != null)
                    {
                        if (header.OffsetToReferences != -1)
                        {
                            // Seek to where references are stored and deserialize them
                            streamReader.NativeStream.Seek(header.OffsetToReferences, SeekOrigin.Begin);

                            List<ChunkReference> chunkReferences = null;
                            streamReader.Serialize(ref chunkReferences, ArchiveMode.Deserialize);

                            foreach (var chunkReference in chunkReferences)
                            {
                                references.Add(chunkReference.Location);
                            }
                        }
                    }
                }
            }

            return references;
        }

        private void CollectReferences(DatabaseFileProvider databaseFileProvider, Bundle bundle, HashSet<string> assets, string assetUrl)
        {
            // Already included?
            if (!assets.Add(assetUrl))
                return;

            if (!databaseFileProvider.ContentIndexMap.TryGetValue(assetUrl, out ObjectId objectId))
                throw new InvalidOperationException($"Could not find Asset {assetUrl} for bundle {bundle.Name}.");

            // Include references
            foreach (var reference in GetChunkReferences(databaseFileProvider, ref objectId))
            {
                CollectReferences(databaseFileProvider, bundle, assets, reference);
            }
        }

        private void CollectBundle(DatabaseFileProvider databaseFileProvider, ResolvedBundle resolvedBundle, string assetUrl)
        {
            // Check if index map contains it already (that also means object id has been stored as well)
            if (resolvedBundle.DependencyIndexMap.ContainsKey(assetUrl) || resolvedBundle.IndexMap.ContainsKey(assetUrl))
                return;

            if (!databaseFileProvider.ContentIndexMap.TryGetValue(assetUrl, out ObjectId objectId))
                throw new InvalidOperationException($"Could not find Asset {assetUrl} for bundle {resolvedBundle.Name}.");

            // Add asset to index map
            resolvedBundle.IndexMap.Add(assetUrl, objectId);

            // Check if object id has already been added (either as dependency or inside this actual pack)
            // As a consequence, no need to check references since they will somehow be part of this package or one of its dependencies.
            if (resolvedBundle.DependencyObjectIds.Contains(objectId) ||
                !resolvedBundle.ObjectIds.Add(objectId))
                return;

            foreach (var reference in GetChunkReferences(databaseFileProvider, ref objectId))
            {
                CollectBundle(databaseFileProvider, resolvedBundle, reference);
            }
        }

        /// <summary>
        ///   Performs a topological sort.
        /// </summary>
        /// <typeparam name="T">The type of item.</typeparam>
        /// <param name="source">The source items.</param>
        /// <param name="dependencies">The function that will give dependencies for a given item.</param>
        /// <returns>The linearized topological sorted elements of the implicit tree.</returns>
        private static List<T> TopologicalSort<T>(IEnumerable<T> source, Func<T, IEnumerable<T>> dependencies)
        {
            var result = new List<T>();
            var temporaryMark = new HashSet<T>();
            var mark = new HashSet<T>();

            foreach (var item in source)
                TopologicalSortVisit(item, temporaryMark, mark, result, dependencies);

            return result;
        }

        private static void TopologicalSortVisit<T>(T item, HashSet<T> temporaryMark, HashSet<T> mark, List<T> result, Func<T, IEnumerable<T>> dependencies)
        {
            // Already processed?
            if (mark.Contains(item))
                return;

            if (temporaryMark.Contains(item))
                throw new InvalidOperationException($"Cyclic dependency found involving {item}.");

            temporaryMark.Add(item);

            foreach (var dep in dependencies(item))
                TopologicalSortVisit(dep, temporaryMark, mark, result, dependencies);

            mark.Add(item);
            result.Add(item);
        }
    }
}
