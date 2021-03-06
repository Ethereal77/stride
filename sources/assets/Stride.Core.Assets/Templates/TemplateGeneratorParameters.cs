// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;

using Stride.Core.Diagnostics;
using Stride.Core.IO;

namespace Stride.Core.Assets.Templates
{
    public sealed class SessionTemplateGeneratorParameters : TemplateGeneratorParameters
    {
        /// <summary>
        /// Gets or sets the current session.
        /// </summary>
        /// <value>The session.</value>
        public PackageSession Session { get; set; }

        protected override void ValidateParameters()
        {
            base.ValidateParameters();

            if (Description.Scope != TemplateScope.Session)
            {
                throw new InvalidOperationException($"[{nameof(Description)}.{nameof(Description.Scope)}] must be {TemplateScope.Session} in {GetType().Name}");
            }
            if (Session == null)
            {
                throw new InvalidOperationException($"[{nameof(Session)}] cannot be null in {GetType().Name}");
            }
        }
    }

    public class PackageTemplateGeneratorParameters : TemplateGeneratorParameters
    {
        public PackageTemplateGeneratorParameters()
        {
        }

        public PackageTemplateGeneratorParameters(TemplateGeneratorParameters parameters, Package package)
            : base(parameters)
        {
            Package = package;
        }
        /// <summary>
        /// Gets or sets the package in which to execute this template
        /// </summary>
        /// <value>The package.</value>
        public Package Package { get; set; }

        protected override void ValidateParameters()
        {
            base.ValidateParameters();

            if (Description.Scope != TemplateScope.Package && GetType() == typeof(PackageTemplateGeneratorParameters))
            {
                throw new InvalidOperationException($"[{nameof(Description)}.{nameof(Description.Scope)}] must be {TemplateScope.Package} in {GetType().Name}");
            }
            if (Package == null)
            {
                throw new InvalidOperationException($"[{nameof(Package)}] cannot be null in {GetType().Name}");
            }
        }
    }

    public class AssetTemplateGeneratorParameters : PackageTemplateGeneratorParameters
    {
        public AssetTemplateGeneratorParameters(UDirectory targetLocation, IEnumerable<UFile> sourceFiles = null)
        {
            TargetLocation = targetLocation;
            SourceFiles = new List<UFile>();
            if (sourceFiles != null)
                SourceFiles.AddRange(sourceFiles);
        }

        public UDirectory TargetLocation { get; }

        public List<UFile> SourceFiles { get; }

        /// <summary>
        /// Indicates whether the session has to be saved after the asset template generator has completed. The default is <c>false</c>.
        /// </summary>
        /// <remarks>This is an out parameter that asset template generator should set if needed.</remarks>
        public bool RequestSessionSave { get; set; } = false;

        protected override void ValidateParameters()
        {
            base.ValidateParameters();

            if (Description.Scope != TemplateScope.Asset)
            {
                throw new InvalidOperationException($"[{nameof(Description)}.{nameof(Description.Scope)}] must be {TemplateScope.Asset} in {GetType().Name}");
            }
            if (Package == null)
            {
                throw new InvalidOperationException($"[{nameof(Package)}] cannot be null in {GetType().Name}");
            }
        }
    }

    /// <summary>
    ///   Represents a set of parameters used by a <see cref="ITemplateGenerator{TParameters}"/> to generate a template.
    /// </summary>
    public abstract class TemplateGeneratorParameters
    {
        protected TemplateGeneratorParameters() { }

        protected TemplateGeneratorParameters(TemplateGeneratorParameters parameters)
        {
            Name = parameters.Name;
            Namespace = parameters.Namespace;
            OutputDirectory = parameters.OutputDirectory;
            Description = parameters.Description;
            Logger = parameters.Logger;
            parameters.Tags.CopyTo(ref Tags);
            Id = parameters.Id;
        }

        /// <summary>
        ///   Gets or sets the project name used to generate the template.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///   Gets or sets the project's unique identifier used to generate the template.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        ///   Gets or sets the default namespace of this project.
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        ///   Gets or sets the output directory for the template generator.
        /// </summary>
        public UDirectory OutputDirectory { get; set; }

        /// <summary>
        ///   Gets or sets the description of the template to generate.
        /// </summary>
        public TemplateDescription Description { get; set; }

        /// <summary>
        ///   Gets or sets a value that specifies if the template generator should run in unattended mode (with no UI).
        /// </summary>
        public bool Unattended { get; set; }

        /// <summary>
        ///   Gets or sets the logger to use to log the result of the generation.
        /// </summary>
        public LoggerResult Logger { get; set; }

        /// <summary>
        ///   Contains extra properties that can be consumed by a template generator.
        /// </summary>
        public PropertyContainer Tags;


        /// <summary>
        ///   Validates this parameters.
        /// </summary>
        public void Validate()
        {
            ValidateParameters();
        }

        /// <summary>
        ///   Gets the tag corresponding to the given property key.
        /// </summary>
        /// <typeparam name="T">The generic type of the property key.</typeparam>
        /// <param name="key">The property key for which to retrieve the value.</param>
        /// <returns>The value of the tag corresponding to the given property key.</returns>
        /// <exception cref="KeyNotFoundException">Tag not found in template generator parameters.</exception>
        public T GetTag<T>(PropertyKey<T> key)
        {
            if (!Tags.TryGetValue(key, out T result))
                throw new KeyNotFoundException("Tag not found in template generator parameters.");

            return result;
        }

        /// <summary>
        ///   Gets the tag corresponding to the given property key if available, otherwise returns a default value.
        /// </summary>
        /// <typeparam name="T">The generic type of the property key.</typeparam>
        /// <param name="key">The property key for which to retrieve the value.</param>
        /// <returns>
        ///   The value of the tag corresponding to the given property key if available, or the default value of the
        ///   property key otherwise.
        /// </returns>
        public T TryGetTag<T>(PropertyKey<T> key)
        {
            return Tags.Get(key);
        }

        /// <summary>
        ///   Determines if a tag corresponding to the given property key exists.
        /// </summary>
        /// <typeparam name="T">The generic type of the property key.</typeparam>
        /// <param name="key">The property key for which to check its existence.</param>
        /// <returns><c>true</c> if the template parameters have the specified tag; <c>fañse</c> otherwise.</returns>
        public bool HasTag<T>(PropertyKey<T> key)
        {
            return Tags.ContainsKey(key);
        }

        /// <summary>
        ///   Sets a tag for a given property key.
        /// </summary>
        /// <typeparam name="T">The generic type of the property key.</typeparam>
        /// <param name="key">The property key for which to set the value.</param>
        /// <param name="value">Value for the tag.</param>
        public void SetTag<T>(PropertyKey<T> key, T value)
        {
            Tags[key] = value;
        }

        protected virtual void ValidateParameters()
        {
            if (Name is null)
                throw new InvalidOperationException($"[{nameof(Name)}] cannot be null in {GetType().Name}.");

            if (OutputDirectory is null && Description.Scope == TemplateScope.Session)
                throw new InvalidOperationException($"[{nameof(OutputDirectory)}] cannot be null in {GetType().Name}.");

            if (Description is null)
                throw new InvalidOperationException($"[{nameof(Description)}] cannot be null in {GetType().Name}.");

            if (Logger is null)
                throw new InvalidOperationException($"[{nameof(Logger)}] cannot be null in {GetType().Name}.");
        }
    }
}
