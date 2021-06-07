using System;
using System.Windows;

namespace Stride.Roslyn.Editor
{
    public static class CommonEvent
    {
        public static RoutedEvent Register<TOwner, TEventArgs>(string name, RoutingStrategy routing)
            where TEventArgs : RoutedEventArgs
        {
            return EventManager.RegisterRoutedEvent(name, routing, typeof(EventHandler<TEventArgs>), typeof(TOwner));
        }
    }
}