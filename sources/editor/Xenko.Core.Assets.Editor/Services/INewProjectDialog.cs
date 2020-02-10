// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Assets.Editor.ViewModel;
using Xenko.Core.IO;
using Xenko.Core.Presentation.Services;

namespace Xenko.Core.Assets.Editor.Services
{
    public interface INewProjectDialog : IModalDialog
    {
        UDirectory DefaultOutputDirectory { get; set; }

        NewPackageParameters Parameters { get; }
    }
}
