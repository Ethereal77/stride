// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Diagnostics;
using System.Threading.Tasks;

using Xenko.Core.Assets.Editor.ViewModel;

namespace Xenko.Assets.Presentation.SceneEditor.Services
{
    public interface IPickedDebugger
    {
        string Name { get; }

        Task<Process> Launch(SessionViewModel session);
    }
}
