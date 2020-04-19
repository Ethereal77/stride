// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Xenko.Core.AssemblyProcessor
{
    [Flags]
    public enum ComplexTypeSerializerFlags
    {
        SerializePublicFields = 1,
        SerializePublicProperties = 2,

        /// <summary>
        /// If the member has DataMemberIgnore and DataMemberUpdatable, it will be included
        /// </summary>
        Updatable = 4,
    }
}
