// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using Xenko.Core.Assets.Editor.ViewModel;
using Xenko.Core.Extensions;

namespace Xenko.Core.Assets.Editor.View.Behaviors
{
    public class ListBoxDragDropBehavior : DragDropBehavior<ListBox, ListBoxItem>
    {
        protected override IEnumerable<object> GetItemsToDrag(ListBoxItem container)
        {
            if (container != null)
            {
                var sourceItem = container.DataContext;
                return AssociatedObject.SelectedItems.Contains(sourceItem) ? AssociatedObject.SelectedItems.Cast<object>() : sourceItem.ToEnumerable<object>();
            }
            return Enumerable.Empty<object>();
        }

        protected override IInsertChildViewModel GetInsertTargetItem(ListBoxItem container, Point mousePosition, out InsertPosition insertPosition)
        {
            insertPosition = InsertPosition.Before;

            if (mousePosition.Y >= 0 && mousePosition.Y <= InsertThreshold)
            {
                insertPosition = InsertPosition.Before;
                return container.DataContext as IInsertChildViewModel;
            }
            if (mousePosition.Y >= container.ActualHeight - InsertThreshold && mousePosition.Y <= container.ActualHeight)
            {
                insertPosition = InsertPosition.After;
                return container.DataContext as IInsertChildViewModel;
            }
            return null;
        }

    }
}
