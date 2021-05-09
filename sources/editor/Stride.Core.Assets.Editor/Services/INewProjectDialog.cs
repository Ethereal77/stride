// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

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
