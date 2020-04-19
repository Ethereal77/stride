// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Rendering
{
    /// <summary>
    /// Handle used to query logical group information.
    /// </summary>
    public struct LogicalGroupReference
    {
        public static readonly LogicalGroupReference Invalid = new LogicalGroupReference(-1);

        internal int Index;

        internal LogicalGroupReference(int index)
        {
            Index = index;
        }
    }
}
