// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reflection;

using Stride.Core.Annotations;
using Stride.Core.Diagnostics;
using Stride.Core.Serialization;

namespace Stride.Core.Reflection
{
    /// <summary>
    ///   Provides basic infrastructure to associate categories to an assembly and to query and register on new
    ///   registered assembly event.
    /// </summary>
    public static class AssemblyRegistry
    {
        private static readonly Lazy<Logger> Log = new Lazy<Logger>(() => GlobalLogger.GetLogger("AssemblyRegistry"));

        private static readonly object Lock = new object();

        private static readonly Dictionary<string, HashSet<Assembly>> MapCategoryToAssemblies = new Dictionary<string, HashSet<Assembly>>();
        private static readonly Dictionary<Assembly, HashSet<string>> MapAssemblyToCategories = new Dictionary<Assembly, HashSet<string>>();

        private static readonly Dictionary<Assembly, ScanTypes> AssemblyToScanTypes = new Dictionary<Assembly, ScanTypes>();
        private static readonly Dictionary<string, Assembly> AssemblyNameToAssembly = new Dictionary<string, Assembly>(StringComparer.OrdinalIgnoreCase);

        static AssemblyRegistry()
        {
            Register(typeof(AssemblyRegistry).Assembly, "core"); // To be included in FindAll()
        }

        /// <summary>
        ///   Occurs when an assembly is registered.
        /// </summary>
        public static event EventHandler<AssemblyRegisteredEventArgs> AssemblyRegistered;

        /// <summary>
        ///   Occurs when an assembly is unregistered.
        /// </summary>
        public static event EventHandler<AssemblyRegisteredEventArgs> AssemblyUnregistered;

        /// <summary>
        ///   Finds all registered assemblies.
        /// </summary>
        /// <returns>A set of all assembly registered.</returns>
        [NotNull]
        public static HashSet<Assembly> FindAll()
        {
            lock (Lock)
            {
                return new HashSet<Assembly>(MapAssemblyToCategories.Keys);
            }
        }

        /// <summary>
        ///   Gets a type from its <see cref="DataContractAttribute.Alias"/> or <see cref="DataAliasAttribute.Name"/>.
        /// </summary>
        /// <param name="alias">The alias of the type.</param>
        /// <returns>The type with the specified alias; or <c>null</c> if not found.</returns>
        [CanBeNull]
        public static Type GetTypeFromAlias([NotNull] string alias)
        {
            // TODO: At some point we might want to reorganize AssemblyRegistry and DataSerializerFactory
            // I am not sure the list of assemblies matches between those two (some assemblies are probably not registered in AssemblyRegistry),
            // so the semantic of GetTypeFromAlias (which include all assemblies) might be different than GetType.
            return DataSerializerFactory.GetTypeFromAlias(alias);
        }

        /// <summary>
        ///   Gets a type by its typename already loaded in the assembly registry.
        /// </summary>
        /// <param name="fullyQualifiedTypeName">The type name, fully qualified (including namespaces).</param>
        /// <param name="throwOnError">A value indicating whether to throw an exception in case the type can't be resolved.</param>
        /// <returns>The queried type; or <c>null</c> if not found.</returns>
        /// <seealso cref="Type.GetType(string,bool)"/>
        /// <seealso cref="Assembly.GetType(string,bool)"/>
        public static Type GetType([NotNull] string fullyQualifiedTypeName, bool throwOnError = true)
        {
            if (fullyQualifiedTypeName is null)
                throw new ArgumentNullException(nameof(fullyQualifiedTypeName));

            var assemblyIndex = fullyQualifiedTypeName.IndexOf(",");
            if (assemblyIndex < 0)
                throw new ArgumentException($"Invalid fulltype name [{fullyQualifiedTypeName}], expecting an assembly name.", nameof(fullyQualifiedTypeName));

            var typeName = fullyQualifiedTypeName.Substring(0, assemblyIndex);
            var assemblyName = new AssemblyName(fullyQualifiedTypeName.Substring(assemblyIndex + 1));
            lock (Lock)
            {
                if (AssemblyNameToAssembly.TryGetValue(assemblyName.Name, out Assembly assembly))
                {
                    return assembly.GetType(typeName, throwOnError, false);
                }
            }

            // Fallback to default lookup
            return Type.GetType(fullyQualifiedTypeName, throwOnError, ignoreCase: false);
        }

        /// <summary>
        ///   Finds all the registered assemblies that are associated with the specified categories.
        /// </summary>
        /// <param name="categories">The categories.</param>
        /// <returns>A set of assemblies associated with the specified categories.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="categories"/> is a <c>null</c> reference.</exception>
        [NotNull]
        public static HashSet<Assembly> Find([NotNull] IEnumerable<string> categories)
        {
            if (categories is null)
                throw new ArgumentNullException(nameof(categories));

            var assemblies = new HashSet<Assembly>();
            lock (Lock)
            {
                foreach (var category in categories)
                {
                    if (category is null)
                        continue;

                    if (MapCategoryToAssemblies.TryGetValue(category, out HashSet<Assembly> assembliesFound))
                    {
                        foreach (var assembly in assembliesFound)
                            assemblies.Add(assembly);
                    }
                }
            }
            return assemblies;
        }

        /// <summary>
        ///   Finds all the registered assemblies that are associated with the specified categories.
        /// </summary>
        /// <param name="categories">The categories.</param>
        /// <returns>A set of assemblies associated with the specified categories.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="categories"/> is a <c>null</c> reference.</exception>
        [NotNull]
        public static HashSet<Assembly> Find([NotNull] params string[] categories)
        {
            return Find((IEnumerable<string>)categories);
        }

        /// <summary>
        ///   Finds the categories that are associated with the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>A set of categories associated with the specified assembly.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="assembly"/> is a <c>null</c> reference.</exception>
        [NotNull]
        public static HashSet<string> FindCategories([NotNull] Assembly assembly)
        {
            if (assembly is null)
                throw new ArgumentNullException(nameof(assembly));

            var categories = new HashSet<string>();
            lock (Lock)
            {
                if (MapAssemblyToCategories.TryGetValue(assembly, out HashSet<string> categoriesFound))
                {
                    foreach (var category in categoriesFound)
                        categories.Add(category);
                }
            }
            return categories;
        }

        public static void RegisterScanTypes([NotNull] Assembly assembly, ScanTypes types)
        {
            if (!AssemblyToScanTypes.ContainsKey(assembly))
                AssemblyToScanTypes.Add(assembly, types);
        }

        public static ScanTypes GetScanTypes([NotNull] Assembly assembly)
        {
            AssemblyToScanTypes.TryGetValue(assembly, out ScanTypes assemblyScanTypes);

            return assemblyScanTypes;
        }

        /// <summary>
        ///   Registers an assembly with the specified categories.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="categories">The categories to associate this assembly with.</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="assembly"/> is a <c>null</c> reference;
        ///   or
        ///   <paramref name="categories"/> is a <c>null</c> reference.
        /// </exception>
        public static void Register([NotNull] Assembly assembly, [NotNull] IEnumerable<string> categories)
        {
            if (assembly is null)
                throw new ArgumentNullException(nameof(assembly));
            if (categories is null)
                throw new ArgumentNullException(nameof(categories));

            HashSet<string> currentRegisteredCategories = null;

            lock (Lock)
            {
                if (!MapAssemblyToCategories.TryGetValue(assembly, out HashSet<string> registeredCategoriesPerAssembly))
                {
                    registeredCategoriesPerAssembly = new HashSet<string>();
                    MapAssemblyToCategories.Add(assembly, registeredCategoriesPerAssembly);
                }

                // Register the assembly name
                var assemblyName = assembly.GetName().Name;
                AssemblyNameToAssembly[assemblyName] = assembly;

                foreach (var category in categories)
                {
                    if (string.IsNullOrWhiteSpace(category))
                    {
                        Log.Value.Error($"Invalid empty category for assembly [{assembly}].");
                        continue;
                    }

                    if (registeredCategoriesPerAssembly.Add(category))
                    {
                        if (currentRegisteredCategories is null)
                        {
                            currentRegisteredCategories = new HashSet<string>();
                        }
                        currentRegisteredCategories.Add(category);
                    }

                    if (!MapCategoryToAssemblies.TryGetValue(category, out HashSet<Assembly> registeredAssembliesPerCategory))
                    {
                        registeredAssembliesPerCategory = new HashSet<Assembly>();
                        MapCategoryToAssemblies.Add(category, registeredAssembliesPerCategory);
                    }

                    registeredAssembliesPerCategory.Add(assembly);
                }
            }

            if (currentRegisteredCategories != null)
            {
                OnAssemblyRegistered(assembly, currentRegisteredCategories);
            }
        }

        /// <summary>
        ///   Registers an assembly with the specified categories.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="categories">The categories to associate this assembly with.</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="assembly"/> is a <c>null</c> reference;
        ///   or
        ///   <paramref name="categories"/> is a <c>null</c> reference.
        /// </exception>
        public static void Register([NotNull] Assembly assembly, [NotNull] params string[] categories)
        {
            Register(assembly, (IEnumerable<string>) categories);
        }

        /// <summary>
        ///   Unregisters the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public static void Unregister([NotNull] Assembly assembly)
        {
            // TODO: Reference counting? Waiting for "plugin" branch to be merged first anyway...
            HashSet<string> categoriesFound;

            lock (Lock)
            {
                if (MapAssemblyToCategories.TryGetValue(assembly, out categoriesFound))
                {
                    // Remove Assembly => Categories entry
                    MapAssemblyToCategories.Remove(assembly);

                    // Remove reverse Category => Assemblies entries
                    foreach (var category in categoriesFound)
                    {
                        if (MapCategoryToAssemblies.TryGetValue(category, out HashSet<Assembly> assembliesFound))
                        {
                            assembliesFound.Remove(assembly);
                        }
                    }
                }
            }

            if (categoriesFound != null)
            {
                OnAssemblyUnregistered(assembly, categoriesFound);
            }
        }

        private static void OnAssemblyRegistered(Assembly assembly, HashSet<string> categories)
        {
            AssemblyRegistered?.Invoke(null, new AssemblyRegisteredEventArgs(assembly, categories));
        }

        private static void OnAssemblyUnregistered(Assembly assembly, HashSet<string> categories)
        {
            AssemblyUnregistered?.Invoke(null, new AssemblyRegisteredEventArgs(assembly, categories));
        }

        /// <summary>
        ///   Represents a list of the types that matches a given <see cref="AssemblyScanAttribute"/> for a given
        ///   assembly.
        /// </summary>
        public class ScanTypes
        {
            public IReadOnlyDictionary<Type, List<Type>> Types { get; }

            public ScanTypes(Dictionary<Type, List<Type>> types)
            {
                Types = types;
            }
        }
    }
}
