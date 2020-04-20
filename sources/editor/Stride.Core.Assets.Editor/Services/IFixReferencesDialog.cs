// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Core.Presentation.Services;

namespace Stride.Core.Assets.Editor.Services
{
    public interface IFixReferencesDialog : IModalDialog
    {
        /// <summary>
        /// Applies the fixes decided by the user to the actual objects.
        /// </summary>
        void ApplyReferenceFixes();
    }
}
