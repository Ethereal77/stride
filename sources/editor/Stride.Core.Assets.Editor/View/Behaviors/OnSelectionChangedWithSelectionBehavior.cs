// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Windows.Controls.Primitives;

using Stride.Core.Presentation.Behaviors;

namespace Stride.Core.Assets.Editor.View.Behaviors
{
    public class OnSelectionChangedWithSelectionBehavior : OnEventCommandBehavior
    {
        public OnSelectionChangedWithSelectionBehavior()
        {
            EventName = "SelectionChanged";
        }

        protected override void OnAttached()
        {
            if (!(AssociatedObject is Selector))
                throw new InvalidOperationException("The OnSelectionChangedWithSelectionBehavior must be attached to a Selector.");

            base.OnAttached();
        }

        protected override void OnEvent()
        {
            var selector = (Selector)AssociatedObject;
            if (selector.SelectedItem != null)
                base.OnEvent();
        }
    }
}
