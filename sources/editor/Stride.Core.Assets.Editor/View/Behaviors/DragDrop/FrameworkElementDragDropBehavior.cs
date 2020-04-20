// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using System.Windows;

using Stride.Core.Extensions;

namespace Stride.Core.Assets.Editor.View.Behaviors
{
    public class FrameworkElementDragDropBehavior : DragDropBehavior<FrameworkElement, FrameworkElement>
    {
        protected override IEnumerable<object> GetItemsToDrag(FrameworkElement container)
        {
            return AssociatedObject.DataContext?.ToEnumerable<object>() ?? Enumerable.Empty<object>();
        }

        protected override FrameworkElement GetContainer(object source)
        {
            return AssociatedObject;
        }
    }
}
