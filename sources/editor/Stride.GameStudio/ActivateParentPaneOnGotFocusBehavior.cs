// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Windows;
using System.Windows.Controls;

using Microsoft.Xaml.Behaviors;

using AvalonDock.Controls;

using Stride.Core.Presentation.Extensions;

namespace Stride.GameStudio
{
    // TODO: This behavior was previously broken, it might work now (migration to AvalonDock) but has not been tested!
    public class ActivateParentPaneOnGotFocusBehavior : Behavior<Control>
    {
        protected override void OnAttached()
        {
            AssociatedObject.GotKeyboardFocus += GotFocus;
            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.GotKeyboardFocus -= GotFocus;
        }

        private void GotFocus(object sender, RoutedEventArgs e)
        {
            var pane = AssociatedObject.FindVisualParentOfType<LayoutAnchorableControl>();
            if (pane != null)
            {
                pane.Model.IsActive = true;
            }
        }
    }
}
