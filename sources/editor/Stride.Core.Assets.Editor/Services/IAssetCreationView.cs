// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Threading.Tasks;

using Stride.Core.Assets.Editor.ViewModel;
using Stride.Core.IO;

namespace Stride.Core.Assets.Editor.Services
{
    public interface IAssetCreationView
    {
        Task<AssetItem> Create(UFile defaultUrl, SessionViewModel sessionViewModel, DirectoryBaseViewModel targetDirectoryViewModel);
    }
}
