﻿// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Assets;
using Stride.Core.Assets.Compiler;
using Stride.Core;
using Stride.Engine;

namespace Stride.Assets.UI
{
    /// <summary>
    /// This assets represents a tree of UI elements. 
    /// </summary>
    [DataContract("UIPageAsset")]
    [AssetDescription(FileExtension, AllowArchetype = false)]
    [AssetContentType(typeof(UIPage))]
    [AssetFormatVersion(StrideConfig.PackageName, CurrentVersion, "2.1.0.1")]
    public sealed partial class UIPageAsset : UIAssetBase
    {
        private const string CurrentVersion = "2.1.0.1";

        /// <summary>
        /// The default file extension used by the <see cref="UIPageAsset"/>.
        /// </summary>
        public const string FileExtension = ".sduipage";
    }
}
