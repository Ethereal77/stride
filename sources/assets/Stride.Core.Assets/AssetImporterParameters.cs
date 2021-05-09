// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

using Stride.Core.Diagnostics;

namespace Stride.Core.Assets
{
    /// <summary>
    ///   Represents the parameters used when importing assets with <see cref="IAssetImporter.Import"/>.
    /// </summary>
    public class AssetImporterParameters
    {
        /// <summary>
        ///   Gets the import input parameters.
        /// </summary>
        /// <value>The import input parameters.</value>
        public PropertyCollection InputParameters { get; private set; }

        /// <summary>
        ///   Gets the selected output types.
        /// </summary>
        /// <value>The selected output types.</value>
        public Dictionary<Type, bool> SelectedOutputTypes { get; private set; }

        /// <summary>
        ///   Gets or sets the logger to use during the import.
        /// </summary>
        public Logger Logger { get; set; }

        /// <summary>
        ///   Initializes a new instance of the <see cref="AssetImporterParameters"/> class.
        /// </summary>
        public AssetImporterParameters()
        {
            InputParameters = new PropertyCollection();
            SelectedOutputTypes = new Dictionary<Type, bool>();
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="AssetImporterParameters"/> class.
        /// </summary>
        /// <param name="supportedTypes">The supported types.</param>
        public AssetImporterParameters(params Type[] supportedTypes)
            : this((IEnumerable<Type>) supportedTypes)
        { }

        /// <summary>
        ///   Initializes a new instance of the <see cref="AssetImporterParameters"/> class.
        /// </summary>
        /// <param name="supportedTypes">The supported types.</param>
        /// <exception cref="ArgumentNullException"><paramref name="supportedTypes"/> is a <c>null</c> reference.</exception>
        /// <exception cref="ArgumentException">Invalid type specified. A supported type must be assignable to Asset.</exception>
        public AssetImporterParameters(IEnumerable<Type> supportedTypes) : this()
        {
            if (supportedTypes is null)
                throw new ArgumentNullException("supportedTypes");

            foreach (var type in supportedTypes)
            {
                if (!typeof(Asset).IsAssignableFrom(type))
                    throw new ArgumentException($"Invalid type [{type}]. The type must be assignable to Asset.", nameof(supportedTypes));

                SelectedOutputTypes[type] = true;
            }
        }

        /// <summary>
        ///   Determines whether the specified type is the type selected for output by this importer.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="Asset"/>.</typeparam>
        /// <returns><c>true</c> if the specified type is type selected for output by this importer; otherwise, <c>false</c>.</returns>
        public bool IsTypeSelectedForOutput<T>() where T : Asset
        {
            return IsTypeSelectedForOutput(typeof(T));
        }

        /// <summary>
        ///   Determines whether the specified type is the type selected for output by this importer.
        /// </summary>
        /// <param name="type">The type of <see cref="Asset"/>.</param>
        /// <returns><c>true</c> if the specified type is type selected for output by this importer; otherwise, <c>false</c>.</returns>
        public bool IsTypeSelectedForOutput(Type type)
        {
            if (SelectedOutputTypes.TryGetValue(type, out bool isSelected))
                return isSelected;

            return false;
        }

        /// <summary>
        ///   Gets a value indicating whether this instance has valid selected output types.
        /// </summary>
        /// <value><c>true</c> if this instance has selected output types; otherwise, <c>false</c>.</value>
        public bool HasSelectedOutputTypes =>
            SelectedOutputTypes.Count > 0 &&
            SelectedOutputTypes.Any(selectedOutputType => selectedOutputType.Value);
    }
}
