// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Assets.Editor.View.Behaviors;

using Xceed.Wpf.AvalonDock.Layout;

namespace Xenko.GameStudio
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
