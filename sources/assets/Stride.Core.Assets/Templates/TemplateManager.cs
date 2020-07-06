// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Stride.Core.Assets.Templates
{
    /// <summary>
    ///   Provides methods to register templates that can be used for the creation of new <see cref="Package"/>s and
    ///   projects (see <see cref="ProjectReference"/>).
    /// </summary>
    public static class TemplateManager
    {
        private static readonly object managerLock = new object();

        private static readonly PackageCollection ExtraPackages = new PackageCollection();
        private static readonly List<ITemplateGenerator> Generators = new List<ITemplateGenerator>();

        public static void RegisterPackage(Package package)
        {
            ExtraPackages.Add(package);
        }

        /// <summary>
        ///   Registers the specified template generator.
        /// </summary>
        /// <param name="generator">An <see cref="ITemplateGenerator"/> that can be used to create a package.</param>
        /// <exception cref="ArgumentNullException"><paramref name="generator"/> is a <c>null</c> reference.</exception>
        public static void RegisterGenerator(ITemplateGenerator generator)
        {
            if (generator is null)
                throw new ArgumentNullException(nameof(generator));

            lock (managerLock)
            {
                if (!Generators.Contains(generator))
                {
                    Generators.Add(generator);
                }
            }
        }

        /// <summary>
        ///   Unregisters the specified template generator.
        /// </summary>
        /// <param name="generator">The <see cref="ITemplateGenerator"/> to unregister.</param>
        /// <exception cref="ArgumentNullException"><paramref name="generator"/> is a <c>null</c> reference.</exception>
        public static void Unregister(ITemplateGenerator generator)
        {
            if (generator is null)
                throw new ArgumentNullException(nameof(generator));

            lock (managerLock)
            {
                Generators.Remove(generator);
            }
        }

        /// <summary>
        ///   Finds all the template descriptions.
        /// </summary>
        /// <param name="session">The session for which to find template descriptions.</param>
        /// <returns>An enumeration of all the registered template descriptions.</returns>
        public static IEnumerable<TemplateDescription> FindTemplates(PackageSession session = null)
        {
            var packages = session?.Packages
                           .Concat(ExtraPackages)
                           .Distinct(DistinctPackagePathComparer.Default)
                           ?? ExtraPackages;

            // TODO: This will not work if the same package has different versions
            return packages.SelectMany(package => package.Templates)
                           .OrderBy(template => template.Order)
                           .ThenBy(template => template.Name)
                           .ToList();
        }

        /// <summary>
        ///   Finds all the template descriptions that match the given scope.
        /// </summary>
        /// <param name="scope">The scope that describes the context for the templates.</param>
        /// <param name="session">The session for which to find template descriptions.</param>
        /// <returns>An enumeration of all the registered template descriptions that match the given scope.</returns>
        public static IEnumerable<TemplateDescription> FindTemplates(TemplateScope scope, PackageSession session = null)
        {
            return FindTemplates(session).Where(template => template.Scope == scope);
        }

        /// <summary>
        ///   Finds a template generator supporting the specified template description.
        /// </summary>
        /// <typeparam name="TParameters">The type of <see cref="TemplateGeneratorParameters"/> of a template generator.</typeparam>
        /// <param name="description">The description of a template generator.</param>
        /// <returns>A template generator supporting the specified description; or <c>null</c> if not found.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="description"/> is a <c>null</c> reference.</exception>
        public static ITemplateGenerator<TParameters> FindTemplateGenerator<TParameters>(TemplateDescription description)
            where TParameters : TemplateGeneratorParameters
        {
            if (description is null)
                throw new ArgumentNullException(nameof(description));

            lock (managerLock)
            {
                // From most recently registered to older
                for (int i = Generators.Count - 1; i >= 0; i--)
                {
                    if (Generators[i] is ITemplateGenerator<TParameters> generator &&
                        generator.IsSupportingTemplate(description))
                    {
                        return generator;
                    }
                }
            }

            return null;
        }
        /// <summary>
        ///   Finds a template generator supporting the specified template description
        /// </summary>
        /// <typeparam name="TParameters">The type of <see cref="TemplateGeneratorParameters"/> of a template generator.</typeparam>
        /// <param name="parameters">The parameters of a template generator.</param>
        /// <returns>A template generator supporting the specified parameters; or <c>null</c> if not found.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="parameters"/> is a <c>null</c> reference.</exception>
        public static ITemplateGenerator<TParameters> FindTemplateGenerator<TParameters>(TParameters parameters)
            where TParameters : TemplateGeneratorParameters
        {
            if (parameters is null)
                throw new ArgumentNullException(nameof(parameters));

            lock (managerLock)
            {
                // From most recently registered to older
                for (int i = Generators.Count - 1; i >= 0; i--)
                {
                    if (Generators[i] is ITemplateGenerator<TParameters> generator &&
                        generator.IsSupportingTemplate(parameters.Description))
                    {
                        return generator;
                    }
                }
            }

            return null;
        }

        #region Equality Comparer: DistinctPackagePathComparer

        private class DistinctPackagePathComparer : IEqualityComparer<Package>
        {
            private static DistinctPackagePathComparer defaultInstance;
            public static DistinctPackagePathComparer Default
            {
                get
                {
                    if (defaultInstance is null)
                        defaultInstance = new DistinctPackagePathComparer();
                    return defaultInstance;
                }
            }

            public bool Equals(Package x, Package y)
            {
                return x.FullPath == y.FullPath;
            }

            public int GetHashCode(Package obj)
            {
                return obj.FullPath.GetHashCode();
            }
        }

        #endregion
    }
}
