// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Diagnostics;
using System.Threading.Tasks;

using Stride.Core.Assets.Editor.ViewModel;

namespace Stride.Assets.Presentation.SceneEditor.Services
{
    public interface IPickedDebugger
    {
        string Name { get; }

        Task<Process> Launch(SessionViewModel session);
    }
}
