// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Stride.UI.Events
{
    /// <summary>
    ///   Provides event-related utility methods that register routed events for class owners and add class handlers.
    /// </summary>
    public static class EventManager
    {
        private static readonly object SyncRoot = new object();

        /// <summary>
        ///   Finds the routed event identified by its name and owner.
        /// </summary>
        /// <param name="ownerType">The type to start the search with. Base classes are included in the search.</param>
        /// <param name="eventName">The event name.</param>
        /// <returns>The matching <see cref="RoutedEvent"/> identifier if any match is found; otherwise, <c>null</c>.</returns>
        public static RoutedEvent GetRoutedEvent(Type ownerType, string eventName)
        {
            var currentType = ownerType;
            while (currentType != null)
            {
                if (OwnerToEvents.TryGetValue(currentType, out var eventsMap) &&
                    eventsMap.TryGetValue(eventName, out var routedEvent))
                    return routedEvent;

                currentType = currentType.GetTypeInfo().BaseType;
            }

            return null;
        }

        /// <summary>
        ///   Returns the identifiers for the routed events that have been registered with the event system.
        /// </summary>
        /// <returns>An array of <see cref="RoutedEvent"/> that contains the registered objects.</returns>
        public static RoutedEvent[] GetRoutedEvents()
        {
            return RoutedEvents.ToArray();
        }

        /// <summary>
        ///   Finds all the routed event identifiers for the events that are registered with the provided owner type.
        /// </summary>
        /// <param name="ownerType">The type to start the search with. Base classes are included in the search.</param>
        /// <returns>An array of matching <see cref="RoutedEvent"/> identifiers if any match is found; otherwise, <c>null</c>.</returns>
        public static RoutedEvent[] GetRoutedEventsForOwner(Type ownerType)
        {
            var types = new List<Type>();

            var currentType = ownerType;
            while (currentType != null)
            {
                types.Add(currentType);
                currentType = currentType.GetTypeInfo().BaseType;
            }

            return types.Where(t => OwnerToEvents.ContainsKey(t)).SelectMany(t => OwnerToEvents[t].Values).ToArray();
        }

        /// <summary>
        ///   Registers a class handler for a particular routed event, with the option to handle events where event data is already marked handled.
        /// </summary>
        /// <param name="classType">The type of the class that is declaring class handling.</param>
        /// <param name="routedEvent">The routed event identifier of the event to handle.</param>
        /// <param name="handler">A reference to the class handler implementation.</param>
        /// <param name="handledEventsToo">
        ///   <c>true</c> to invoke this class handler even if arguments of the routed event have been marked as handled;
        ///   <c>false</c> to retain the default behavior of not invoking the handler on any marked-handled event.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="classType"/>, <paramref name="routedEvent"/>, or <paramref name="handler"/> are a <c>null</c> reference.
        /// </exception>
        public static void RegisterClassHandler<T>(Type classType, RoutedEvent<T> routedEvent, EventHandler<T> handler, bool handledEventsToo = false) where T : RoutedEventArgs
        {
            if (classType is null)
                throw new ArgumentNullException(nameof(classType));
            if (routedEvent is null)
                throw new ArgumentNullException(nameof(routedEvent));
            if (handler is null)
                throw new ArgumentNullException(nameof(handler));

            lock(SyncRoot)
            {
                if (!ClassesToClassHandlers.ContainsKey(classType))
                    ClassesToClassHandlers[classType] = new Dictionary<RoutedEvent, RoutedEventHandlerInfo>();

                ClassesToClassHandlers[classType][routedEvent] = new RoutedEventHandlerInfo<T>(handler, handledEventsToo);
            }
        }

        /// <summary>
        ///   Gets the class handler for a class type and a routed event.
        /// </summary>
        /// <param name="classType">The type of the class that is handling the event.</param>
        /// <param name="routedEvent">The routed event to handle.</param>
        /// <returns>The <see cref="RoutedEventHandlerInfo"/> representing the class handler.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="classType"/>, or <paramref name="routedEvent"/> are a <c>null</c> reference.</exception>
        internal static RoutedEventHandlerInfo GetClassHandler(Type classType, RoutedEvent routedEvent)
        {
            if (classType is null)
                throw new ArgumentNullException(nameof(classType));
            if (routedEvent is null)
                throw new ArgumentNullException(nameof(routedEvent));

            var currentType = classType;
            while (currentType != null)
            {
                if (ClassesToClassHandlers.TryGetValue(currentType, out var classHandlersMap) &&
                    classHandlersMap.TryGetValue(routedEvent, out var handler))
                    return handler;

                currentType = currentType.GetTypeInfo().BaseType;
            }

            return null;
        }

        private static readonly Dictionary<Type, Dictionary<RoutedEvent, RoutedEventHandlerInfo>> ClassesToClassHandlers = new Dictionary<Type, Dictionary<RoutedEvent, RoutedEventHandlerInfo>>();

        /// <summary>
        ///   Registers a new routed event.
        /// </summary>
        /// <param name="name">
        ///   The name of the routed event. The name must be unique within the owner type (base classes included) and cannot be
        ///   <c>null</c> or an empty string.
        /// </param>
        /// <param name="routingStrategy">The routing strategy of the event.</param>
        /// <param name="ownerType">The owner class type of the routed event. This cannot be <c>null</c>.</param>
        /// <returns>
        ///   The identifier for the newly registered routed event.
        ///   This identifier object can now be stored as a static field in a class and then used as a parameter for methods that
        ///   attach handlers to the event.
        ///   <para/>
        ///   The routed event identifier is also used for other event system APIs.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> or <paramref name="ownerType"/> are a <c>null</c> reference.</exception>
        /// <exception cref="InvalidOperationException">
        ///   A routed event with name <paramref name="name"/> already exists for the type <paramref name="ownerType"/> and its parents.
        /// </exception>
        public static RoutedEvent<T> RegisterRoutedEvent<T>(string name, RoutingStrategy routingStrategy, Type ownerType) where T: RoutedEventArgs
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (ownerType == null) throw new ArgumentNullException(nameof(ownerType));

            if (GetRoutedEvent(ownerType, name) != null)
                throw new InvalidOperationException("A routed event named '" + name + "' already exists in provided owner type '" + ownerType + "' or base classes.");

            var newRoutedEvent = new RoutedEvent<T> {  Name = name, OwnerType = ownerType, RoutingStrategy = routingStrategy, };
            lock(SyncRoot)
            {
                RoutedEvents.Add(newRoutedEvent);

                if (!OwnerToEvents.ContainsKey(ownerType))
                    OwnerToEvents[ownerType] = new Dictionary<string, RoutedEvent>();

                OwnerToEvents[ownerType][name] = newRoutedEvent;
            }

            return newRoutedEvent;
        }

        private static readonly List<RoutedEvent> RoutedEvents = new List<RoutedEvent>();
        private static readonly Dictionary<Type, Dictionary<string, RoutedEvent>> OwnerToEvents = new Dictionary<Type, Dictionary<string, RoutedEvent>>();

        /// <summary>
        ///   Resets all the registers and invalidates all the created routed events.
        ///   It is mostly used for tests purposes.
        /// </summary>
        internal static void UnregisterRoutedEvent(RoutedEvent routedEvent)
        {
            lock(SyncRoot)
            {
                RoutedEvents.Remove(routedEvent);
                if (OwnerToEvents.TryGetValue(routedEvent.OwnerType, out var events))
                    events.Remove(routedEvent.Name);
                foreach (var classHandlersMap in ClassesToClassHandlers)
                    classHandlersMap.Value.Remove(routedEvent);
            }
        }
    }
}
