// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

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

        public static readonly DebugAssembly Empty = new(0);

        internal DebugAssembly(int id)
        {
            this.id = id;
        }

        public bool Equals(DebugAssembly other) => id == other.id;

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;

            return obj is DebugAssembly assembly && Equals(assembly);
        }

        public override int GetHashCode() => id;
    }
}
