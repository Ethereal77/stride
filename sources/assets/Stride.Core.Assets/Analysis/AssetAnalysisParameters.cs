// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.IO;

namespace Stride.Core.Assets.Analysis
{
    /// <summary>
    /// Parameters for asset analysis.
    /// </summary>
    public class AssetAnalysisParameters
    {
        public bool IsLoggingAssetNotFoundAsError { get; set; }

        public bool IsProcessingAssetReferences { get; set; }

        public bool IsProcessingUPaths { get; set; }

        public bool SetDirtyFlagOnAssetWhenFixingAbsoluteUFile { get; set; }

        public bool SetDirtyFlagOnAssetWhenFixingUFile { get; set; }

        public UPathType ConvertUPathTo { get; set; }

        public virtual AssetAnalysisParameters Clone()
        {
            return (AssetAnalysisParameters)MemberwiseClone();
        }
    }
}
