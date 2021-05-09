// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;

using Mono.Cecil;

namespace Stride.Core.AssemblyProcessor
{
    /// <summary>
    ///   Allow to register assemblies manually, with their in-memory representation if necessary.
    /// </summary>
    public class CustomAssemblyResolver : BaseAssemblyResolver
    {
        /// <summary>
        ///   Assemblies stored as byte arrays.
        /// </summary>
        private readonly Dictionary<AssemblyDefinition, byte[]> assemblyData = new Dictionary<AssemblyDefinition, byte[]>();

        private readonly IDictionary<string, AssemblyDefinition> assemblyCache = new Dictionary<string, AssemblyDefinition>(StringComparer.Ordinal);
        private readonly List<string> referencePaths = new List<string>();

        public List<string> References { get; } = new List<string>();

        /// <summary>
        ///   Gets or sets the Windows SDK directory for Windows 10 apps.
        /// </summary>
        public string WindowsKitsReferenceDirectory { get; set; }

        private HashSet<string> existingWindowsKitsReferenceAssemblies;

        protected override void Dispose(bool disposing)
        {
            foreach (var assemblyDef in assemblyCache)
                assemblyDef.Value.Dispose();

            assemblyCache.Clear();
            assemblyData.Clear();
            References.Clear();
            existingWindowsKitsReferenceAssemblies?.Clear();

            base.Dispose(disposing);
        }

        public override AssemblyDefinition Resolve(AssemblyNameReference name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (assemblyCache.TryGetValue(name.FullName, out AssemblyDefinition assembly))
                return assembly;

            assembly = base.Resolve(name);
            assemblyCache[name.FullName] = assembly;

            return assembly;
        }

        public void RegisterAssembly(AssemblyDefinition assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            var name = assembly.Name.FullName;
            if (assemblyCache.ContainsKey(name))
                return;

            assemblyCache[name] = assembly;
        }

        public void RegisterAssemblies(List<AssemblyDefinition> mergedAssemblies)
        {
            foreach (var assemblyDefinition in mergedAssemblies)
            {
                RegisterAssembly(assemblyDefinition);
            }
        }

        /// <summary>
        ///   Registers the specified assembly.
        /// </summary>
        /// <param name="assembly">Assembly to register.</param>
        public void Register(AssemblyDefinition assembly)
        {
            RegisterAssembly(assembly);
        }

        /// <summary>
        ///   Registers the specified assembly.
        /// </summary>
        /// <param name="assembly">Assembly to register.</param>
        public void Register(AssemblyDefinition assembly, byte[] peData)
        {
            assemblyData[assembly] = peData;
            RegisterAssembly(assembly);
        }

        public void RegisterReference(string path)
        {
            References.Add(path);
            referencePaths.Add(Path.GetDirectoryName(path));
        }

        /// <summary>
        ///   Gets the assembly data (if it exists).
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>Data for the specified assembly.</returns>
        public byte[] GetAssemblyData(AssemblyDefinition assembly)
        {
            assemblyData.TryGetValue(assembly, out byte[] data);
            return data;
        }

        private static readonly string[] assemblyExtensions = new[] { ".dll", ".exe" };

        public override AssemblyDefinition Resolve(AssemblyNameReference name, ReaderParameters parameters)
        {
            // Try list of references
            foreach (var reference in References)
            {
                if (string.Compare(Path.GetFileNameWithoutExtension(reference), name.Name, StringComparison.OrdinalIgnoreCase) == 0 &&
                    File.Exists(reference))
                {
                    return GetAssembly(reference, parameters);
                }
            }

            // Try list of reference paths
            foreach (var referencePath in referencePaths)
            {
                foreach (var extension in assemblyExtensions)
                {
                    var assemblyFile = Path.Combine(referencePath, name.Name + extension);
                    if (File.Exists(assemblyFile))
                    {
                        // Add it as a new reference
                        References.Add(assemblyFile);

                        return GetAssembly(assemblyFile, parameters);
                    }
                }
            }

            if (WindowsKitsReferenceDirectory != null)
            {
                if (existingWindowsKitsReferenceAssemblies == null)
                {
                    // First time, make list of existing assemblies in windows kits directory
                    existingWindowsKitsReferenceAssemblies = new HashSet<string>();

                    try
                    {
                        foreach (var directory in Directory.EnumerateDirectories(WindowsKitsReferenceDirectory))
                        {
                            existingWindowsKitsReferenceAssemblies.Add(Path.GetFileName(directory));
                        }
                    }
                    catch (Exception) { }
                }

                // Look for this assembly in the Windows SDK directory
                if (existingWindowsKitsReferenceAssemblies.Contains(name.Name))
                {
                    var assemblyFile = Path.Combine(WindowsKitsReferenceDirectory, name.Name, name.Version.ToString(), name.Name + ".winmd");
                    if (File.Exists(assemblyFile))
                    {
                        if (parameters.AssemblyResolver == null)
                            parameters.AssemblyResolver = this;

                        return ModuleDefinition.ReadModule(assemblyFile, parameters).Assembly;
                    }
                }
            }

            if (parameters == null)
                parameters = new ReaderParameters();

            try
            {
                // Check .winmd files as well
                var assembly = SearchDirectoryExtra(name, GetSearchDirectories(), parameters);
                if (assembly != null)
                    return assembly;

                return base.Resolve(name, parameters);
            }
            catch (AssemblyResolutionException)
            {
                // Check cache again, ignoring version numbers this time
                foreach (var assembly in assemblyCache)
                {
                    if (assembly.Value.Name.Name == name.Name)
                    {
                        return assembly.Value;
                    }
                }
                throw;
            }
        }

        private static readonly string[] windowsMetadataExtensions = new[] { ".winmd" };

        // Copied from BaseAssemblyResolver
        private AssemblyDefinition SearchDirectoryExtra(AssemblyNameReference name, IEnumerable<string> directories, ReaderParameters parameters)
        {
            foreach (var directory in directories)
            {
                foreach (var extension in windowsMetadataExtensions)
                {
                    string file = Path.Combine(directory, name.Name + extension);
                    if (File.Exists(file))
                        return GetAssembly(file, parameters);
                }
            }

            return null;
        }

        // Copied from BaseAssemblyResolver
        private AssemblyDefinition GetAssembly(string file, ReaderParameters parameters)
        {
            if (parameters.AssemblyResolver == null)
                parameters.AssemblyResolver = this;

            return ModuleDefinition.ReadModule(file, parameters).Assembly;
        }
    }
}
