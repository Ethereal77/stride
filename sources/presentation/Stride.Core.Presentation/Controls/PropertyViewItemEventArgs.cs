// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Windows;

namespace Xenko.Core.Presentation.Controls
{
    public class PropertyViewItemEventArgs : RoutedEventArgs
    {
        public PropertyViewItem Container { get; private set; }

        public object Item { get; private set; }

        public PropertyViewItemEventArgs(RoutedEvent routedEvent, object source, PropertyViewItem container, object item)
            : base(routedEvent, source)
        {
            Container = container;
            Item = item;
        }
    }
}
