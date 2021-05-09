// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Assets.Editor.View.Behaviors;

using AvalonDock.Layout;

namespace Stride.GameStudio
{
    /// <summary>
    /// An implementation of the <see cref="ActivateOnLocationChangedBehavior{T}"/> for the <see cref="LayoutAnchorable"/> control.
    /// </summary>
    public class LayoutAnchorableActivateOnLocationChangedBehavior : ActivateOnLocationChangedBehavior<LayoutAnchorable>
    {
        protected override void Activate()
        {
            AssociatedObject.Show();
            AssociatedObject.IsSelected = true;
        }
    }
}
