// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Windows;

namespace Stride.Core.Presentation.Core
{
    public class CancelRoutedEventArgs : RoutedEventArgs
    {
        public bool Cancel { get; set; }

        public CancelRoutedEventArgs(RoutedEvent routedEvent, bool cancel = false)
            : base(routedEvent)
        {
            Cancel = cancel;
        }
    }

    public delegate void CancelRoutedEventHandler(object sender, CancelRoutedEventArgs e);
}
