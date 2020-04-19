// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Core.Assets.Editor.Quantum.ViewModels
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
