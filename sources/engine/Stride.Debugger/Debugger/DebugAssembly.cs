// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Stride.Debugger.Target
{
    /// <summary>
    ///   Represents a loaded assembly in the debug process.
    /// </summary>
    [Serializable]
    public struct DebugAssembly : IEquatable<DebugAssembly>
    {
        private readonly int id;

        public static readonly DebugAssembly Empty = new DebugAssembly(0);

        internal DebugAssembly(int id)
        {
            this.id = id;
        }

        public bool Equals(DebugAssembly other)
        {
            return id == other.id;
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;

            return obj is DebugAssembly assembly && Equals(assembly);
        }

        public override int GetHashCode()
        {
            return id;
        }
    }
}
