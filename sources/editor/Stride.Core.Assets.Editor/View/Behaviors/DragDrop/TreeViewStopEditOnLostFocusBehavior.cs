// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core.Presentation.Behaviors;
using TreeViewItem = Stride.Core.Presentation.Controls.TreeViewItem;

namespace Stride.Core.Assets.Editor.View.Behaviors
{
    public class TreeViewStopEditOnLostFocusBehavior : OnEventBehavior
    {
        public TreeViewStopEditOnLostFocusBehavior()
        {
            EventName = "LostFocus";
        }

        protected override void OnAttached()
        {
            if (!(AssociatedObject is TreeViewItem))
                throw new InvalidOperationException("This behavior must be attached to an instance of TreeViewItem.");
            base.OnAttached();
        }

        protected override void OnEvent()
        {
            var treeViewItem = (TreeViewItem)AssociatedObject;
            treeViewItem.SetCurrentValue(TreeViewItem.IsEditingProperty, false);
        }
    }
}
