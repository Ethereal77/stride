// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Windows;

namespace Stride.Core.Presentation.Controls
{
    public class TreeViewItemEventArgs : RoutedEventArgs
    {
        public TreeViewItem Container { get; private set; }

        public object Item { get; private set; }

        public TreeViewItemEventArgs(RoutedEvent routedEvent, object source, TreeViewItem container, object item)
            : base(routedEvent, source)
        {
            Container = container;
            Item = item;
        }
    }
}
