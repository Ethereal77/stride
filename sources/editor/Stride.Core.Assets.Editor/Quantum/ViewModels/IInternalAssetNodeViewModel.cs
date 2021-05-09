// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Core.Assets.Editor.Quantum.ViewModels
{
    /// <summary>
    /// Interface representing protected properties of <see cref="IAssetNodeViewModel"/> instances.
    /// </summary>
    /// <remarks>
    /// This interface is purely internal and exists only because implementations of <see cref="IAssetNodeViewModel"/>
    /// all inherit from generic classes and do not have a common non-generic base.
    /// </remarks>
    internal interface IInternalAssetNodeViewModel
    {
        void ChildOverrideChanging();
        void ChildOverrideChanged();
    }
}
