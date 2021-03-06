// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Windows;
using System.Windows.Controls;

namespace Stride.LauncherApp.Views
{
    public class StaysOpenContextMenu : ContextMenu
    {
        private bool mustStayOpen;

        static StaysOpenContextMenu()
        {
            IsOpenProperty.OverrideMetadata(
                typeof(StaysOpenContextMenu),
                new FrameworkPropertyMetadata(false, null, CoerceIsOpen));
            StaysOpenProperty.OverrideMetadata(
                typeof(StaysOpenContextMenu),
                new FrameworkPropertyMetadata(false, PropertyChanged, CoerceStaysOpen));
        }

        private static void PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            StaysOpenContextMenu menu = (StaysOpenContextMenu)d;
            menu.mustStayOpen = (bool)e.NewValue;
        }

        private static object CoerceStaysOpen(DependencyObject d, object basevalue)
        {
            d.CoerceValue(IsOpenProperty);
            return basevalue;
        }

        private static object CoerceIsOpen(DependencyObject d, object basevalue)
        {
            StaysOpenContextMenu menu = (StaysOpenContextMenu)d;
            if (menu.StaysOpen && menu.mustStayOpen)
            {
                return true;
            }

            return basevalue;
        }

        public void CloseContextMenu()
        {
            mustStayOpen = false;
            IsOpen = false;
        }
    }
}
