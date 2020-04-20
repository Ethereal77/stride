// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Stride.Rendering
{
    public struct PermutationParameter<T>
    {
        internal readonly int BindingSlot;
        internal readonly int Count;

        internal PermutationParameter(int bindingSlot, int count)
        {
            this.BindingSlot = bindingSlot;
            this.Count = count;
        }
    }
}
