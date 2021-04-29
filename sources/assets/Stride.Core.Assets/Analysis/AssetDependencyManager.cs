// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

using Stride.Core.Reflection;
using Stride.Core.Serialization;
using Stride.Core.Serialization.Contents;
using Stride.Core.Assets.Visitors;

namespace Stride.Core.Assets.Analysis
{
    /// <summary>
    ///   A class responsible for providing asset dependencies for a <see cref="PackageSession"/> and file tracking dependency.
    /// </summary>
    /// <remarks>
    ///   This class provides methods to:
    ///   <list type="bullet">
    ///     <item><description>Find assets referencing a particular asset (recursively or not).</description></item>
    ///     <item><description>Find assets referenced by a particular asset (recursively or not).</description></item>
    ///     <item><description>Find missing references.</description></item>
    ///     <item><description>Find missing references for a particular asset.</description></item>
    ///     <item><description>Find assets file changed events that have changed on the disk.</description></item>
    ///   </list>
    /// </remarks>
    public sealed class AssetDependencyManager : IAssetDependencyManager, IDisposable
    {
        internal readonly object ThisLock = new object();

        private readonly PackageSession session;
        internal readonly HashSet<Package> Packages;
        internal readonly Dictionary<AssetId, AssetDependencies> Dependencies;
        internal readonly Dictionary<AssetId, AssetDependencies> AssetsWithMissingReferences;
        internal readonly Dictionary<AssetId, HashSet<AssetDependencies>> MissingReferencesToParent;

        private bool isDisposed;
        private bool isSessionSaving;
        private bool isInitialized;

        /// <summary>
        ///   Gets a value indicating whether this instance is initialized.
        /// </summary>
        /// <value><c>true</c> if this instance is initialized; otherwise, <c>false</c>.</value>
        /// <remarks>
        ///   If this instance is not initialized, all public methods may block until the full initialization happens with <see cref="Initialize"/>.
        /// </remarks>
        public bool IsInitialized => isInitialized;

        /// <summary>
        ///   Occurs when an asset has changed. This event is called in the critical section of the dependency manager,
        ///   meaning that dependencies can be safely computed via <see cref="ComputeDependencies"/> method from this callback.
        /// </summary>
        public event DirtyFlagChangedDelegate<AssetItem> AssetChanged;

        /// <summary>
        ///   Initializes a new instance of the <see cref="AssetDependencyManager"/> class.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="session"/> is a <c>null</c> reference.</exception>
        internal AssetDependencyManager(PackageSession session)
        {
            this.session = session ?? throw new ArgumentNullException(nameof(session));

            this.session.Packages.CollectionChanged += Packages_CollectionChanged;
            session.AssetDirtyChanged += Session_AssetDirtyChanged;

            Packages = new HashSet<Package>();
            Dependencies = new Dictionary<AssetId, AssetDependencies>();
            AssetsWithMissingReferences = new Dictionary<AssetId, AssetDependencies>();
            MissingReferencesToParent = new Dictionary<AssetId, HashSet<AssetDependencies>>();

            // If the session has already a root package, then initialize the dependency manager directly
            if (session.LocalPackages.Any())
                Initialize();
        }

        /// <summary>
        ///   Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (isDisposed)
                return;

            isDisposed = true;
        }

        /// <inheritdoc />
        public AssetDependencies ComputeDependencies(AssetId assetId, AssetDependencySearchOptions dependenciesOptions = AssetDependencySearchOptions.All,
                                                     ContentLinkType linkTypes = ContentLinkType.Reference, HashSet<AssetId> visited = null)
        {
            bool recursive = dependenciesOptions.HasFlag(AssetDependencySearchOptions.Recursive);
            if (visited is null && recursive)
                visited = new HashSet<AssetId>();

            //var clock = Stopwatch.StartNew();

            lock (Initialize())
            {
                if (!Dependencies.TryGetValue(assetId, out AssetDependencies dependencies))
                    return null;

                dependencies = new AssetDependencies(dependencies.Item);

                int inCount = 0, outCount = 0;

                if (dependenciesOptions.HasFlag(AssetDependencySearchOptions.In))
                {
                    CollectInputReferences(dependencies, assetId, visited, recursive, linkTypes, ref inCount);
                }

                if (dependenciesOptions.HasFlag(AssetDependencySearchOptions.Out))
                {
                    visited?.Clear();
                    CollectOutputReferences(dependencies, assetId, visited, recursive, linkTypes, ref outCount);
                }

                //Console.WriteLine("Time to compute dependencies: {0}ms in: {1} out:{2}", clock.ElapsedMilliseconds, inCount, outCount);

                return dependencies;
            }
        }

        /// <summary>
        ///   Initializes this instance of <see cref="AssetDependencyManager"/> to analyze and track the packages of a session.
        /// </summary>
        /// <remarks>
        ///   Until this method is called to initialize the <see cref="AssetDependencyManager"/>, all public methods may block.
        /// </remarks>
        private object Initialize()
        {
            lock (ThisLock)
            {
                if (isInitialized)
                    return ThisLock;

                // If the package is cancelled, don't try to do anything
                // A cancellation means that the package session will be destroyed
                if (isDisposed)
                    return ThisLock;

                // Initialize with the list of packages
                foreach (var package in session.Packages)
                {
                    // If the package is cancelled, don't try to do anything
                    // A cancellation means that the package session will be destroyed
                    if (isDisposed)
                        return ThisLock;

                    TrackPackage(package);
                }

                isInitialized = true;
            }

            return ThisLock;
        }

        /// <summary>
        ///   Collects all references of an asset dynamically.
        /// </summary>
        /// <param name="result">The resulting <see cref="AssetDependencies"/>.</param>
        /// <param name="assetResolver">The asset resolver, a function to resolve an asset from its <see cref="AssetId"/>.</param>
        /// <param name="isRecursive">If set to <c>true</c>, collects references recursively.</param>
        /// <param name="keepParents">Indicates if the parent of the provided <paramref name="result"/> should be kept or not.</param>
        /// <exception cref="System.ArgumentNullException">
        ///   <paramref name="result"/> is a <c>null</c> reference,
        ///   or
        ///   <paramref name="assetResolver"/> is a <c>null</c> reference.
        /// </exception>
        private static void CollectDynamicOutReferences(AssetDependencies result, Func<AssetId, AssetItem> assetResolver, bool isRecursive, bool keepParents)
        {
            if (result is null)
                throw new ArgumentNullException(nameof(result));
            if (assetResolver is null)
                throw new ArgumentNullException(nameof(assetResolver));

            var addedReferences = new HashSet<AssetId>();
            var itemsToAnalyze = new Queue<AssetItem>();
            var referenceCollector = new DependenciesCollector();

            // Reset the dependencies / parts
            result.Reset(keepParents);

            var assetItem = result.Item;

            // Mark as processed to not add it again
            addedReferences.Add(assetItem.Id);
            itemsToAnalyze.Enqueue(assetItem);

            while (itemsToAnalyze.Count > 0)
            {
                var item = itemsToAnalyze.Dequeue();

                foreach (var link in referenceCollector.GetDependencies(item))
                {
                    if (addedReferences.Contains(link.Element.Id))
                        continue;

                    // Mark as processed to not add it again
                    addedReferences.Add(link.Element.Id);

                    // Add the location to the reference location list
                    var nextItem = assetResolver(link.Element.Id);
                    if (nextItem != null)
                    {
                        result.AddLinkOut(nextItem, link.Type);

                        // Add current element to analyze list, to analyze dependencies recursively
                        if (isRecursive)
                            itemsToAnalyze.Enqueue(nextItem);
                    }
                    else
                    {
                        result.AddBrokenLinkOut(link);
                    }
                }

                if (!isRecursive)
                    break;
            }
        }

        private AssetItem FindAssetFromDependencyOrSession(AssetId assetId)
        {
            // We cannot return the item from the session. We can only return assets currently tracked by the dependency manager
            var item = session.FindAsset(assetId);
            if (item != null)
            {
                var dependencies = TrackAsset(assetId);
                return dependencies.Item;
            }

            return null;
        }

        /// <summary>
        ///   This methods is called when a session is about to being saved.
        /// </summary>
        public void BeginSavingSession()
        {
            isSessionSaving = true;
        }

        /// <summary>
        ///   This methods is called when a session has been saved.
        /// </summary>
        public void EndSavingSession()
        {
            isSessionSaving = false;
        }

        /// <summary>
        ///   Calculate the dependencies for the specified asset either by using the internal cache if the asset is already in the session
        ///   or by computing its dependencies.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <returns><see cref="AssetDependencies"/> for the specified asset.</returns>
        private AssetDependencies CalculateDependencies(AssetId assetId)
        {
            Dependencies.TryGetValue(assetId, out AssetDependencies dependencies);
            return dependencies;
        }

        /// <summary>
        ///   This method is called when a <see cref="Package"/> needs to be tracked.
        /// </summary>
        /// <param name="package">The package to track.</param>
        private void TrackPackage(Package package)
        {
            lock (ThisLock)
            {
                if (Packages.Contains(package))
                    return;

                Packages.Add(package);

                foreach (var asset in package.Assets)
                {
                    // If the package is cancelled, don't try to do anything
                    // A cancellation means that the package session will be destroyed
                    if (isDisposed)
                        return;

                    TrackAsset(asset);
                }

                package.Assets.CollectionChanged += Assets_CollectionChanged;
            }
        }

        /// <summary>
        ///   This method is called when a <see cref="Package"/> does not need to be tracked anymore.
        /// </summary>
        /// <param name="package">The package to un-track.</param>
        private void UnTrackPackage(Package package)
        {
            lock (ThisLock)
            {
                if (!Packages.Contains(package))
                    return;

                package.Assets.CollectionChanged -= Assets_CollectionChanged;

                foreach (var asset in package.Assets)
                {
                    UnTrackAsset(asset);
                }

                Packages.Remove(package);
            }
        }

        /// <summary>
        ///   This method is called when a <see cref="AssetItem"/> needs to be tracked.
        /// </summary>
        /// <param name="assetItemSource">The asset item to track.</param>
        /// <returns><see cref="AssetDependencies"/> for the specified asset.</returns>
        private AssetDependencies TrackAsset(AssetItem assetItemSource)
        {
            return TrackAsset(assetItemSource.Id);
        }

        /// <summary>
        ///   This method is called when a <see cref="AssetId"/> needs to be tracked.
        /// </summary>
        /// <returns><see cref="AssetDependencies"/> for the specified asset.</returns>
        private AssetDependencies TrackAsset(AssetId assetId)
        {
            lock (ThisLock)
            {
                if (Dependencies.TryGetValue(assetId, out AssetDependencies dependencies))
                    return dependencies;

                // TODO: Provide an optimized version of TrackAsset method, taking directly a well known asset (loaded from a Package, etc.) to avoid session.FindAsset
                var assetItem = session.FindAsset(assetId);
                if (assetItem is null)
                    return null;

                // Clone the asset before using it in this instance to make sure that we have some kind of immutable state
                // TODO: This is not handling shadow registry

                // No need to clone assets from readonly package
                var assetItemCloned = assetItem.Package.IsSystem
                    ? assetItem
                    : new AssetItem(assetItem.Location, AssetCloner.Clone(assetItem.Asset), assetItem.Package)
                        {
                            SourceFolder = assetItem.SourceFolder,
                            AlternativePath = assetItem.AlternativePath,
                        };

                dependencies = new AssetDependencies(assetItemCloned);

                // Adds to global list
                Dependencies.Add(assetId, dependencies);

                // Update dependencies
                UpdateAssetDependencies(dependencies);
                CheckAllDependencies();

                return dependencies;
            }
        }

        private void CheckAllDependencies()
        {
            //foreach (var dependencies in Dependencies.Values)
            //{
            //    foreach (var outDependencies in dependencies)
            //    {
            //        if (outDependencies.Package == null)
            //        {
            //            System.Diagnostics.Debugger.Break();
            //        }
            //    }
            //}
        }

        /// <summary>
        ///   This method is called when an <see cref="AssetItem"/> does not need to be tracked anymore.
        /// </summary>
        /// <param name="assetItemSource">The asset item source.</param>
        private void UnTrackAsset(AssetItem assetItemSource)
        {
            lock (ThisLock)
            {
                var assetId = assetItemSource.Id;
                if (!Dependencies.TryGetValue(assetId, out AssetDependencies dependencies))
                    return;

                // Remove from global list
                Dependencies.Remove(assetId);

                // Remove previous missing dependencies
                RemoveMissingDependencies(dependencies);

                // Update [In] dependencies for children
                foreach (var childItem in dependencies.LinksOut)
                {
                    if (Dependencies.TryGetValue(childItem.Item.Id, out AssetDependencies childDependencyItem))
                    {
                        childDependencyItem.RemoveLinkIn(dependencies.Item);
                    }
                }

                // Update [Out] dependencies for parents
                foreach (var parentDependencies in dependencies.LinksIn)
                {
                    var assetDependencies = Dependencies[parentDependencies.Item.Id];
                    var linkOut = assetDependencies.RemoveLinkOut(dependencies.Item);
                    assetDependencies.AddBrokenLinkOut(linkOut);

                    UpdateMissingDependencies(assetDependencies);
                }
            }

            CheckAllDependencies();
        }

        private void UpdateAssetDependencies(AssetDependencies dependencies)
        {
            lock (ThisLock)
            {
                // Remove previous missing dependencies
                RemoveMissingDependencies(dependencies);

                // Remove [In] dependencies from previous children
                foreach (var referenceAsset in dependencies.LinksOut)
                {
                    var childDependencyItem = TrackAsset(referenceAsset.Item);
                    childDependencyItem?.RemoveLinkIn(dependencies.Item);
                }

                // Recalculate [Out] dependencies
                CollectDynamicOutReferences(dependencies, FindAssetFromDependencyOrSession, isRecursive: false, keepParents: true);

                // Add [In] dependencies to new children
                foreach (var assetLink in dependencies.LinksOut)
                {
                    var childDependencyItem = TrackAsset(assetLink.Item);
                    childDependencyItem?.AddLinkIn(dependencies.Item, assetLink.Type);
                }

                // Update missing dependencies
                UpdateMissingDependencies(dependencies);
            }
        }

        private void RemoveMissingDependencies(AssetDependencies dependencies)
        {
            if (AssetsWithMissingReferences.ContainsKey(dependencies.Item.Id))
            {
                AssetsWithMissingReferences.Remove(dependencies.Item.Id);
                foreach (var assetLink in dependencies.BrokenLinksOut)
                {
                    var list = MissingReferencesToParent[assetLink.Element.Id];
                    list.Remove(dependencies);
                    if (list.Count == 0)
                    {
                        MissingReferencesToParent.Remove(assetLink.Element.Id);
                    }
                }
            }
        }

        private void UpdateMissingDependencies(AssetDependencies dependencies)
        {
            HashSet<AssetDependencies> parentDependencyItems;

            // If the asset has any missing dependencies, update the fast lookup tables
            if (dependencies.HasMissingDependencies)
            {
                AssetsWithMissingReferences[dependencies.Item.Id] = dependencies;

                foreach (var assetLink in dependencies.BrokenLinksOut)
                {
                    if (!MissingReferencesToParent.TryGetValue(assetLink.Element.Id, out parentDependencyItems))
                    {
                        parentDependencyItems = new HashSet<AssetDependencies>();
                        MissingReferencesToParent.Add(assetLink.Element.Id, parentDependencyItems);
                    }

                    parentDependencyItems.Add(dependencies);
                }
            }

            var item = dependencies.Item;

            // If the new asset was a missing reference, remove all missing references for this asset
            if (MissingReferencesToParent.TryGetValue(item.Id, out parentDependencyItems))
            {
                MissingReferencesToParent.Remove(item.Id);
                foreach (var parentDependencies in parentDependencyItems)
                {
                    // Remove missing dependency from parent
                    var oldBrokenLink = parentDependencies.RemoveBrokenLinkOut(item.Id);

                    // Update [Out] dependency to parent
                    parentDependencies.AddLinkOut(item, oldBrokenLink.Type);

                    // Update [In] dependency to current
                    dependencies.AddLinkIn(parentDependencies.Item, oldBrokenLink.Type);

                    // Remove global cache for assets with missing references
                    if (!parentDependencies.HasMissingDependencies)
                    {
                        AssetsWithMissingReferences.Remove(parentDependencies.Item.Id);
                    }
                }
            }
        }

        private void Session_AssetDirtyChanged(AssetItem asset, bool oldValue, bool newValue)
        {
            // Don't update the dependency manager while saving (setting dirty flag to false)
            if (!isSessionSaving)
            {
                lock (ThisLock)
                {
                    if (Dependencies.TryGetValue(asset.Id, out AssetDependencies dependencies))
                    {
                        dependencies.Item.Asset = AssetCloner.Clone(asset.Asset);
                        dependencies.Item.Version = asset.Version;
                        UpdateAssetDependencies(dependencies);

                        // Notify an asset changed
                        OnAssetChanged(dependencies.Item, oldValue, newValue);
                    }
                }

                CheckAllDependencies();
            }
        }

        private void Packages_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    TrackPackage((Package)e.NewItems[0]);
                    break;

                case NotifyCollectionChangedAction.Remove:
                    UnTrackPackage((Package)e.OldItems[0]);
                    break;

                case NotifyCollectionChangedAction.Replace:
                    foreach (var oldPackage in e.OldItems.OfType<Package>())
                        UnTrackPackage(oldPackage);
                    foreach (var packageToCopy in session.Packages)
                        TrackPackage(packageToCopy);
                    break;
            }
        }

        private void Assets_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    TrackAsset(((AssetItem)e.NewItems[0]));
                    break;

                case NotifyCollectionChangedAction.Remove:
                    UnTrackAsset(((AssetItem)e.OldItems[0]));
                    break;

                case NotifyCollectionChangedAction.Reset:
                    var collection = (PackageAssetCollection) sender;

                    var items = Dependencies.Values.Where(item => ReferenceEquals(item.Item.Package, collection.Package)).ToList();
                    foreach (var assetItem in items)
                        UnTrackAsset(assetItem.Item);
                    foreach (var assetItem in collection)
                        TrackAsset(assetItem);
                    break;
            }
        }

        private void CollectInputReferences(AssetDependencies dependencyRoot, AssetId assetId, HashSet<AssetId> visited, bool recursive, ContentLinkType linkTypes, ref int count)
        {
            if (visited != null)
            {
                if (visited.Contains(assetId))
                    return;

                visited.Add(assetId);
            }

            count++;

            Dependencies.TryGetValue(assetId, out AssetDependencies dependencies);
            if (dependencies != null)
            {
                foreach (var pair in dependencies.LinksIn)
                {
                    if (linkTypes.HasFlag(pair.Type))
                    {
                        dependencyRoot.AddLinkIn(pair);

                        if (visited != null && recursive)
                        {
                            CollectInputReferences(dependencyRoot, pair.Item.Id, visited, true, linkTypes, ref count);
                        }
                    }
                }
            }
        }

        private void CollectOutputReferences(AssetDependencies dependencyRoot, AssetId assetId, HashSet<AssetId> visited, bool recursive, ContentLinkType linkTypes, ref int count)
        {
            if (visited != null)
            {
                if (visited.Contains(assetId))
                    return;

                visited.Add(assetId);
            }

            count++;

            var dependencies = CalculateDependencies(assetId);
            if (dependencies is null)
                return;

            // Add missing references
            foreach (var missingRef in dependencies.BrokenLinksOut)
            {
                dependencyRoot.AddBrokenLinkOut(missingRef);
            }

            // Add output references
            foreach (var child in dependencies.LinksOut)
            {
                if (linkTypes.HasFlag(child.Type))
                {
                    dependencyRoot.AddLinkOut(child);

                    if (visited != null && recursive)
                    {
                        CollectOutputReferences(dependencyRoot, child.Item.Id, visited, true, linkTypes, ref count);
                    }
                }
            }
        }

        /// <summary>
        ///   An interface providing methods to collect of asset references from an <see cref="AssetItem"/>.
        /// </summary>
        private interface IDependenciesCollector
        {
            /// <summary>
            ///   Get the asset references of an <see cref="AssetItem"/>. This function is not recursive.
            /// </summary>
            /// <param name="item">The asset whose references we want to get.</param>
            /// <returns>A collection of <see cref="IContentLink"/> that represents the references to the specified asset.</returns>
            IEnumerable<IContentLink> GetDependencies(AssetItem item);
        }

        private void OnAssetChanged(AssetItem asset, bool oldValue, bool newValue)
        {
            // Make sure we clone the item here only if it is necessary
            // Cloning the AssetItem is mandatory in order to make sure the asset item won't change
            AssetChanged?.Invoke(asset.Clone(keepPackage: true), oldValue, newValue);
        }

        /// <summary>
        ///   Visitor that collect all asset references.
        /// </summary>
        private class DependenciesCollector : AssetVisitorBase, IDependenciesCollector
        {
            private AssetDependencies dependencies;

            public IEnumerable<IContentLink> GetDependencies(AssetItem item)
            {
                dependencies = new AssetDependencies(item);
                Visit(item.Asset);

                return dependencies.BrokenLinksOut;
            }

            public override void VisitObject(object obj, ObjectDescriptor descriptor, bool visitMembers)
            {
                // References and base
                var reference = obj as IReference;
                if (reference is null)
                {
                    var attachedReference = AttachedReferenceManager.GetAttachedReference(obj);
                    if (attachedReference != null && attachedReference.IsProxy)
                        reference = attachedReference;
                }

                if (reference != null)
                {
                    dependencies.AddBrokenLinkOut(reference, ContentLinkType.Reference);
                }
                else
                {
                    base.VisitObject(obj, descriptor, visitMembers);
                }
            }

            public override void VisitObjectMember(object container, ObjectDescriptor containerDescriptor, IMemberDescriptor member, object value)
            {
                if (typeof(Asset).IsAssignableFrom(member.DeclaringType) &&
                    member.Name == nameof(Asset.Archetype) &&
                    value != null)
                {
                    dependencies.AddBrokenLinkOut((AssetReference)value, ContentLinkType.Reference);
                    return;
                }

                base.VisitObjectMember(container, containerDescriptor, member, value);
            }
        }
    }
}
