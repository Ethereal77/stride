// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Stride.Core.Shaders.Utility
{
    public partial class MessageCode
    {
        // Errors
        public static readonly MessageCode ErrorMatrixInvalidMemberReference    = new MessageCode("E0100", "Invalid member reference [{0}] for matrix type");
        public static readonly MessageCode ErrorMatrixInvalidIndex              = new MessageCode("E0101", "Invalid index [{0}] for matrix type member access. Must be in the range [{1},{2}]  member for array type");
    }
}
