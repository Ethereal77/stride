// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Windows.Forms;

using Stride.Assets.Presentation.ViewModel;
using Stride.UI;

namespace Stride.Assets.Presentation.AssetEditors.UIEditor.Adorners
{
    internal interface IResizingAdorner
    {
        UIElement GameSideElement { get; }

        ResizingDirection ResizingDirection { get; }

        Cursor GetCursor();

        /// <summary>
        /// Notify the adorner once the resizing operation is completed.
        /// </summary>
        void OnResizingCompleted();

        /// <summary>
        /// Called on the adorner during the resizing operation.
        ///  </summary>
        /// <param name="horizontalChange">The horizontal delta of the resizing.</param>
        /// <param name="verticalChange">The vertical delta of the resizing.</param>
        void OnResizingDelta(float horizontalChange, float verticalChange);
    }
}
