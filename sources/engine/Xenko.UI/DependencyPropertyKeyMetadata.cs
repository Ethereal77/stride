// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Xenko.Core;

namespace Xenko.UI
{
    [Flags]
    public enum DependencyPropertyFlags
    {
        Default = 0,
        ReadOnly = 0x1,
        Attached = 0x2,
    }

    public class DependencyPropertyKeyMetadata : PropertyKeyMetadata
    {
        public static readonly DependencyPropertyKeyMetadata Attached = new DependencyPropertyKeyMetadata(DependencyPropertyFlags.Attached);

        public static readonly DependencyPropertyKeyMetadata AttachedReadOnly = new DependencyPropertyKeyMetadata(DependencyPropertyFlags.Attached | DependencyPropertyFlags.ReadOnly);

        public static readonly DependencyPropertyKeyMetadata Default = new DependencyPropertyKeyMetadata(DependencyPropertyFlags.Default);

        public static readonly DependencyPropertyKeyMetadata ReadOnly = new DependencyPropertyKeyMetadata(DependencyPropertyFlags.ReadOnly);

        internal DependencyPropertyKeyMetadata(DependencyPropertyFlags flags)
        {
            Flags = flags;
        }

        public DependencyPropertyFlags Flags { get; }
    }
}
