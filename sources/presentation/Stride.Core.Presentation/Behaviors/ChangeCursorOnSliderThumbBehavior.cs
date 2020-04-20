// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

using Stride.Core.Presentation.Extensions;

namespace Stride.Core.Presentation.Behaviors
{
    public class ChangeCursorOnSliderThumbBehavior : DeferredBehaviorBase<Slider>
    {
        protected override void OnAttachedAndLoaded()
        {
            var thumb = AssociatedObject.FindVisualChildOfType<Thumb>();
            if (thumb != null)
                thumb.Cursor = Cursors.SizeWE;

            base.OnAttachedAndLoaded();
        }
    }
}
