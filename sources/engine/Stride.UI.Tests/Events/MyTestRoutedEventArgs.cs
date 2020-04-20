// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.UI.Events;

namespace Stride.UI.Tests.Events
{
    internal class MyTestRoutedEventArgs : RoutedEventArgs
    {
        public MyTestRoutedEventArgs(RoutedEvent routedEvent)
            : base(routedEvent)
        {
        }
    }
}
