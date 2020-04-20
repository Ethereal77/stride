// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Stride.Core.Assets
{
    public sealed class RawAssetImporter : RawAssetImporterBase<RawAsset>
    {
        private static readonly Guid Uid = new Guid("6f86ec95-c1ca-41e1-8adc-1449bb5ce3be");

        public RawAssetImporter()
        {
            // Raw asset is always last
            Order = int.MaxValue;
        }

        /// <inheritdoc />
        public override Guid Id => Uid;

        /// <inheritdoc />
        public override string Description => "Generic importer for raw assets";

        /// <inheritdoc />
        public override string SupportedFileExtensions => "*.*";

        /// <inheritdoc />
        public override bool IsSupportingFile(string filePath)
        {
            // Always return true
            return true;
        }

    }
}
