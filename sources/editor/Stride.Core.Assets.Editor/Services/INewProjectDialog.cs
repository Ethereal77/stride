// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Core.Assets.Editor.ViewModel;
using Stride.Core.IO;
using Stride.Core.Presentation.Services;

namespace Stride.Core.Assets.Editor.Services
{
    public interface INewProjectDialog : IModalDialog
    {
        UDirectory DefaultOutputDirectory { get; set; }

        NewPackageParameters Parameters { get; }
    }
}
