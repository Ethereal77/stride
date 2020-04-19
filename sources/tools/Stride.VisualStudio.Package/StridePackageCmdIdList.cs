// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

// PkgCmdID.cs
// MUST match PkgCmdID.h

using System;
using System.ComponentModel.Design;

using Microsoft.VisualStudio.Shell;

namespace Xenko.VisualStudio
{
    static class XenkoPackageCmdIdList
    {
        public const uint cmdXenkoPlatformSelect =        0x100;
        public const uint cmdXenkoOpenWithGameStudio = 0x101;
        public const uint cmdXenkoPlatformSelectList = 0x102;
        public const uint cmdXenkoCleanIntermediateAssetsSolutionCommand = 0x103;
        public const uint cmdXenkoCleanIntermediateAssetsProjectCommand = 0x104;
    }
}
