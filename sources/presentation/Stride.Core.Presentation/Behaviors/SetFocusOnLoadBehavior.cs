// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

using Microsoft.Xaml.Behaviors;

namespace Stride.Core.Presentation.Behaviors
{
    public class SetFocusOnLoadBehavior : Behavior<FrameworkElement>
    {
        protected override void OnAttached()
        {
            if (AssociatedObject.IsLoaded)
                AssociatedObject.Focus();
            else
                AssociatedObject.Loaded += OnHostLoaded;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= OnHostLoaded;
        }

        private void OnHostLoaded(object sender, RoutedEventArgs e)
        {
            AssociatedObject.Focus();
        }
    }
}
