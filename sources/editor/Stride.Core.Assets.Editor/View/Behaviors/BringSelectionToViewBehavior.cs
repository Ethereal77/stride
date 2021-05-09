// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Linq;
using System.Windows.Controls;

using Microsoft.Xaml.Behaviors;

using Stride.Core.Assets.Editor.View.Controls;
using Stride.Core.Presentation.Controls;
using Stride.Core.Presentation.Extensions;

namespace Stride.Core.Assets.Editor.View.Behaviors
{
    public class BringSelectionToViewBehavior : Behavior<EditableContentListBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SelectionChanged += SelectionChanged;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.SelectionChanged -= SelectionChanged;
            base.OnDetaching();
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AssociatedObject.SelectedIndex >= 0)
            {
                var panel = AssociatedObject.FindVisualChildOfType<VirtualizingTilePanel>();
                if (panel != null)
                {
                    panel.ScrollToIndexedItem(AssociatedObject.SelectedIndex);
                }
            }
        }
    }
}
