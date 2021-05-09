// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core.Assets.Yaml;
using Stride.Core.Annotations;

namespace Stride.Core.Assets.Editor.Services
{
    /// <summary>
    /// Interface for a copy processor used by the <see cref="ICopyPasteService"/>.
    /// </summary>
    public interface ICopyProcessor
    {
        /// <summary>
        /// Gets whether this processor is able to process the data.
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns><c>true</c> if this processor is able to process the data; otherwise, <c>false</c>.</returns>
        bool Accept([NotNull] Type dataType);

        /// <summary>
        /// Process the data before it is serialized.
        /// </summary>
        /// <param name="data">The data to process.</param>
        /// <param name="metadata"></param>
        /// <returns><c>true</c> if the data was successfully processed; otherwise, <c>false</c>.</returns>
        bool Process([NotNull] ref object data, [NotNull] AttachedYamlAssetMetadata metadata);
    }
}
