// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Stride.Input
{
    /// <summary>
    ///   Represents a virtual button (a key from a keyboard, a mouse button, an axis of a joystick, etc).
    /// </summary>
    public abstract partial class VirtualButton : IVirtualButton
    {
        internal const int TypeIdMask = 0x0FFFFFFF;

        private static readonly Dictionary<int, VirtualButton> mapIp = new Dictionary<int, VirtualButton>();
        private static readonly Dictionary<string, VirtualButton> mapName = new Dictionary<string, VirtualButton>();

        private static readonly List<VirtualButton> registered = new List<VirtualButton>();
        private static IReadOnlyCollection<VirtualButton> registeredReadOnly;


        /// <summary>
        ///   Initializes a new instance of the <see cref="VirtualButton" /> class.
        /// </summary>
        /// <param name="shortName">The short name of the button.</param>
        /// <param name="type">The type of virtual button.</param>
        /// <param name="id">The unique id of the button.</param>
        /// <param name="isPositiveAndNegative"><c>true</c> if the value of the buttton can be positive and negative.</param>
        protected VirtualButton(string shortName, VirtualButtonType type, int id, bool isPositiveAndNegative = false)
        {
            Id = (int) type | id;
            Type = type;
            ShortName = shortName;
            IsPositiveAndNegative = isPositiveAndNegative;
            Index = Id & TypeIdMask;
        }


        /// <summary>
        ///   Unique Id for a particular button.
        /// </summary>
        /// <seealso cref="Type"/>
        public readonly int Id;

        /// <summary>
        ///   Gets the full name of this button.
        /// </summary>
        public string Name
        {
            get
            {
                if (name is null)
                    name = BuildButtonName();

                return name;
            }
        }

        /// <summary>
        ///   The short name of this button.
        /// </summary>
        public readonly string ShortName;

        /// <summary>
        ///   The type of this button.
        /// </summary>
        public readonly VirtualButtonType Type;

        /// <summary>
        ///   A value indicating whether this button supports positive and negative values.
        /// </summary>
        public readonly bool IsPositiveAndNegative;

        protected readonly int Index;

        private string name;

        public override string ToString() => Name;

        /// <summary>
        ///   Implements the + operator to combine two <see cref="VirtualButton"/>s.
        /// </summary>
        /// <param name="left">The left virtual button.</param>
        /// <param name="right">The right virtual button.</param>
        /// <returns>A <see cref="IVirtualButton"/> representing the combination of the specified virtual buttons.</returns>
        public static IVirtualButton operator +(IVirtualButton left, VirtualButton right)
        {
            if (left is null)
            {
                return right;
            }

            return right is null ? left : new VirtualButtonGroup { left, right };
        }

        /// <summary>
        ///   Gets all registered <see cref="VirtualButton"/>s.
        /// </summary>
        /// <value>A collection of the registered virtual buttons.</value>
        public static IReadOnlyCollection<VirtualButton> Registered
        {
            get
            {
                EnsureInitialize();
                return registeredReadOnly;
            }
        }

        /// <summary>
        ///   Finds a virtual button by the specified name.
        /// </summary>
        /// <param name="name">The name of the virtual button.</param>
        /// <returns>
        ///   A <see cref="VirtualButton"/> with the specified <paramref name="name"/>; or <c>null</c> if no match is found.
        /// </returns>
        public static VirtualButton Find(string name)
        {
            EnsureInitialize();

            mapName.TryGetValue(name, out VirtualButton virtualButton);
            return virtualButton;
        }

        /// <summary>
        ///   Finds a virtual button by the specified id.
        /// </summary>
        /// <param name="id">The id of the virtual button.</param>
        /// <returns>
        ///   A <see cref="VirtualButton"/> with the specified <paramref name="id"/>; or <c>null</c> if no match is found.
        /// </returns>
        public static VirtualButton Find(int id)
        {
            EnsureInitialize();

            mapIp.TryGetValue(id, out VirtualButton virtualButton);
            return virtualButton;
        }

        /// <summary>
        ///   Gets the value associated with this virtual button from an input manager.
        /// </summary>
        /// <param name="manager">The input manager.</param>
        /// <returns>Value of the virtual button.</returns>
        public abstract float GetValue(InputManager manager);

        /// <summary>
        ///   Gets a value that indicates whether the button is currently down.
        /// </summary>
        /// <param name="manager">The input manager</param>
        /// <returns><c>true</c> if the button is currently down; <c>false</c> otherwise.</returns>
        public abstract bool IsDown(InputManager manager);

        /// <summary>
        ///   Gets a value that indicates whether the button has been pressed since the last frame.
        /// </summary>
        /// <param name="manager">The input manager</param>
        /// <returns><c>true</c> if the button has been pressed; <c>false</c> otherwise.</returns>
        public abstract bool IsPressed(InputManager manager);

        /// <summary>
        ///   Gets a value that indicates whether the button has been released since the last frame.
        /// </summary>
        /// <param name="manager">The input manager</param>
        /// <returns><c>true</c> if the button has been released; <c>false</c> otherwise.</returns>
        public abstract bool IsReleased(InputManager manager);

        protected virtual string BuildButtonName()
        {
            return Type.ToString() + "." + ShortName;
        }

        private static void EnsureInitialize()
        {
            lock (mapIp)
            {
                if (mapIp.Count == 0)
                {
                    RegisterFromType(typeof(Keyboard));
                    RegisterFromType(typeof(GamePad));
                    RegisterFromType(typeof(Mouse));
                    registeredReadOnly = registered;
                }
            }
        }

        private static void RegisterFromType(Type type)
        {
            foreach (var fieldInfo in type.GetTypeInfo().DeclaredFields)
            {
                if (fieldInfo.IsStatic && typeof(VirtualButton).IsAssignableFrom(fieldInfo.FieldType))
                {
                    Register((VirtualButton) fieldInfo.GetValue(null));
                }
            }
        }

        private static void Register(VirtualButton virtualButton)
        {
            if (!mapIp.ContainsKey(virtualButton.Id))
            {
                mapIp.Add(virtualButton.Id, virtualButton);
                registered.Add(virtualButton);
            }

            if (!mapName.ContainsKey(virtualButton.Name))
            {
                mapName.Add(virtualButton.Name, virtualButton);
            }
        }
    }
}
